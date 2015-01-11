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
    /// A form for setting custom timer interval 
    /// </summary>
    public partial class FrmOtherTime : Form {
        /// <summary>
        /// Selected value 
        /// </summary>
        private int _minutes;

        /// <summary>
        /// Returns the selected value 
        /// </summary>
        public int Minutes {
            get { return _minutes; }
        }

        /// <summary>
        /// Selects the time element and sets the focus on it 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOtherTime_Load(object sender, EventArgs e) {
            numValue.Focus();
            numValue.Select(0, 1);
            lblCaption.Text += " Maximum: " + numValue.Maximum;
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public FrmOtherTime() {
            InitializeComponent();
            Icon = IconContainer.AppIcon;
        }

        /// <summary>
        /// REads the state of elements selected by the user 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e) {
            _minutes = (int)numValue.Value;

            if (_minutes < numValue.Minimum || _minutes > numValue.Maximum) {
                DialogResult = DialogResult.None;
            }
        }
    }
}
