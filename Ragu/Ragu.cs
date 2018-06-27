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
//
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;

namespace org.herbal3d.ragu {

    // Class passed around for global context for this region module instance.
    // NOT FOR PASSING DATA! Only used for global resources like logging, configuration
    //    parameters, statistics, and the such.
    public class GlobalContext {
        public RaguParams parms;
        public RaguStats stats;
        public Logger log;
        public string version;
        public string buildDate;
        public string gitCommit;

        public GlobalContext() {
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            // A command is added to the pre-build events that generates BuildDate resource:
            //        echo %date% %time% > "$(ProjectDir)\Resources\BuildDate.txt"
            buildDate = Properties.Resources.BuildDate.Trim();
            // A command is added to the pre-build events that generates last commit resource:
            //        git rev-parse HEAD > "$(ProjectDir)\Resources\GitCommit.txt"
            gitCommit = Properties.Resources.GitCommit.Trim();
        }
    }

    class Ragu {
        public static GlobalContext Globals;

        private string Invocation() {
            StringBuilder buff = new StringBuilder();
            buff.AppendLine("Invocation: ragu <parameters>");
            buff.AppendLine("   Possible parameters are (negate bool parameters by prepending 'no'):");
            string[] paramDescs = Globals.parms.ParameterDefinitions.Select(pp => { return pp.ToString(); }).ToArray();
            buff.AppendLine(String.Join(Environment.NewLine, paramDescs));
            return buff.ToString();
        }

        static void Main(string[] args) {
            Ragu prog = new Ragu();
            prog.Start(args);
            return;
        }

        // If run from the command line, create instance and call 'Start' with args.
        // If run programmatically, create instance and call 'Start' with parameters.
        public void Start(string[] args) {
            Globals = new GlobalContext() {
                parms = new RaguParams(),
                log = new LoggerMetaverse(),
                // log = new LoggerConsole(),
                // log = new LoggerLog4Net(),   // must use log4net because of libOMV
                stats = new RaguStats()
            };

            // A single parameter of '--help' outputs the invocation parameters
            if (args.Length > 0 && args[0] == "--help") {
                System.Console.Write(Invocation());
                return;
            }

            // 'ConvoarParams' initializes to default values.
            // Over ride default values with command line parameters.
            try {
                // Note that trailing parameters will be put into "InputOAR" parameter
                Globals.parms.MergeCommandLine(args, null, null);
            }
            catch (Exception e) {
                Globals.log.ErrorFormat("ERROR: bad parameters: " + e.Message);
                Globals.log.ErrorFormat(Invocation());
                return;
            }

            if (Globals.parms.P<bool>("Verbose")) {
                Globals.log.SetVerbose(Globals.parms.P<bool>("Verbose"));
            }

            if (!Globals.parms.P<bool>("Quiet")) {
                System.Console.WriteLine("Ragu v" + Globals.version
                            + " built " + Globals.buildDate
                            + " commit " + Globals.gitCommit
                            );
            }
            else {
                Globals.log.SetQuiet(true);
            }

            // Verify parameters

            // Make connection
        }

    }
}
