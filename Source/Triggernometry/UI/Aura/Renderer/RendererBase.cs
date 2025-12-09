using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.UI.Aura;

namespace Triggernometry.UI.Aura.Renderer
{

    abstract internal class RendererBase : IDisposable
    {

        internal Manager Owner { get; set; }

        abstract public void Dispose();

        abstract internal void Initialize(Aura a);
        abstract internal void Render(Aura a);

    }

}
