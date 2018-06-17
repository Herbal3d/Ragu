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
using System.Text;
using System.Collections.Generic;

using OMV = OpenMetaverse;
using OMVR = OpenMetaverse.Rendering;

namespace org.herbal3d.ragu {
    public class RaguStats : Monitored {

    #pragma warning disable 414
        private static readonly string _logHeader = "[Stats]";
    #pragma warning restore 414

        public RaguStats() : base() {
        }

    }
}
