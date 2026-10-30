using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Triggernometry.Core;

namespace Triggernometry.UI.Aura
{

    internal sealed class AuraImage : Aura
    {

        internal string ImageFilenameExpression { get; set; }

        private string _ImageFileName;
        internal string ImageFileName
        {
            get
            {
                return _ImageFileName;
            }
            set
            {
                if (value != _ImageFileName)
                {
                    Changed = true;
                    _ImageFileName = value;
                }
            }
        }

        private PictureBoxSizeMode _Display;
        internal PictureBoxSizeMode Display
        {
            get
            {
                return _Display;
            }
            set
            {
                if (value != _Display)
                {
                    Changed = true;
                    _Display = value;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        internal static string GetImageFilename(RealPlugin plug, string ifn)
        {
            Uri uri = new Uri(ifn);
            return uri.IsFile ? ifn : plug.GetCachedResource(uri,
                "TriggernometryRemoteImages", plug.Config.CacheImageExpiry, "Triggernometry Image Retriever");
        }

    }

}
