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

using org.herbal3d.basil.protocol.BasilType;
using org.herbal3d.basil.protocol.BasilServer;

using RSG;

namespace org.herbal3d.ragu {
    public class BasilClient {

        public BasilClient(BTransport xport) {
        }

        public Promise<IdentifyDisplayableObjectResp> IdentifyDisplayObject(AccessAuthorization auth, AssetInformation asset, AaBoundingBox aabb) {
            return new Promise<IdentifyDisplayableObjectResp>((resolve,reject) => {
                reject(new Exception("BasilClient: IdentifyDisplayableObject not implimented"));
            });
        }
        public Promise<ForgetDisplayableObjectResp> ForgetDisplayObject(AccessAuthorization auth, ObjectIdentifier id) {
            return new Promise<ForgetDisplayableObjectResp>((resolve, reject) => {
                reject(new Exception("BasilClient: ForgetDisplayObject not implimented"));
            });
        }
        public Promise<CreateObjectInstanceResp> CreateObjectInstance(AccessAuthorization auth, ObjectIdentifier id, InstancePositionInfo posInfo, Dictionary<string,string> propList) {
            return new Promise<CreateObjectInstanceResp>((resolve, reject) => {
                reject(new Exception("BasilClient: CreateObjectInstance not implimented"));
            });
        }
        public Promise<DeleteObjectInstanceResp> DeleteObjectInstance(AccessAuthorization auth, InstanceIdentifier id) {
            return new Promise<DeleteObjectInstanceResp>((resolve, reject) => {
                reject(new Exception("BasilClient: DeleteObjectInstance not implimented"));
            });
        }
        public Promise<UpdateObjectPropertyResp> UpdateObjectProperty(AccessAuthorization auth, ObjectIdentifier id, Dictionary<string,string> propList) {
            return new Promise<UpdateObjectPropertyResp>((resolve, reject) => {
                reject(new Exception("BasilClient: UpdateObjectProperty not implimented"));
            });
        }
        public Promise<UpdateInstancePropertyResp> UpdateInstanceProperty(AccessAuthorization auth, InstanceIdentifier id, Dictionary<string,string> propList) {
            return new Promise<UpdateInstancePropertyResp>((resolve, reject) => {
                reject(new Exception("BasilClient: UpdateInstanceProperty not implimented"));
            });
        }
        public Promise<UpdateInstancePositionResp> UpdateInstancePosition(AccessAuthorization auth, InstanceIdentifier id, InstancePositionInfo posInfo) {
            return new Promise<UpdateInstancePositionResp>((resolve, reject) => {
                reject(new Exception("BasilClient: UpdateInstancePosition not implimented"));
            });
        }
        public Promise<RequestInstancePropertiesResp> RequestObjectProperties(AccessAuthorization auth, ObjectIdentifier id, string filter) {
            return new Promise<RequestInstancePropertiesResp>((resolve, reject) => {
                reject(new Exception("BasilClient: RequestObjectProperties not implimented"));
            });
        }
        public Promise<RequestInstancePropertiesResp> RequestInstanceProperties(AccessAuthorization auth, InstanceIdentifier id, string filter) {
            return new Promise<RequestInstancePropertiesResp>((resolve, reject) => {
                reject(new Exception("BasilClient: RequestInstanceProperties not implimented"));
            });
        }
        public Promise<OpenSessionResp> OpenSession(AccessAuthorization auth, Dictionary<string,string> propList) {
            return new Promise<OpenSessionResp>((resolve, reject) => {
                reject(new Exception("BasilClient: OpenSession not implimented"));
            });
        }
        public Promise<CloseSessionResp> CloseSession(AccessAuthorization auth, Dictionary<string,string> propList) {
            return new Promise<CloseSessionResp>((resolve, reject) => {
                reject(new Exception("BasilClient: CloseSession not implimented"));
            });
        }
    }
}
