// Copyright 2018 Robert Adams
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

// using NLog;
using log4net;

namespace org.herbal3d.ragu {

    public abstract class Logger {
        public abstract void SetVerbose(bool val);
        public abstract void SetQuiet(bool val);
        public abstract void Log(string msg, params Object[] args);
        public abstract void DebugFormat(string msg, params Object[] args);
        public abstract void ErrorFormat(string msg, params Object[] args);
    }

    public class LoggerConsole : Logger {
        private bool _verbose = false;
        private bool _quiet = false;
        public override void SetVerbose(bool value) {
            _verbose = value;
        }

        public override void SetQuiet(bool value) {
            _quiet = value;
        }

        public override void Log(string msg, params Object[] args) {
            if (!_quiet) {
                System.Console.WriteLine(msg, args);
            }
        }

        // Output the message if 'Verbose' is true
        public override void DebugFormat(string msg, params Object[] args) {
            if (_verbose && !_quiet) {
                System.Console.WriteLine(msg, args);
            }
        }

        public override void ErrorFormat(string msg, params Object[] args) {
            System.Console.WriteLine(msg, args);
        }
    }

    /*
    // Do logging with NLog
    public class LoggerNLog : Logger {
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private static string _logHeader = "[Logger]";

        private bool _verbose = false;
        private bool _quiet = false;
        public override void SetVerbose(bool value) {
            // https://gist.github.com/pmullins/f21c3d83e96b9fd8a720
            _verbose = value;
            foreach (var rule in NLog.LogManager.Configuration.LoggingRules) {
                if (_verbose) {
                    rule.EnableLoggingForLevel(NLog.LogLevel.Debug);
                }
                else {
                    rule.EnableLoggingForLevel(NLog.LogLevel.Info);
                }
            }
            NLog.LogManager.ReconfigExistingLoggers();
        }

        public override void SetQuiet(bool value) {
            _quiet = value;
            if (_quiet) {
                NLog.LogManager.DisableLogging();
            }
            else {
                NLog.LogManager.EnableLogging();
            }
            NLog.LogManager.ReconfigExistingLoggers();
        }

        public override void Log(string msg, params Object[] args) {
            _log.Info(msg, args);
        }

        // Output the message if 'Verbose' is true
        public override void DebugFormat(string msg, params Object[] args) {
            _log.Debug(msg, args);
        }

        public override void ErrorFormat(string msg, params Object[] args) {
            _log.Error(msg, args);
        }
    }
    */

    // Do logging with log4net
    public class LoggerLog4Net : Logger {
        private static readonly ILog _log = LogManager.GetLogger("convoar");
        private bool _verbose = false;
        private bool _quiet = false;
        private log4net.Core.Level _oldLevel = log4net.Core.Level.Info;

        public override void SetVerbose(bool value) {
            _verbose = value;
            var logHeir = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            if (_verbose) {
                if (logHeir.Root.Level != log4net.Core.Level.Debug) {
                    _oldLevel = logHeir.Root.Level;
                    logHeir.Root.Level = log4net.Core.Level.Debug;
                }
            }
            else {
                logHeir.Root.Level = _oldLevel;
            }
            logHeir.RaiseConfigurationChanged(EventArgs.Empty);
        }

        public override void SetQuiet(bool val) {
            _quiet = val;
            var logHeir = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            if (_quiet) {
                if (logHeir.Root.Level != log4net.Core.Level.Error) {
                    _oldLevel = logHeir.Root.Level;
                    logHeir.Root.Level = log4net.Core.Level.Error;
                }
            }
            else {
                logHeir.Root.Level = _oldLevel;
            }
            logHeir.RaiseConfigurationChanged(EventArgs.Empty);
        }

        public override void Log(string msg, params Object[] args) {
            _log.InfoFormat(msg, args);
        }

        // Output the message if 'Verbose' is true
        public override void DebugFormat(string msg, params Object[] args) {
            _log.DebugFormat(msg, args);
        }

        public override void ErrorFormat(string msg, params Object[] args) {
            _log.ErrorFormat(msg, args);
        }
    }
}
