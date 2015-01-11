using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Shell.Ext {
    /// <summary>
    /// Must return a bitmap that will be shown to the user as peek bitmap 
    /// </summary>
    /// <returns>Peek bitmap</returns>
    public delegate Bitmap GetPeekBitmapDelegate();
}
