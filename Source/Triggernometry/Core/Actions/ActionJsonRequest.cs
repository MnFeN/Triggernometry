using System;
using System.Collections.Generic;
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
    /// JSON remote request
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Networking)]
    [XmlRoot(ElementName = "JsonRequest")]
    internal class ActionJsonRequest : ActionBase
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
        [Action(order: 1)]
        public string Endpoint { get; set; } = "";

        [XmlAttribute("Endpoint")]
        public string Xml_Endpoint
        {
            get => XmlAttr.String(Endpoint);
            set => Endpoint = value;
        }

        /// <summary>
        /// Request method to use
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
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
        [Action(order: 3)]
        public string Payload { get; set; } = "";

        [XmlAttribute("Payload")]
        public string Xml_Payload
        {
            get => XmlAttr.String(Payload);
            set => Payload = value;
        }

        /// <summary>
        /// Header expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string Headers { get; set; } = "";

        [XmlAttribute("Headers")]
        public string Xml_Headers
        {
            get => XmlAttr.String(Headers);
            set => Headers = value;
        }

        /// <summary>
        /// Scalar variable in which the result of the request will be stored
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)]
        public string ResultVariable { get; set; } = "";

        [XmlAttribute("ResultVariable")]
        public string Xml_ResultVariable
        {
            get => XmlAttr.String(ResultVariable);
            set => ResultVariable = value;
        }

        /// <summary>
        /// Expression to be used when the result of the request is intended to be fired as a log event
        /// </summary>
        [XmlIgnore]
        [Action(order: 6)]
        public string FiringExpression { get; set; } = "";

        [XmlAttribute("FiringExpression")]
        public string Xml_FiringExpression
        {
            get => XmlAttr.String(FiringExpression);
            set => FiringExpression = value;
        }

        /// <summary>
        /// If set, Triggernometry will check its cache for a similar request and return that
        /// </summary>
        [XmlIgnore]
        [Action(order: 7)]
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
        [Action(order: 8)] // todo need to couple this with variable on editor
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
            string cache = I18n.TrlCacheFile(UseCache);
            if (FiringExpression != null && FiringExpression.Trim().Length > 0)
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

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            string response = "";
            int responseCode = 0;
            string endpoint = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Endpoint);
            string payload = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Payload);
            string headers = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Headers).Trim();
            string varname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, ResultVariable);
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
                string fh = RealPlugin.GenerateHash(endpointh + payloadh + headers);
                string fn = Path.Combine(ctx.Plugin.ConfigPath, "TriggernometryJsonCache");
                if (Directory.Exists(fn) == false)
                {
                    Directory.CreateDirectory(fn);
                }
                fn = Path.Combine(fn, fh + ".json");
                bool fromcache = false;
                if (File.Exists(fn) == true)
                {
                    FileInfo fi = new FileInfo(fn);
                    DateTime dt = DateTime.Now.AddMinutes(0 - ctx.Plugin.cfg.CacheJsonExpiry);
                    if (fi.LastWriteTime > dt)
                    {
                        responseCode = (int)HttpStatusCode.OK;
                        response = File.ReadAllText(fn);
                        fromcache = true;
                    }
                }
                if (fromcache == false)
                {
                    Tuple<int, string> resp = SendJson(ctx, Method, endpoint, payload, headerslist, false);
                    responseCode = resp.Item1;
                    response = resp.Item2;
                    File.WriteAllText(fn, response);
                }
            }
            else
            {
                Tuple<int, string> resp = SendJson(ctx, Method, endpoint, payload, headerslist, false);
                responseCode = resp.Item1;
                response = resp.Item2;
            }
            if (varname != "")
            {
                VariableStore vs = ctx.Plugin.GetVariableStore(Persistent);
                lock (vs.Scalar) // verified
                {
                    if (vs.Scalar.ContainsKey(varname) == false)
                    {
                        vs.Scalar[varname] = new VariableScalar();
                    }
                    VariableScalar x = vs.Scalar[varname];
                    x.Value = response;
                    if (ctx.Trigger != null)
                    {
                        x.LastChanger = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe());
                    }
                    else
                    {
                        x.LastChanger = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe());
                    }
                    x.LastChanged = DateTime.Now;
                }
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarsetjson",
                    "{1}Scalar variable ({0}) value set to JSON response", varname, persist));
            }
            ctx.contextResponse = response;
            ctx.contextResponseCode = responseCode;
            if (FiringExpression != null && FiringExpression.Trim().Length > 0)
            {
                string firing = ctx.EvaluateStringExpression(ActionContextLogger, ctx, FiringExpression);
                if (firing.Length > 0)
                {
                    ctx.Plugin.LogLineQueuer(firing, "", LogEvent.SourceEnum.Log);
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
            action.ResultVariable = oldAction._JsonResultVariable;
            action.FiringExpression = oldAction._JsonFiringExpression;
            action.UseCache = oldAction._JsonCacheRequest;
            action.Persistent = oldAction._JsonResultVariablePersist;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionJsonRequest action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.GenericJson;
            oldAction._JsonEndpointExpression = action.Endpoint;
            oldAction._JsonOperationType = (ActionOld.HTTPMethodEnum)(int)action.Method;
            oldAction._JsonPayloadExpression = action.Payload;
            oldAction._JsonHeaderExpression = action.Headers;
            oldAction._JsonResultVariable = action.ResultVariable;
            oldAction._JsonFiringExpression = action.FiringExpression;
            oldAction._JsonCacheRequest = action.UseCache;
            oldAction._JsonResultVariablePersist = action.Persistent;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
