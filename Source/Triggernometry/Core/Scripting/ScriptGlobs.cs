using System;
using System.Collections.Generic;
using Triggernometry.Core.Variables;

namespace Triggernometry.Core.Scripting
{
    public class ScriptGlobs
    {
        public ScriptContextHelper TriggernometryHelpers { get; set; } = null;

        internal ScriptGlobs(Context ctx)
        {
            TriggernometryHelpers = new ScriptContextHelper()
            {
                CurrentContext = ctx
            };
        }
    }
}
