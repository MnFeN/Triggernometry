using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// File system operations
    /// </summary>
    [Action("File operation", ActionAttribute.CategoryType.SystemInteraction)]
    [XmlRoot(ElementName = "ActionDiskOperation")]
    public class ActionDiskOperation : ActionBase
    {

        #region Properties

        /// <summary>
        /// File system operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Read the contents of a file into a scalar variable
            /// </summary>
            ReadIntoVariable,
            /// <summary>
            /// Read the contents of a file into a list variable, where every line is its own index
            /// </summary>
            ReadIntoListVariable,
            /// <summary>
            /// Read the contents of a CSV file into a table variable
            /// </summary>
            ReadCSVIntoTableVariable
        }

        /// <summary>
        /// Type of the file system operation
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation type",
            ChoiceLabels = new[]
        {
            "Read file into scalar variable",
            "Read file lines into list variable",
            "Read CSV file into table variable",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.ReadIntoVariable;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.ReadIntoVariable);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// File name
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [FileEditor(2, EditorFileType.Any, Label = "File name")]
        public string Filename { get; set; } = "";

        [XmlAttribute("Filename")]
        public string Xml_Filename
        {
            get => XmlAttr.String(Filename, "", omitWhitespace: false);
            set => Filename = value;
        }

        /// <summary>
        /// Target variable name
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(3, Label = "Variable name")]
        public string TargetVar { get; set; } = "";

        [XmlAttribute("TargetVar")]
        public string Xml_TargetVar
        {
            get => XmlAttr.String(TargetVar, "", omitWhitespace: false);
            set => TargetVar = value;
        }

        /// <summary>
        /// If set, instructs Triggernometry to look at its own cache first for the file, reading that instead if found (applies to remote files)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [CheckBoxEditor(5, Label = "Cache file on disk")]
        public bool UseCache { get; set; } = false;

        [XmlAttribute("UseCache")]
        public string Xml_UseCache
        {
            get => XmlAttr.Bool(UseCache, false);
            set => UseCache = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [PersistenceEditor(4, nameof(TargetVar), Label = "Variable name")]
        public bool Persistent { get; set; } = false;

        [XmlAttribute("Persistent")]
        public string Xml_Persistent
        {
            get => XmlAttr.Bool(Persistent, false);
            set => Persistent = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            string persist = I18n.TrlVarPersist(Persistent);
            string cache = I18n.TrlCacheFile(UseCache);
            switch (Operation)
            {
                case OperationEnum.ReadIntoListVariable:
                    return I18n.Translate(
                        "internal/Action/descfilereadlistvar",
                        "read file ({0}) lines into {2}list variable ({1}){3}",
                        Filename, TargetVar, persist, cache
                    );
                case OperationEnum.ReadIntoVariable:
                    return I18n.Translate(
                        "internal/Action/descfilereadvar",
                        "read file ({0}) lines into {2}scalar variable ({1}){3}",
                        Filename, TargetVar, persist, cache
                    );
                case OperationEnum.ReadCSVIntoTableVariable:
                    return I18n.Translate(
                        "internal/Action/descfilereadcsvtable",
                        "read csv file ({0}) into {2}table variable ({1}){3}",
                        Filename, TargetVar, persist, cache
                    );
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }        

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string filename = EvalString(Filename, ctx);
            var (tgtName, tgtStore) = ResolveVariable(ctx, TargetVar, Persistent);
            string persist = I18n.TrlVarPersist(Persistent);
            string cache = I18n.TrlCacheFile(UseCache);
            if (Operation == OperationEnum.ReadCSVIntoTableVariable || 
                Operation == OperationEnum.ReadIntoListVariable || 
                Operation == OperationEnum.ReadIntoVariable)
            {
                Uri u = new Uri(filename);
                if (u.IsFile == false)
                {
                    string fn = Path.Combine(plug.ConfigPath, "TriggernometryFileCache");
                    if (Directory.Exists(fn) == false)
                    {
                        Directory.CreateDirectory(fn);
                    }
                    string ext = Path.GetExtension(u.LocalPath);
                    fn = Path.Combine(fn, RealPlugin.GenerateHash(u.AbsoluteUri) + Path.GetExtension(u.LocalPath));
                    bool isFromCache = false;
                    if (File.Exists(fn) == true && UseCache == true)
                    {
                        FileInfo fi = new FileInfo(fn);
                        DateTime dt = DateTime.Now.AddMinutes(0 - plug.Config.CacheFileExpiry);
                        if (fi.LastWriteTime > dt)
                        {
                            filename = fn;
                            isFromCache = true;
                        }
                    }
                    if (isFromCache == false)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.Headers["User-Agent"] = "Triggernometry File Retriever";
                            byte[] data = wc.DownloadData(u.AbsoluteUri);
                            File.WriteAllBytes(fn, data);
                            filename = fn;
                        }
                    }
                }
            }
            switch (Operation)
            {
                case OperationEnum.ReadCSVIntoTableVariable:
                    {
                        List<string[]> data = new List<string[]>();
                        int datawidth = 0;
                        using (StreamReader sr = new StreamReader(filename))
                        using (CsvReader csv = new CsvReader(sr, CultureInfo.InvariantCulture))
                        {
                            while (csv.Parser.Read() == true)
                            {
                                string[] x = csv.Parser.Record;
                                if (x.Length > datawidth)
                                {
                                    datawidth = x.Length;
                                }
                                data.Add(x);
                            }
                        }
                        string vtchanger = DescribeVariableChanger(ctx);
                        VariableTable vt = VariableTable.ActualBuild(data, datawidth, data.Count);
                        vt.LastChanged = DateTime.Now;
                        vt.LastChanger = vtchanger;
                        lock (tgtStore.Table)
                        {
                            tgtStore.Table[tgtName] = vt;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/filetableset",
                            "{2}Table variable ({0}) value read from CSV file ({1})", tgtName, filename, persist));
                    }
                    break;
                case OperationEnum.ReadIntoListVariable:
                    {
                        string[] data = File.ReadAllLines(filename);
                        lock (tgtStore.List)
                        {
                            if (tgtStore.List.ContainsKey(tgtName) == false)
                            {
                                tgtStore.List[tgtName] = new VariableList();
                            }
                            VariableList x = tgtStore.List[tgtName];
                            foreach (string dat in data)
                            {
                                x.Values.Add(new VariableScalar() { Value = dat });
                            }
                            // #18：复用变量修改来源文本的生成逻辑。
                            x.LastChanger = DescribeVariableChanger(ctx);
                            x.LastChanged = DateTime.Now;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/filelistset",
                            "{2}List variable ({0}) value read from file ({1})", tgtName, filename, persist));
                    }
                    break;
                case OperationEnum.ReadIntoVariable:
                    {
                        string data = File.ReadAllText(filename);
                        lock (tgtStore.Scalar)
                        {
                            if (tgtStore.Scalar.ContainsKey(tgtName) == false)
                            {
                                tgtStore.Scalar[tgtName] = new VariableScalar();
                            }
                            VariableScalar x = tgtStore.Scalar[tgtName];
                            x.Value = data;
                            // #18：复用变量修改来源文本的生成逻辑。
                            x.LastChanger = DescribeVariableChanger(ctx);
                            x.LastChanged = DateTime.Now;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/filescalarset",
                            "{2}Scalar variable ({0}) value read from file ({1})",
                            tgtName, filename, persist));
                    }
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionDiskOperation(ActionOld oldAction)
        {
            var action = new ActionDiskOperation();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._DiskFileOp;
            action.Filename = oldAction._DiskFileOpName;
            action.TargetVar = oldAction._DiskFileOpVar;
            action.UseCache = oldAction._DiskFileCache;
            action.Persistent = oldAction._DiskPersist;
            return action;
        }

        #endregion Old Action Converter

    }

}
