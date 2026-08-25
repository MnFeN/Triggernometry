using System;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Discord webhook operation
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.RemoteControl)]
    [XmlRoot(ElementName = "DiscordWebhook")]
    public class ActionDiscordWebhook : ActionBase
    {

        #region Properties
        /*
        public enum MethodEnum
        {
            POST,
            GET
        }
        */

        /// <summary>
        /// Discord webhook URL
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public string WebhookURL { get; set; } = "";

        [XmlAttribute("WebhookURL")]
        public string Xml_WebhookURL
        {
            get => XmlAttr.String(WebhookURL);
            set => WebhookURL = value;
        }

        /// <summary>
        /// Message to send to the webhook
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string Message { get; set; } = "";

        [XmlAttribute("Message")]
        public string Xml_Message
        {
            get => XmlAttr.String(Message);
            set => Message = value;
        }

        /// <summary>
        /// If set, sent telegram will be flagged as a TTS message
        /// </summary>
        [XmlIgnore]
        [Action(order: 3)]
        public bool UseTTS { get; set; } = false;

        [XmlAttribute("UseTTS")]
        public string Xml_UseTTS
        {
            get => XmlAttr.Bool(UseTTS, false);
            set => UseTTS = XmlAttr.Bool(value);
        }


        #endregion

        #region Implementation

        internal override string DescribeImplementation()
        {
            if (UseTTS == true)
            {
                return I18n.Translate("internal/Action/descdiscordttsmsg", "send TTS message ({0}) to Discord webhook ({1})", Message, WebhookURL);
            }
            return I18n.Translate("internal/Action/descdiscordmsg", "send message ({0}) to Discord webhook ({1})", Message, WebhookURL);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            string msg = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Message);
            string url = ctx.EvaluateStringExpression(ActionContextLogger, ctx, WebhookURL);
            if (UseTTS == true)
            {
                if (msg.Length > 1970)
                {
                    msg = msg.Substring(0, 1970);
                    AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/warndiscordtrunc", "Discord message too long, capping to {0}", msg.Length));
                }
                var wh = new JavaScriptSerializer().Serialize(new { content = msg, tts = true });
                SendJson(ctx, ActionOld.HTTPMethodEnum.POST, url, wh, null, true);
            }
            else
            {
                if (msg.Length > 1980)
                {
                    msg = msg.Substring(0, 1980);
                    AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/warndiscordtrunc", "Discord message too long, capping to {0}", msg.Length));
                }
                var wh = new JavaScriptSerializer().Serialize(new { content = msg });
                SendJson(ctx, ActionOld.HTTPMethodEnum.POST, url, wh, null, true);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionDiscordWebhook(ActionOld oldAction)
        {
            var action = new ActionDiscordWebhook();
            oldAction.CopyCommonPropertiesTo(action);
            action.WebhookURL = oldAction._DiscordWebhookURL;
            action.Message = oldAction._DiscordWebhookMessage;
            action.UseTTS = oldAction._DiscordTts;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionDiscordWebhook action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.DiscordWebhook;
            oldAction._DiscordWebhookURL = action.WebhookURL;
            oldAction._DiscordWebhookMessage = action.Message;
            oldAction._DiscordTts = action.UseTTS;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
