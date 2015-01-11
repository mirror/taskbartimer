using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskbarTimer {
    /// <summary>
    /// Represents additional options for timer 
    /// </summary>
    public class TimerOptions {
        /// <summary>
        /// Name of the timer 
        /// </summary>
        private string _timerName;

        /// <summary>
        /// Whether we need to disable the sound 
        /// </summary>
        private readonly bool _disableSound;

        /// <summary>
        /// Default timer options 
        /// </summary>
        public static TimerOptions Default {
            get { return new TimerOptions("Taskbar Timer", false); }
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="timerName">Custom window text</param>
        /// <param name="disableSound">Whether we need to disable sound when the timer is elapsed</param>
        public TimerOptions(string timerName, bool disableSound) {
            _timerName = timerName;
            _disableSound = disableSound;
        }

        /// <summary>
        /// Custom window text 
        /// </summary>
        public string TimerName {
            get { return _timerName; }
            set { _timerName = value; }
        }

        /// <summary>
        /// Whether we need to disable sound when the timer is elapsed 
        /// </summary>
        public bool DisableSound {
            get { return _disableSound; }
        }
    }
}
