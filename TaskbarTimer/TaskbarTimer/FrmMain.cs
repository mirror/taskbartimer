using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Ext;
using Microsoft.WindowsAPICodePack.Taskbar;
using TaskbarTimer.Properties;

namespace TaskbarTimer {
    /// <summary>
    /// Main form of the application 
    /// </summary>
    public partial class FrmMain : GlassFormWithCustomThumbnails {
        /// <summary>
        /// A button in the taskbar to stop the timer 
        /// </summary>
        private readonly ThumbnailToolbarButton _buttonPause = new ThumbnailToolbarButton(Resources.Pause, Resources.PauseTimerTooltip);

        /// <summary>
        /// A button in the taskbar show About dialog 
        /// </summary>
        private readonly ThumbnailToolbarButton _buttonAbout = new ThumbnailToolbarButton(Resources.About, Resources.AboutTooltip) { DismissOnClick = true };
        
        /// <summary>
        /// A font to print the text on an image 
        /// </summary>
        private readonly Font _textFont = new Font("Segoe UI", 72);

        /// <summary>
        /// A font to print the text on a form 
        /// </summary>
        private readonly Font _imageFont = new Font("Segoe UI", 108);

        /// <summary>
        /// A font to print small text on a form 
        /// </summary>
        private readonly Font _imageSmallFont = new Font("Segoe UI", 24);

        /// <summary>
        /// Parameters of string formatting to print the time 
        /// </summary>
        private readonly StringFormat _stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        /// <summary>
        /// Time left before the timer expires 
        /// </summary>
        private TimeSpan _timeLeft;

        /// <summary>
        /// Total minutes (the interval of the timer) 
        /// </summary>
        private static int _totalMinutes;

        /// <summary>
        /// Total interval expressed in milliseconds 
        /// </summary>
        private readonly long _totalMilliseconds = 10000;

        /// <summary>
        /// Milliseconds in one minute 
        /// </summary>
        private const long MILLIS_IN_MINUTE = 1000 * 60;

        /// <summary>
        /// Additional options of the timer 
        /// </summary>
        private readonly TimerOptions _options;

        /// <summary>
        /// Constructor. Initializes a new instance of the main form 
        /// </summary>
        /// <param name="minutes">Number of minutes after which the timer elapses</param>
        /// <param name="elapsedMinutes">Number of minutes already elapsed</param>
        /// <param name="options">Additional options for the timer</param>
        public FrmMain(int minutes, int elapsedMinutes, TimerOptions options) {
            _totalMinutes = minutes;
            _options = options ?? TimerOptions.Default;

            _totalMilliseconds = minutes * MILLIS_IN_MINUTE;
            _timeLeft = new TimeSpan(0, minutes - elapsedMinutes, 0);

            InitializeComponent();

            Icon = IconContainer.AppIcon;
            GetPeekBitmap += OnGetPeekBitmap;
            GetIconicThumbnail += OnGetIconicThumbnail;
        }

        /// <summary>
        /// Handle form load event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e) {
            Text = _options.TimerName;
            _buttonPause.Click += PauseTimer_Clicked;
            _buttonAbout.Click += About_Clicked;
            ThumbnailToolbarButton[] buttons = new[] { _buttonPause, _buttonAbout };
            TaskbarManager.Instance.ThumbnailToolbars.AddButtons(Handle, buttons.ToArray());
        }

        /// <summary>
        /// After the form is shown, we can manipulate JumpList 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Shown(object sender, EventArgs e) {
            new Thread(ConfigureJumpList).Start();
        }

        /// <summary>
        /// Configures the jump-list
        /// </summary>
        private void ConfigureJumpList() {
            JumpListHelper jumpListHelper = new JumpListHelper();
            jumpListHelper.ConfigureJumpList();
            if (_totalMinutes != 5 && _totalMinutes != 10 && _totalMinutes != 30) {
                jumpListHelper.AddCustomLink(_totalMinutes, _options.TimerName);
            }
            jumpListHelper.Save();
        }

        /// <summary>
        /// Pauses the timer 
        /// </summary>
        private void PauseTimer_Clicked(object sender, ThumbnailButtonClickedEventArgs e) {
            if (timer.Enabled) {
                timer.Stop();
                e.ThumbnailButton.Icon = Resources.Play;
                e.ThumbnailButton.Tooltip = Resources.StartTimerTooltip;
            } else {
                timer.Start();
                e.ThumbnailButton.Icon = Resources.Pause;
                e.ThumbnailButton.Tooltip = Resources.PauseTimerTooltip;
            }
        }

        /// <summary>
        /// Shows the About message-box 
        /// </summary>
        private static void About_Clicked(object sender, ThumbnailButtonClickedEventArgs e) {
            MessageBox.Show(Resources.AboutText, Resources.AboutTooltip, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Returns a bitmap that will be shown to the user as peek bitmap 
        /// </summary>
        /// <returns>Peek bitmap</returns>
        protected Bitmap OnGetPeekBitmap() {
            Bitmap bmp = Resources.Splash;
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.DrawString(GetTimeLeftText(), _imageFont, Brushes.White, new RectangleF(new PointF(0, 0), bmp.Size), _stringFormat);
                g.DrawString(Text, _imageSmallFont, Brushes.White, new RectangleF(0, 0, bmp.Width, bmp.Height * 0.35f), _stringFormat);
            }
            return bmp;
        }

        /// <summary>
        /// Returns a small thubmnail that will be shown in the taskbar 
        /// </summary>
        /// <param name="size">Requested size of the thumbnail</param>
        /// <returns>Iconic thumbnail</returns>
        protected Bitmap OnGetIconicThumbnail(Size size) {
            Bitmap bmp = Resources.Splash;
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.DrawString(GetTimeLeftText(), _imageFont, Brushes.White, new RectangleF(new PointF(0, 0), bmp.Size), _stringFormat);
            }
            return new Bitmap(bmp, size);
        }

        /// <summary>
        /// Pains the timer on the form 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.DrawString(GetTimeLeftText(), _textFont, Brushes.Black, ClientRectangle, _stringFormat);
        }

        /// <summary>
        /// Gets the time left as text 
        /// </summary>
        /// <returns>Text representation of the time left</returns>
        private string GetTimeLeftText() {
            if (_timeLeft.TotalMilliseconds <= 0) {
                return "--:--";
            }
            string timeStr = string.Format("{0}{1:00}:{2:00}", _timeLeft.Hours > 0 ? _timeLeft.Hours + ":" : "", _timeLeft.Minutes, _timeLeft.Seconds);
            return timeStr;
        }

        /// <summary>
        /// Handles timer tick event. Invalidates form image and thumbnails 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e) {
            _timeLeft = _timeLeft.Subtract(new TimeSpan(0, 0, 0, 1));

            Invalidate();
            InvalidateThumbnails();

            // Calculate the percent of completion 
            int percent = _timeLeft.TotalMilliseconds > 0 ? (100 - (int)(_timeLeft.TotalMilliseconds / _totalMilliseconds * 100d)) : 100;
            if (percent < 0 || percent > 100) {
                percent = 0;
            }
            TaskbarManager.Instance.SetProgressValue(percent, 100);

            if (_timeLeft.TotalMilliseconds <= 0) {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                timer.Stop();
                _buttonPause.Enabled = false;
                NativeMethods.FlashWindow(Handle, true);
                if (!_options.DisableSound) {
                    NativeMethods.PlaySound(Path.Combine(KnownFolders.Windows.Path, "Media\\Windows Default.wav"), IntPtr.Zero, PlaySoundFlags.SND_FILENAME | PlaySoundFlags.SND_ASYNC);
                }
            }
        }

        /// <summary>
        /// Handle FormClosing event. If the timer has not yet elapsed, we'll display the confirmation message to prevent accidental close 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            if (timer.Enabled) {
                if (MessageBox.Show("Exit and discard current timer?", "Taskbar Timer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

    }
}
