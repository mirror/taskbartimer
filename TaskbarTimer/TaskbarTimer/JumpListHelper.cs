using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace TaskbarTimer {
    /// <summary>
    /// Class contains helper methods for jump-list 
    /// </summary>
    public class JumpListHelper {
        /// <summary>
        /// Icon reference for all elements in the jump-list 
        /// </summary>
        private static IconReference? _icon;
        
        /// <summary>
        /// The jump-list itself 
        /// </summary>
        private JumpList _jumpList;

        /// <summary>
        /// Gets the icon for the jump-link 
        /// </summary>
        private static IconReference Icon {
            get {
                if (_icon == null) {
                    string pathToIcon = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Timer.ico");
                    int iconNum = 0;

                    if (!File.Exists(pathToIcon)) {
                        pathToIcon = Path.Combine(KnownFolders.System.Path, "shell32.dll");
                        iconNum = 20;
                    }
                    _icon = new IconReference(pathToIcon, iconNum);
                }
                return _icon.Value;
            }
        }

        /// <summary>
        /// Gets or initializes the jump-list 
        /// </summary>
        private JumpList JumpList {
            get {
                if (_jumpList == null) {
                    _jumpList = JumpList.CreateJumpList();
                }
                return _jumpList;
            }
        }

        /// <summary>
        /// Configures the jump list 
        /// </summary>
        public void ConfigureJumpList() {
            JumpList.AddUserTasks(new[] { GetMinutesLink(5, null, Icon), GetMinutesLink(10, null, Icon), GetMinutesLink(15, null, Icon), GetMinutesLink(20, null, Icon), GetMinutesLink(30, null, Icon) });
        }

        /// <summary>
        /// Adds a link for custom timer 
        /// </summary>
        /// <param name="minutes">The number of minutes</param>
        /// <param name="timerName">Name of the timer</param>
        public void AddCustomLink(int minutes, string timerName) {
            JumpList.AddUserTasks(new[] { GetMinutesLink(minutes, timerName, Icon) });
        }

        /// <summary>
        /// Saves the jump-list
        /// </summary>
        public void Save() {
            JumpList.Refresh();
        }

        /// <summary>
        /// Adds link "N minutes timer"
        /// </summary>
        /// <param name="minutes">The number of minutes</param>
        /// <param name="timerName">Name of the timer</param>
        /// <param name="icon">The icon for the jump-list</param>
        private static JumpListLink GetMinutesLink(int minutes, string timerName, IconReference icon) {
            string title = string.Format("New {0} minute{1} timer", minutes, IsPlural(minutes) ? "s" : "");
            string arguments = " -minutes=" + minutes;
            if (!string.IsNullOrEmpty(timerName) && timerName != TimerOptions.Default.TimerName) {
                arguments += string.Format(" -name=\"{0}\"", timerName);
                title += string.Format(" ({0})", timerName);
            }
            return new JumpListLink(Application.ExecutablePath, title) { IconReference = icon, Arguments = arguments};
        }

        /// <summary>
        /// Determine whether we need add -s ending 
        /// </summary>
        /// <param name="minutes">Number of minutes</param>
        /// <returns>Is the number plural</returns>
        private static bool IsPlural(int minutes) {
            return !(minutes == 1 || (minutes%100 != 11 && minutes%10 == 1));
        }
    }
}
