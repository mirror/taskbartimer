using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace TaskbarTimer {
    /// <summary>
    /// Main class of the application 
    /// </summary>
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Program arguments</param>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Check wether we are running on Windows 7 
            if (!TaskbarManager.IsPlatformSupported) {
                MessageBox.Show("This program requires Windows 7", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int minutes;
            TimerOptions options = TimerOptions.Default;

            if (!TryParseTimerOptionsFromArgs(args, options, out minutes)) {
                using (TimerTaskDialog dialog = new TimerTaskDialog()) {
                    if (dialog.Show() != TaskDialogResult.CustomButtonClicked) {
                        return;
                    }
                    minutes = dialog.Minutes;
                    options = dialog.TimerOptions;
                }
                if (minutes < 0) {
                    using (FrmOtherTime frm = new FrmOtherTime()) {
                        if (frm.ShowDialog() != DialogResult.OK) {
                            return;
                        }
                        minutes = frm.Minutes;
                    }
                }
            }

            if (minutes > 0) {
                Application.Run(new FrmMain(minutes, 0, options));
            }
        }

        /// <summary>
        /// Tries to parse the timer options from program arguments 
        /// </summary>
        /// <param name="args">Program arguments</param>
        /// <param name="options">Timer options</param>
        /// <param name="minutes">Minutes for the timer, or -1 if it's not set</param>
        /// <returns>Whether the options have been successfully parsed</returns>
        private static bool TryParseTimerOptionsFromArgs(ICollection<string> args, TimerOptions options, out int minutes) {
            minutes = -1;
            if (args.Count > 0) {
                foreach (string arg in args) {
                    if (arg.StartsWith("-minutes=")) {
                        int.TryParse(arg.Split('=')[1], out minutes);
                    } else if (arg.StartsWith("-name=")) {
                        options.TimerName = arg.Split('=')[1];
                    }
                }
            }
            return minutes > 0;
        }
    }
}
