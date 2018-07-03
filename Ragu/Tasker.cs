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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace org.herbal3d.ragu {
    // Static class to keep track of all the running tasks
    public class Tasker {

        private static Dictionary<Taskette, Task> runningTasks;

        static Tasker() {
            runningTasks = new Dictionary<Taskette, Task>();
        }

        public static Taskette NewTask(Taskette pTask) {
            lock (runningTasks) {
                var running = Task.Run(pTask.Run());
                runningTasks.Add(pTask, running);
            }
            return pTask;
        }

        // Wait for all tasks to complete.
        // NOTE: concurrency problems since we don't lock 'runningTasks'
        public static async void WaitForAllCompletion() {
            await Task.WhenAll(runningTasks.Values);
        }
    }

    // A wrapper for tasks that are managed
    public abstract class Taskette {
        public abstract Action Run();
        public string TaskName = "unspecified";
    }
}
