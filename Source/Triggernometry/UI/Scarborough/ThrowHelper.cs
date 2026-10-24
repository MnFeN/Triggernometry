using Triggernometry.Localization;
using System;

namespace Scarborough
{
    internal static class ThrowHelper
    {
        public static InvalidOperationException DeviceNotInitialized()
        {
            return new InvalidOperationException(I18n.Translate("internal/Graphics/uninitializeddevice", "The DirectX device is not initialized."));
        }

        public static InvalidOperationException UseBeginScene()
        {
            return new InvalidOperationException(I18n.Translate("internal/ThrowHelper/beginscenerequired", "Call BeginScene before drawing."));
        }
    }
}
