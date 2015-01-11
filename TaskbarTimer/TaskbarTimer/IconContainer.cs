using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TaskbarTimer {
    /// <summary>
    /// Contains the icon associated with the application
    /// </summary>
    static class IconContainer {
        /// <summary>
        /// Application icon
        /// </summary>
        private static Icon _icon;

        /// <summary>
        /// Gets the application icon 
        /// </summary>
        public static Icon AppIcon {
            get {
                if (_icon == null) {
                    _icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                }
                return _icon;
            }
        }
    }
}