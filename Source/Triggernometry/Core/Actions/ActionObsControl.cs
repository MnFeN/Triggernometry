using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.PluginBridges.ExternalTools;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// OBS remote control operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.RemoteControl)]
    [XmlRoot(ElementName = "ObsControl")]
    internal class ActionObsControl : ActionBase
    {

        #region Properties

        /// <summary>
        /// OBS control operations
        /// </summary>
        public enum OperationEnum
        {
            StartStreaming,
            StopStreaming,
            ToggleStreaming,
            StartRecording,
            StopRecording,
            ToggleRecording,
            RestartRecording,
            RestartRecordingIfActive,
            ResumeRecording,
            PauseRecording,
            ToggleRecordPause,
            StartReplayBuffer,
            StopReplayBuffer,
            ToggleReplayBuffer,
            SaveReplayBuffer,
            SetScene,
            ShowSource,
            HideSource,
            JSONPayload
        }

        /// <summary>
        /// Type of the OBS control operation
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public OperationEnum Operation { get; set; } = OperationEnum.StartStreaming;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.StartStreaming);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// OBS WebSocket endpoint
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string Endpoint { get; set; } = @"ws://${_const[OBSWebsocketEndpoint]}:${_const[OBSWebsocketPort]}";

        [XmlAttribute("Endpoint")]
        public string Xml_Endpoint
        {
            get => XmlAttr.String(Endpoint, @"ws://${_const[OBSWebsocketEndpoint]}:${_const[OBSWebsocketPort]}");
            set => Endpoint = value;
        }

        /// <summary>
        /// Optional password for the OBS endpoint
        /// </summary>
        [XmlIgnore]
        [Action(order: 3)]
        public string Password { get; set; } = @"${_const[OBSWebsocketPassword]}";

        [XmlAttribute("Password")]
        public string Xml_Password
        {
            get => XmlAttr.String(Password, @"${_const[OBSWebsocketPassword]}");
            set => Password = value;
        }

        /// <summary>
        /// Name of the scene referenced in some operations
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string SceneName { get; set; } = "";

        [XmlAttribute("SceneName")]
        public string Xml_SceneName
        {
            get => XmlAttr.String(SceneName);
            set => SceneName = value;
        }

        /// <summary>
        /// Name of the source referenced in some operations
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)]
        public string SourceName { get; set; } = "";

        [XmlAttribute("SourceName")]
        public string Xml_SourceName
        {
            get => XmlAttr.String(SourceName);
            set => SourceName = value;
        }

        /// <summary>
        /// Custom JSON payload
        /// </summary>
        [XmlIgnore]
        [Action(order: 6)]
        public string JSONPayload { get; set; } = "";

        [XmlAttribute("JSONPayload")]
        public string Xml_JSONPayload
        {
            get => XmlAttr.String(JSONPayload);
            set => JSONPayload = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            switch (Operation)
            {
                case OperationEnum.StartStreaming:
                    return I18n.Translate("internal/Action/descobsstartstream", "start streaming on OBS");
                case OperationEnum.StopStreaming:
                    return I18n.Translate("internal/Action/descobsstopstream", "stop streaming on OBS");
                case OperationEnum.ToggleStreaming:
                    return I18n.Translate("internal/Action/descobstogglestream", "start/stop streaming on OBS (toggle)");
                case OperationEnum.StartRecording:
                    return I18n.Translate("internal/Action/descobsstartrecord", "start recording on OBS");
                case OperationEnum.StopRecording:
                    return I18n.Translate("internal/Action/descobsstoprecord", "stop recording on OBS");
                case OperationEnum.ToggleRecording:
                    return I18n.Translate("internal/Action/descobstogglerecord", "start/stop recording on OBS (toggle)");
                case OperationEnum.RestartRecording:
                    return I18n.Translate("internal/Action/descobsrestartrecord", "stop then start recording on OBS");
                case OperationEnum.RestartRecordingIfActive:
                    return I18n.Translate("internal/Action/descobsrestartrecordifactive", "stop then start recording on OBS (if currently recording)");
                case OperationEnum.ResumeRecording:
                    return I18n.Translate("internal/Action/descobsresumerecord", "resume recording on OBS");
                case OperationEnum.PauseRecording:
                    return I18n.Translate("internal/Action/descobspauserecord", "pause recording on OBS");
                case OperationEnum.ToggleRecordPause:
                    return I18n.Translate("internal/Action/descobstogglerecordpause", "resume/pause recording on OBS (toggle)");
                case OperationEnum.StartReplayBuffer:
                    return I18n.Translate("internal/Action/descobsstartreplay", "start OBS replay buffer");
                case OperationEnum.StopReplayBuffer:
                    return I18n.Translate("internal/Action/descobsstopreplay", "stop OBS replay buffer");
                case OperationEnum.ToggleReplayBuffer:
                    return I18n.Translate("internal/Action/descobstogglereplay", "start/stop OBS replay buffer (toggle)");
                case OperationEnum.SaveReplayBuffer:
                    return I18n.Translate("internal/Action/descobssavereplay", "save OBS replay buffer");
                case OperationEnum.SetScene:
                    return I18n.Translate("internal/Action/descobssetscene", "set current OBS scene to ({0})", SceneName);
                case OperationEnum.ShowSource:
                    return I18n.Translate("internal/Action/descobsshowsource", "show source ({0}) on OBS scene ({1})", SourceName, SceneName);
                case OperationEnum.HideSource:
                    return I18n.Translate("internal/Action/descobshidesource", "hide source ({0}) on OBS scene ({1})", SourceName, SceneName);
                case OperationEnum.JSONPayload:
                    return I18n.Translate("internal/Action/descobsjsonpayload", "Send custom JSON payload to OBS");
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            ObsController obsController = ctx.Plugin._obs;
            if (obsController != null)
            {
                string endpoint = "";
                if (!string.IsNullOrWhiteSpace(Endpoint))
                {
                    endpoint = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Endpoint);
                }
                else
                {
                    var constants = RealPlugin.Instance.cfg.Constants;
                    if (constants.TryGetValue("OBSWebsocketEndpoint", out var e) && constants.TryGetValue("OBSWebsocketPort", out var p))
                        endpoint = $"ws://{e}:{p}";
                }

                string password = "";
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    password = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Password);
                }
                else
                {
                    var constants = RealPlugin.Instance.cfg.Constants;
                    if (constants.TryGetValue("OBSWebsocketPassword", out var pw))
                        password = pw.ToString();
                }

                lock (obsController)
                {
                    if (ObsConnector(ctx, endpoint, password) != true)
                        return; // already complaint about errors
                    try
                    {
                        switch (Operation)
                        {
                            case OperationEnum.StartStreaming:
                                obsController.StartStreaming();
                                break;
                            case OperationEnum.StopStreaming:
                                obsController.StopStreaming();
                                break;
                            case OperationEnum.ToggleStreaming:
                                obsController.ToggleStreaming();
                                break;
                            case OperationEnum.StartRecording:
                                obsController.StartRecording();
                                break;
                            case OperationEnum.StopRecording:
                                obsController.StopRecording();
                                break;
                            case OperationEnum.ToggleRecording:
                                obsController.ToggleRecording();
                                break;
                            case OperationEnum.RestartRecording:
                                obsController.RestartRecording();
                                break;
                            case OperationEnum.RestartRecordingIfActive:
                                obsController.RestartRecordingIfActive();
                                break;
                            case OperationEnum.ResumeRecording:
                                obsController.ResumeRecording();
                                break;
                            case OperationEnum.PauseRecording:
                                obsController.PauseRecording();
                                break;
                            case OperationEnum.ToggleRecordPause:
                                obsController.ToggleRecordPause();
                                break;
                            case OperationEnum.StartReplayBuffer:
                                obsController.StartReplayBuffer();
                                break;
                            case OperationEnum.StopReplayBuffer:
                                obsController.StopReplayBuffer();
                                break;
                            case OperationEnum.ToggleReplayBuffer:
                                obsController.ToggleReplayBuffer();
                                break;
                            case OperationEnum.SaveReplayBuffer:
                                obsController.SaveReplayBuffer();
                                break;
                            case OperationEnum.SetScene:
                                {
                                    string scn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, SceneName);
                                    obsController.SetCurrentScene(scn);
                                }
                                break;
                            case OperationEnum.ShowSource:
                                {
                                    string scn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, SceneName);
                                    string src = ctx.EvaluateStringExpression(ActionContextLogger, ctx, SourceName);
                                    obsController.ShowHideSource(scn, src, true);
                                }
                                break;
                            case OperationEnum.HideSource:
                                {
                                    string scn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, SceneName);
                                    string src = ctx.EvaluateStringExpression(ActionContextLogger, ctx, SourceName);
                                    obsController.ShowHideSource(scn, src, false);
                                }
                                break;
                            case OperationEnum.JSONPayload:
                                {
                                    string json = ctx.EvaluateStringExpression(ActionContextLogger, ctx, JSONPayload);
                                    obsController.JSONPayload(json);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Action/obscontrolexception", "Can't execute OBS control action due to exception: {0}" + ex.Message));
                    }
                }
            }
        }

        #endregion

    }

}
