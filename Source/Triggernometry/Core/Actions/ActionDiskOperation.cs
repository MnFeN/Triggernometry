using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// File system operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.File)]
    [XmlRoot(ElementName = "DiskOperation")]
    internal class ActionDiskOperation : ActionBase
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
        [Action(order: 1)]
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
        [Action(order: 2, specialtype: ActionAttribute.SpecialTypeEnum.FileSelector)]
        public string Filename { get; set; } = "";

        [XmlAttribute("Filename")]
        public string Xml_Filename
        {
            get => XmlAttr.String(Filename);
            set => Filename = value;
        }

        /// <summary>
        /// Target variable name
        /// </summary>
        [XmlIgnore]
        [Action(order: 3)]
        public string Variable { get; set; } = "";

        [XmlAttribute("Variable")]
        public string Xml_Variable
        {
            get => XmlAttr.String(Variable);
            set => Variable = value;
        }

        /// <summary>
        /// If set, instructs Triggernometry to look at its own cache first for the file, reading that instead if found (applies to remote files)
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
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
        [Action(order: 5)] // todo need to couple this with variable on editor
        public bool Persistent { get; set; } = false;

        [XmlAttribute("Persistent")]
        public string Xml_Persistent
        {
            get => XmlAttr.Bool(Persistent, false);
            set => Persistent = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            string persist = I18n.TrlVarPersist(Persistent);
            string cache = I18n.TrlCacheFile(UseCache);
            switch (Operation)
            {
                case OperationEnum.ReadIntoListVariable:
                    return I18n.Translate(
                        "internal/Action/descfilereadlistvar",
                        "read file ({0}) lines into {2}list variable ({1}){3}",
                        Filename, Variable, persist, cache
                    );
                case OperationEnum.ReadIntoVariable:
                    return I18n.Translate(
                        "internal/Action/descfilereadvar",
                        "read file ({0}) lines into {2}scalar variable ({1}){3}",
                        Filename, Variable, persist, cache
                    );
                case OperationEnum.ReadCSVIntoTableVariable:
                    return I18n.Translate(
                        "internal/Action/descfilereadcsvtable",
                        "read csv file ({0}) into {2}table variable ({1}){3}",
                        Filename, Variable, persist, cache
                    );
            }
            return "";
        }        

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            string filename = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Filename);
            string varname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Variable);
            string persist = I18n.TrlVarPersist(Persistent);
            string cache = I18n.TrlCacheFile(UseCache);
            VariableStore vs = Persistent == false ? ctx.Plugin.sessionvars : ctx.Plugin.cfg.PersistentVariables;
            if (Operation == OperationEnum.ReadCSVIntoTableVariable || Operation == OperationEnum.ReadIntoListVariable || Operation == OperationEnum.ReadIntoVariable)
            {
                Uri u = new Uri(filename);
                if (u.IsFile == false)
                {
                    string fn = Path.Combine(ctx.Plugin.ConfigPath, "TriggernometryFileCache");
                    if (Directory.Exists(fn) == false)
                    {
                        Directory.CreateDirectory(fn);
                    }
                    string ext = Path.GetExtension(u.LocalPath);
                    fn = Path.Combine(fn, RealPlugin.GenerateHash(u.AbsoluteUri) + Path.GetExtension(u.LocalPath));
                    bool fromcache = false;
                    if (File.Exists(fn) == true && UseCache == true)
                    {
                        FileInfo fi = new FileInfo(fn);
                        DateTime dt = DateTime.Now.AddMinutes(0 - ctx.Plugin.cfg.CacheFileExpiry);
                        if (fi.LastWriteTime > dt)
                        {
                            filename = fn;
                            fromcache = true;
                        }
                    }
                    if (fromcache == false)
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
                        {
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
                        }
                        VariableTable vt = vs.GetTableVariable(varname, true);
                        if (data.Count > 0 && datawidth > 0)
                        {
                            string vtchanger;
                            if (ctx.Trigger != null)
                            {
                                vtchanger = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe(ctx));
                            }
                            else
                            {
                                vtchanger = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe(ctx));
                            }
                            vt.Resize(datawidth, data.Count);
                            int y = 1;
                            foreach (string[] row in data)
                            {
                                for (int x = 0; x < row.Length; x++)
                                {
                                    vt.Set(x + 1, y, row[x], vtchanger);
                                }
                                y++;
                            }
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/filetableset",
                            "{2}Table variable ({0}) value read from CSV file ({1})", varname, filename, persist));
                    }
                    break;
                case OperationEnum.ReadIntoListVariable:
                    {
                        string[] data = File.ReadAllLines(filename);
                        lock (vs.List) // verified
                        {
                            if (vs.List.ContainsKey(varname) == false)
                            {
                                vs.List[varname] = new VariableList();
                            }
                            VariableList x = vs.List[varname];
                            foreach (string dat in data)
                            {
                                x.Push(new VariableScalar() { Value = dat }, "");
                            }
                            if (ctx.Trigger != null)
                            {
                                x.LastChanger = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe(ctx));
                            }
                            else
                            {
                                x.LastChanger = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe(ctx));
                            }
                            x.LastChanged = DateTime.Now;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/filelistset",
                            "{2}List variable ({0}) value read from file ({1})", varname, filename, persist));
                    }
                    break;
                case OperationEnum.ReadIntoVariable:
                    {
                        string data = File.ReadAllText(filename);
                        lock (vs.Scalar) // verified
                        {
                            if (vs.Scalar.ContainsKey(varname) == false)
                            {
                                vs.Scalar[varname] = new VariableScalar();
                            }
                            VariableScalar x = vs.Scalar[varname];
                            x.Value = data;
                            if (ctx.Trigger != null)
                            {
                                x.LastChanger = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe(ctx));
                            }
                            else
                            {
                                x.LastChanger = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe(ctx));
                            }
                            x.LastChanged = DateTime.Now;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/filescalarset",
                            "{2}Scalar variable ({0}) value read from file ({1})",
                            varname, filename, persist));
                    }
                    break;
            }
        }

        #endregion

    }

}
