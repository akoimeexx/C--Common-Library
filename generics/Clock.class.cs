/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Generics.Clock: basic class to provide realtime clock binding, v.0.0.1
 *    Johnathan Graham McKnight <akoimeexx@gmail.com>
 *
 *
 * Copyright (c) 2016, Johnathan Graham McKnight
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors
 * may be used to endorse or promote products derived from this software without
 * specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */
namespace Common.Generics {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Threading;

    public partial class Clock : Notifiable {
#region Properties
        public DateTime Time {
            get { return _time; }
            set { Set(ref _time, value); }
        } private DateTime _time = DateTime.Now;

        public ObservableCollection<DateTime> Alarms {
            get { return _alarms; }
            set { Set(ref _alarms, value); }
        } private ObservableCollection<DateTime> _alarms = 
        new ObservableCollection<DateTime>();

        private DispatcherTimer _timer = new DispatcherTimer() {
            Interval = new TimeSpan(0, 0, 1)
        };
#endregion Properties
    }
    public partial class Clock {
#region Components
        public class AlarmEventArgs : EventArgs {
            private List<DateTime> _alarmsTriggered;
            public List<DateTime> AlarmsTriggered => _alarmsTriggered;
            public AlarmEventArgs(List<DateTime> alarmsTriggered) {
                _alarmsTriggered = alarmsTriggered;
            }
        }
#endregion Components
    }
    public partial class Clock {
#region Events
        public event EventHandler<AlarmEventArgs> AlarmTriggered;
#endregion Events
    }
    public partial class Clock {
#region Methods
        private void clock_Tick(object o, EventArgs e) {
            Time = Time.AddSeconds(1);

            List<DateTime> alarms = Alarms.Where(alarm => 
                alarm.TimeOfDay.ToString("HH:mm:ss") == 
                Time.TimeOfDay.ToString("HH:mm:ss")
            ).ToList();
            if (alarms.Count > 0) AlarmTriggered(
                this, new AlarmEventArgs(alarms)
            );
        }

        public override string ToString() { return Time.ToString(); }
        public string ToString(string format) { return Time.ToString(format); }
        public string ToString(IFormatProvider provider) {
            return Time.ToString(provider);
        }
        public string ToString(string format, IFormatProvider provider) {
            return Time.ToString(format, provider);
        }
#endregion Methods
    }
    public partial class Clock {
#region Constructors & Destructor
        public Clock() : this(DateTime.Now) { }
        public Clock(DateTime start) : this(start, new List<DateTime>()) { }
        public Clock(List<DateTime> alarms) : this(DateTime.Now, alarms) { }
        public Clock(DateTime start, List<DateTime> alarms) {
            Time = start;
            Alarms = new ObservableCollection<DateTime>(alarms);
            _timer.Tick += clock_Tick;
            _timer.Start();
        }
        ~Clock() {
            _timer.Stop();
            if (this.AlarmTriggered != null)
            foreach (var eh in AlarmTriggered.GetInvocationList())
                AlarmTriggered -= (EventHandler<AlarmEventArgs>)eh;
        }
#endregion Constructors & Destructor
    }
}
