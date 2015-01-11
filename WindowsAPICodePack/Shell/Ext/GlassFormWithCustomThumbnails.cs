using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Microsoft.WindowsAPICodePack.Shell.Ext {
    /// <summary>
    /// Represents a GlassWindows that has ability to change thumbnail image for itself 
    /// </summary>
    public class GlassFormWithCustomThumbnails : GlassForm {
        /// <summary>
        /// Enables custom thumbnails for the form 
        /// </summary>
        /// <param name="e">The arguments for this event</param>
        protected override void OnLoad(EventArgs e) {
            if (!DesignMode) {
                TabbedThumbnailNativeMethods.EnableCustomWindowPreview(Handle, true);
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// Handles taskbar-related messages 
        /// </summary>
        /// <param name="m">A message</param>
        protected override void WndProc(ref System.Windows.Forms.Message m) {
            if (m.Msg == (int)TaskbarNativeMethods.WM_DWMSENDICONICTHUMBNAIL) {
                int width = (int)((long)m.LParam >> 16);
                int height = (int)(((long)m.LParam) & (0xFFFF));
                Size requestedSize = new Size(width, height);
                using (Bitmap iconicThumb = GetIconicThumbnail(requestedSize)) {
                    TabbedThumbnailNativeMethods.SetIconicThumbnail(Handle, iconicThumb.GetHbitmap());
                }
            } else if (m.Msg == (int)TaskbarNativeMethods.WM_DWMSENDICONICLIVEPREVIEWBITMAP) {
                using (Bitmap peekBitmap = GetPeekBitmap()) {
                    TabbedThumbnailNativeMethods.SetPeekBitmap(Handle, peekBitmap.GetHbitmap(), false);
                }
            } 

            base.WndProc(ref m);
        }

        /// <summary>
        /// Invalidates the thumbnails 
        /// </summary>
        protected void InvalidateThumbnails() {
            TabbedThumbnailNativeMethods.DwmInvalidateIconicBitmaps(Handle);
        }

        /// <summary>
        /// Event to set peek bitmap 
        /// </summary>
        protected event GetPeekBitmapDelegate GetPeekBitmap;

        /// <summary>
        /// Event to set the iconic thumbnail 
        /// </summary>
        protected event GetIconicThumbnailDelegate GetIconicThumbnail;

    }
}
