using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskbarTimer {
    /// <summary>
    /// A form for setting additional timer options 
    /// </summary>
    public partial class FrmOptions : Form {
        /// <summary>
        /// Additional options for the timer 
        /// </summary>
        private TimerOptions _options;

        /// <summary>
        /// Gets the additional options for the timer 
        /// </summary>
        public TimerOptions TimerOptions  {
            get { return _options; }
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="options">Existing additional options</param>
        public FrmOptions(TimerOptions options) {
            InitializeComponent();
            Icon = IconContainer.AppIcon;
            txtTimerText.Text = options.TimerName;
            checkDisableSound.Checked = options.DisableSound;
        }

        /// <summary>
        /// Closes the form and reads the addidional options 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e) {
            string timerName = txtTimerText.Text.Trim();
            if (timerName == string.Empty) {
                timerName = "Taskbar Timer";
            }
            _options = new TimerOptions(timerName, checkDisableSound.Checked);
        }

        /// <summary>
        /// Sets the focus on timer text element 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOptions_Load(object sender, EventArgs e) {
            txtTimerText.SelectAll();
            txtTimerText.Focus();
        }
    }
}
