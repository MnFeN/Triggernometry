using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// JSON remote request
    /// </summary>
    [Action("Generic JSON operation", ActionAttribute.CategoryType.ExternalInteraction)]
    [XmlRoot(ElementName = "ActionJsonRequest")]
    public class ActionJsonRequest : ActionBase
    {

        #region Properties
        /// <summary>
        /// Request method
        /// </summary>
        public enum MethodEnum
        {
            POST,
            GET
        }

        /// <summary>
        /// Remote endpoint expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, Label = "Endpoint URL")]
        public string Endpoint { get; set; } = "";

        [XmlAttribute("Endpoint")]
        public string Xml_Endpoint
        {
            get => XmlAttr.String(Endpoint, "", omitWhitespace: false);
            set => Endpoint = value;
        }

        /// <summary>
        /// Request method to use
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(2, Label = "HTTP method",
            ChoiceLabels = new[]
        {
            "POST",
            "GET",
        })]
        public MethodEnum Method { get; set; } = MethodEnum.POST;

        [XmlAttribute("Method")]
        public string Xml_Method
        {
            get => XmlAttr.Enum(Method, MethodEnum.POST);
            set => Method = XmlAttr.Enum<MethodEnum>(value);
        }

        /// <summary>
        /// Payload expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(3, Label = "Payload to send")]
        public string Payload { get; set; } = "";

        [XmlAttribute("Payload")]
        public string Xml_Payload
        {
            get => XmlAttr.String(Payload, "", omitWhitespace: false);
            set => Payload = value;
        }

        /// <summary>
        /// Header expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(4, Label = "Headers to send")]
        public string Headers { get; set; } = "";

        [XmlAttribute("Headers")]
        public string Xml_Headers
        {
            get => XmlAttr.String(Headers, "", omitWhitespace: false);
            set => Headers = value;
        }

        /// <summary>
        /// Scalar variable in which the result of the request will be stored
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(5, Label = "Variable to store response to", Autofill = EditorAutofillType.Scalar)]
        public string ResponseVar { get; set; } = "";

        [XmlAttribute("ResponseVar")]
        public string Xml_ResponseVar
        {
            get => XmlAttr.String(ResponseVar, "", omitWhitespace: false);
            set => ResponseVar = value;
        }

        /// <summary>
        /// Expression to be used when the result of the request is intended to be fired as a log event
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(7, Label = "Response firing expression")]
        public string ResponseLogTemplate { get; set; } = "";

        [XmlAttribute("ResponseLogTemplate")]
        public string Xml_ResponseLogTemplate
        {
            get => XmlAttr.String(ResponseLogTemplate, "", omitWhitespace: false);
            set => ResponseLogTemplate = value;
        }

        /// <summary>
        /// If set, Triggernometry will check its cache for a similar request and return that
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [CheckBoxEditor(8, Label = "Cache response on disk")]
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
        [PersistenceEditor(6, nameof(ResponseVar), Label = "Variable to store response to")]
        public bool Persistent { get; set; } = false;

        [XmlAttribute("Persistent")]
        public string Xml_Persistent
        {
            get => XmlAttr.Bool(Persistent, false);
            set => Persistent = XmlAttr.Bool(value);
        }

        #endregion


        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var method = editor.GetEditor(nameof(Method));
            var json = editor.GetEditor(nameof(Payload));
            return () => editor.SetEnabled((MethodEnum)method.GetValue() == MethodEnum.POST, json);
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            string cache = I18n.TrlCacheFile(UseCache);
            if (ResponseLogTemplate != null && ResponseLogTemplate.Trim().Length > 0)
            {
                return I18n.Translate(
                    "internal/Action/descjsonsendrelay",
                    "send JSON payload to endpoint ({0}){1}, and relaying response for further processing",
                    Endpoint, cache
                );
            }
            else
            {
                return I18n.Translate(
                    "internal/Action/descjsonsend",
                    "send JSON payload to endpoint ({0}){1} and cache the response",
                    Endpoint, cache
                );
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string response = "";
            int responseCode = 0;
            string endpoint = EvalString(Endpoint, ctx);
            string payload = EvalString(Payload, ctx);
            string headers = EvalString(Headers, ctx).Trim();
            var (tgtName, tgtStore) = ResolveVariable(ctx, ResponseVar, Persistent);
            string persist = I18n.TrlVarPersist(Persistent);
            List<string> headerslist = new List<string>();
            if (headers.Length > 0)
            {
                headerslist.AddRange(headers.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }
            if (UseCache == true)
            {
                string endpointh = RealPlugin.GenerateHash(endpoint);
                string payloadh = RealPlugin.GenerateHash(payload);
                string headersh = RealPlugin.GenerateHash(headers);
                string fh = RealPlugin.GenerateHash(Method + endpointh + payloadh + headersh);
                string fn = Path.Combine(plug.ConfigPath, "TriggernometryJsonCache");
                if (Directory.Exists(fn) == false)
                {
                    Directory.CreateDirectory(fn);
                }
                fn = Path.Combine(fn, fh + ".json");
                bool isFromCache = false;
                if (File.Exists(fn) == true)
                {
                    FileInfo fi = new FileInfo(fn);
                    DateTime dt = DateTime.Now.AddMinutes(0 - plug.Config.CacheJsonExpiry);
                    if (fi.LastWriteTime > dt && fi.Length > 0)
                    {
                        responseCode = (int)HttpStatusCode.OK;
                        response = File.ReadAllText(fn);
                        isFromCache = true;
                    }
                }
                if (isFromCache == false)
                {
                    Tuple<int, string> resp = SendJson(ctx, Method, endpoint, payload, headerslist, false);
                    responseCode = resp.Item1;
                    response = resp.Item2;
                    // Cache files only store the body, so only cache non-empty 200 responses.
                    if (responseCode == (int)HttpStatusCode.OK && response.Length > 0)
                    {
                        File.WriteAllText(fn, response);
                    }
                }
            }
            else
            {
                Tuple<int, string> resp = SendJson(ctx, Method, endpoint, payload, headerslist, false);
                responseCode = resp.Item1;
                response = resp.Item2;
            }
            if (tgtName != "")
            {
                lock (tgtStore.Scalar)
                {
                    if (tgtStore.Scalar.ContainsKey(tgtName) == false)
                    {
                        tgtStore.Scalar[tgtName] = new VariableScalar();
                    }
                    VariableScalar x = tgtStore.Scalar[tgtName];
                    x.Value = response;
                    x.LastChanger = DescribeVariableChanger(ctx);
                    x.LastChanged = DateTime.Now;
                }
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarsetjson",
                    "{1}Scalar variable ({0}) value set to JSON response", tgtName, persist));
            }
            ctx.contextResponse = response;
            ctx.contextResponseCode = responseCode;
            ctx.contextJsonResponse = null;
            ctx.isContextJsonParsed = false;
            if (ResponseLogTemplate != null && ResponseLogTemplate.Trim().Length > 0)
            {
                string firing = EvalString(ResponseLogTemplate, ctx);
                if (firing.Length > 0)
                {
                    plug.LogLineQueuer(firing, "", LogEvent.SourceEnum.Log);
                }
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionJsonRequest(ActionOld oldAction)
        {
            var action = new ActionJsonRequest();
            oldAction.CopyCommonPropertiesTo(action);
            action.Endpoint = oldAction._JsonEndpointExpression;
            action.Method = (MethodEnum)(int)oldAction._JsonOperationType;
            action.Payload = oldAction._JsonPayloadExpression;
            action.Headers = oldAction._JsonHeaderExpression;
            action.ResponseVar = oldAction._JsonResultVariable;
            action.ResponseLogTemplate = oldAction._JsonFiringExpression;
            action.UseCache = oldAction._JsonCacheRequest;
            action.Persistent = oldAction._JsonResultVariablePersist;
            return action;
        }

        #endregion Old Action Converter

    }

}
