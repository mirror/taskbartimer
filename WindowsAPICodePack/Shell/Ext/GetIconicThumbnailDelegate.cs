using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Shell.Ext {
    /// <summary>
    /// Must return a small thubmnail that will be shown in the taskbar 
    /// </summary>
    /// <param name="size">Requested size of the thumbnail</param>
    /// <returns>Iconic thumbnail</returns>
    public delegate Bitmap GetIconicThumbnailDelegate(Size size);
}
