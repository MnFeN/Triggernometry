using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Mouse operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Input)]
    [XmlRoot(ElementName = "Mouse")]
    internal class ActionMouse : ActionBase
    {

        #region Properties

        /// <summary>
        /// Mouse operations
        /// </summary>
        public enum OperationEnum
        {
            Move,
            LeftClick,
            MiddleClick,
            RightClick
        }

        /// <summary>
        /// Coordinate definitions
        /// </summary>
        public enum CoordinateEnum
        {
            /// <summary>
            /// Coordinates are in absolute screen space (0,0 being the top-left corner of screen)
            /// </summary>
            Absolute,
            /// <summary>
            /// Coordinates are relative to current mouse position
            /// </summary>
            Relative
        }

        /// <summary>
        /// Type of the mouse operation
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public OperationEnum Operation { get; set; } = OperationEnum.Move;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Move);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Coordinate system to use
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public CoordinateEnum Coordinate { get; set; } = CoordinateEnum.Absolute;

        [XmlAttribute("Coordinate")]
        public string Xml_Coordinate
        {
            get => XmlAttr.Enum(Coordinate, CoordinateEnum.Absolute);
            set => Coordinate = XmlAttr.Enum<CoordinateEnum>(value);
        }

        /// <summary>
        /// Mouse X position/offset
        /// </summary>
        [XmlIgnore]
        [Action(order: 3, typehint: typeof(int))]
        public string X { get; set; } = "0";

        [XmlAttribute("X")]
        public string Xml_X
        {
            get => XmlAttr.String(X, "0");
            set => X = value;
        }

        /// <summary>
        /// Mouse Y position/offset
        /// </summary>
        [XmlIgnore]
        [Action(order: 4, typehint: typeof(int))]
        public string Y { get; set; } = "0";

        [XmlAttribute("Y")]
        public string Xml_Y
        {
            get => XmlAttr.String(Y, "0");
            set => Y = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            string coorddesc = "";
            switch (Coordinate)
            {
                case CoordinateEnum.Absolute:
                    coorddesc = I18n.Translate("internal/Action/descmousecoordabsolute", "to absolute coordinates");
                    break;
                case CoordinateEnum.Relative:
                    coorddesc = I18n.Translate("internal/Action/descmousecoordrelative", "by relative coordinates");
                    break;
            }
            switch (Operation)
            {
                case OperationEnum.Move:
                    return I18n.Translate("internal/Action/descmousemove", "Move mouse {0} X: {1} Y: {2}", coorddesc, X, Y);
                case OperationEnum.LeftClick:
                    return I18n.Translate("internal/Action/descmouselmb", "Move mouse {0} X: {1} Y: {2} and left click", coorddesc, X, Y);
                case OperationEnum.MiddleClick:
                    return I18n.Translate("internal/Action/descmousemmb", "Move mouse {0} X: {1} Y: {2} and middle click", coorddesc, X, Y);
                case OperationEnum.RightClick:
                    return I18n.Translate("internal/Action/descmousermb", "Move mouse {0} X: {1} Y: {2} and right click", coorddesc, X, Y);
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            int mousex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, X);
            int mousey = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Y);
            WindowsUtils.MouseEventFlags flags = 0;
            switch (Coordinate)
            {
                case CoordinateEnum.Absolute:
                    flags |= WindowsUtils.MouseEventFlags.ABSOLUTE;
                    break;
                case CoordinateEnum.Relative:
                    break;
            }
            switch (Operation)
            {
                case OperationEnum.Move:
                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                    break;
                case OperationEnum.LeftClick:
                    Task.Run(() =>
                    {
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                        System.Threading.Thread.Sleep(10);
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.LEFTDOWN, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                        System.Threading.Thread.Sleep(10);
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.LEFTUP, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                    });
                    break;
                case OperationEnum.MiddleClick:
                    Task.Run(() =>
                    {
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                        System.Threading.Thread.Sleep(10);
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MIDDLEDOWN, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                        System.Threading.Thread.Sleep(10);
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MIDDLEUP, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                    });
                    break;
                case OperationEnum.RightClick:
                    Task.Run(() =>
                    {
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                        System.Threading.Thread.Sleep(10);
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.RIGHTDOWN, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                        System.Threading.Thread.Sleep(10);
                        WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.RIGHTUP, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                    });
                    break;
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionMouse(ActionOld oldAction)
        {
            var action = new ActionMouse();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._MouseOpType;
            action.Coordinate = (CoordinateEnum)(int)oldAction._MouseCoordType;
            action.X = oldAction._MouseX;
            action.Y = oldAction._MouseY;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionMouse action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Mouse;
            oldAction._MouseOpType = (ActionOld.MouseOpEnum)(int)action.Operation;
            oldAction._MouseCoordType = (ActionOld.MouseCoordEnum)(int)action.Coordinate;
            oldAction._MouseX = action.X;
            oldAction._MouseY = action.Y;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
