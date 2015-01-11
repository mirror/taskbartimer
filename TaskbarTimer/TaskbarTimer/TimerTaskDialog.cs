using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using TaskbarTimer.Properties;

namespace TaskbarTimer {
    /// <summary>
    /// Represents TaskDialod used in TaskbarTimer application 
    /// </summary>
    public class TimerTaskDialog : IDisposable {
        /// <summary>
        /// The dialog itself 
        /// </summary>
        private readonly TaskDialog _dialog;

        /// <summary>
        /// Interval for the fimer 
        /// </summary>
        private int _minutes;

        /// <summary>
        /// Additional timer options 
        /// </summary>
        private TimerOptions _options;

        /// <summary>
        /// Returns the number of selected minutes 
        /// </summary>
        public int Minutes {
            get { return _minutes; }
        }

        /// <summary>
        /// Additional timer options 
        /// </summary>
        public TimerOptions TimerOptions {
            get { return _options; }
        }

        /// <summary>
        /// Constructor. Initlalizes the _dialog 
        /// </summary>
        public TimerTaskDialog() {
            _dialog = new TaskDialog();
            _dialog.Caption = "Taskbar Timer";
            _dialog.Cancelable = true;
            _dialog.InstructionText = "Please select a time interval for taskbar timer";
            _dialog.FooterText = Resources.AboutText;
            _dialog.HyperlinksEnabled = true;
            _dialog.StandardButtons = TaskDialogStandardButtons.Close;

            TaskDialogCommandLink btn5Munutes = new TaskDialogCommandLink("btn5min", "5 minutes", "Set timer to 5 minutes and start the timer");
            btn5Munutes.Default = true;
            btn5Munutes.Click += (sender, e) => SetInterval(5);
            
            TaskDialogCommandLink btn10Munutes = new TaskDialogCommandLink("btn10min", "10 minutes", "Set timer to 10 minutes and start the timer");
            btn10Munutes.Click += (sender, e) => SetInterval(10);

            TaskDialogCommandLink btn30Munutes = new TaskDialogCommandLink("btn30min", "Half an hour", "Set timer to 30 minutes and start the timer");
            btn30Munutes.Click += (sender, e) => SetInterval(30);

            TaskDialogCommandLink btn60Munutes = new TaskDialogCommandLink("btn60min", "One hour", "Set timer to an hour and start the timer");
            btn60Munutes.Click += (sender, e) => SetInterval(60);

            TaskDialogCommandLink btnOther = new TaskDialogCommandLink("btnOther", "Other interval", "None of the above matches me. Please let me select a time interval by myself");
            btnOther.Click += (sender, e) => SelectInterval();

            TaskDialogCommandLink btnOptions = new TaskDialogCommandLink("btnOptions", "Options", "Configure additional options");
            btnOptions.Click += (sender, e) => ConfigureOptions();

            _dialog.Controls.Add(btn5Munutes);
            _dialog.Controls.Add(btn10Munutes);
            _dialog.Controls.Add(btn30Munutes);
            _dialog.Controls.Add(btn60Munutes);
            _dialog.Controls.Add(btnOther);
            _dialog.Controls.Add(btnOptions);
        }

        /// <summary>
        /// Set the interval to N minutes and close the dialog
        /// </summary>
        /// <param name="minutes">N, number of minutes to set</param>
        void SetInterval(int minutes) {
            _minutes = minutes;
            _dialog.Close(TaskDialogResult.CustomButtonClicked);
        }

        /// <summary>
        /// Show the dialog to select custom time interval 
        /// </summary>
        private void SelectInterval() {
            _minutes = -1;
            _dialog.Close(TaskDialogResult.CustomButtonClicked);
        }

        /// <summary>
        /// Shows a dialog for configuring additional options 
        /// </summary>
        private void ConfigureOptions() {
            using (FrmOptions frm = new FrmOptions(_options ?? TimerOptions.Default)) {
                if (frm.ShowDialog() == DialogResult.OK) {
                    _options = frm.TimerOptions;
                }
            }
        }

        /// <summary>
        /// Shows the dialog 
        /// </summary>
        /// <returns>The result of the dialog</returns>
        public TaskDialogResult Show() {
            return _dialog.Show();
        }

        /// <summary>
        /// Disposes the _dialog 
        /// </summary>
        public void Dispose() {
            _dialog.Dispose();
        }
    }
}
