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
/*
 * Some code covered by: Copyright (c) Contributors, http://opensimulator.org/
 */

using System;
using System.Text;

using OMV = OpenMetaverse;

namespace org.herbal3d.ragu {
    public class RaguParams {
        private static readonly string _logHeader = "[RAGU PARAMS]";

        public RaguParams() {
            SetParameterDefaultValues();
        }

        // =====================================================================================
        // =====================================================================================
        // List of all of the externally visible parameters.
        // For each parameter, this table maps a text name to getter and setters.
        // To add a new externally referencable/settable parameter, add the paramter storage
        //    location somewhere in the program and make an entry in this table with the
        //    getters and setters.
        // It is easiest to find an existing definition and copy it.
        //
        // A ParameterDefn<T>() takes the following parameters:
        //    -- the text name of the parameter. This is used for console input and ini file.
        //          Also used as the name of that parameter when getting and setting.
        //    -- a short text description of the parameter. This shows up in the console listing.
        //    -- a default value
        //    -- optional parameter names (like an "i" as an alternate for "input")
        //
        // The single letter parameters for the delegates are:
        //    v = value (appropriate type)
        public ParameterDefnBase[] ParameterDefinitions =
        {
            new ParameterDefn<string>("==========", "Connection Parameters", null),

            // Debugging and logging
            new ParameterDefn<string>("==========", "Debugging", null),
            new ParameterDefn<bool>("Quiet", "supress as much informational output as possible",
                false, "q" ),
            new ParameterDefn<bool>("Verbose", "enable DEBUG information logging",
                false, "v" ),
        };

        // =====================================================================================
        // =====================================================================================

        // Base parameter definition that gets and sets parameter values via a string
        public abstract class ParameterDefnBase {
            public string name;         // string name of the parameter
            public string desc;         // a short description of what the parameter means
            public abstract Type GetValueType();
            public string[] symbols;    // command line symbols for this parameter (short forms)
            public RaguParams context; // context for setting and getting values
            public ParameterDefnBase(string pName, string pDesc, string[] pSymbols) {
                name = pName;
                desc = pDesc;
                symbols = pSymbols;
            }
            // Set the parameter value to the default
            public abstract void AssignDefault();
            // Get the value as a string
            public abstract string GetValue();
            // Set the value to this string value
            public abstract void SetValue(string valAsString);
        }

        // Specific parameter definition for a parameter of a specific type.
        public sealed class ParameterDefn<T> : ParameterDefnBase {
            public T defaultValue;
            public T value;
            public override Type GetValueType() {
                return typeof(T);
            }
            public ParameterDefn(string pName, string pDesc, T pDefault, params string[] symbols)
                : base(pName, pDesc, symbols) {
                defaultValue = pDefault;
            }
            public T Value() {
                return value;
            }
            public override void AssignDefault() {
                value = defaultValue;
            }
            public override string GetValue() {
                string ret = String.Empty;
                if (value != null) {
                    ret = value.ToString();
                }
                return ret;
            }
            public override void SetValue(String valAsString) {
                // Find the 'Parse' method on that type
                System.Reflection.MethodInfo parser = null;
                try {
                    parser = GetValueType().GetMethod("Parse", new Type[] { typeof(String) } );
                }
                catch {
                    parser = null;
                }
                if (parser != null) {
                    // Parse the input string
                    try {
                        T setValue = (T)parser.Invoke(GetValueType(), new Object[] { valAsString });
                        // System.Console.WriteLine("SetValue: setting value on {0} to {1}", this.name, setValue);
                        // Store the parsed value
                        value = setValue;
                        Ragu.Globals.log.DebugFormat("{0} SetValue. {1} = {2}", _logHeader, name, setValue);
                    }
                    catch (Exception e) {
                        Ragu.Globals.log.ErrorFormat("{0} Failed parsing parameter value '{1}': '{2}'", _logHeader, valAsString, e);
                    }
                }
                else {
                    // If there is not a parser, try doing a conversion
                    try {
                        T setValue = (T)Convert.ChangeType(valAsString, GetValueType());
                        value = setValue;
                        Ragu.Globals.log.DebugFormat("{0} SetValue. Converter. {1} = {2}", _logHeader, name, setValue);
                    }
                    catch (Exception e) {
                        Ragu.Globals.log.ErrorFormat("{0} Conversion failed for {1}: {2}", _logHeader, this.name, e);
                    }
                }
            }
            // Create a description for this parameter that can be used in a list of parameters.
            // For better listings, there is a special 'separator' parameter that is just for the description.
            //      These separator parameters start with an equal sign ('=').
            const int leader = 20;
            public override string ToString() {
                StringBuilder buff = new StringBuilder();
                bool hasValue = true;
                // Start with the parameter name. If multiple, first is "--" type and later are "-" type.
                if (symbols.Length > 0) {
                    buff.Append("[ ");
                    buff.Append("--");
                    buff.Append(name);
                    foreach (string sym in symbols) {
                        buff.Append(" | ");
                        buff.Append("-");
                        buff.Append(sym);
                    }
                    buff.Append(" ]: ");
                }
                else {
                    if (name.StartsWith("=")) {
                        hasValue = false;
                        buff.Append(name.Substring(1));
                    }
                    else {
                        buff.Append("--");
                        buff.Append(name);
                        buff.Append(": ");
                    }
                }
                // Provide tab like padding between the name and the description
                if (buff.Length < leader) {
                    buff.Append("                                        ".Substring(0, leader - buff.Length));
                }
                buff.Append(desc);
                // Add the type and the default value of the parameter
                if (hasValue) {
                    buff.Append(" (");
                    buff.Append("Type=");
                    switch (GetValueType().ToString()) {
                        case "System.Boolean": buff.Append("bool"); break;
                        case "System.Int32": buff.Append("int"); break;
                        case "System.Float": buff.Append("float"); break;
                        case "System.Double": buff.Append("double"); break;
                        case "System.String": buff.Append("string"); break;
                        case "OpenMetaverse.Vector3": buff.Append("vector3"); break;
                        case "OpenMetaverse.Quaterion": buff.Append("quaterion"); break;
                        default: buff.Append(GetValueType().ToString()); break;
                    }
                    buff.Append(",Default=");
                    buff.Append(GetValue());
                    buff.Append(")");
                }

                return buff.ToString();
            }
        }

        // Search through the parameter definitions and return the matching
        //    ParameterDefn structure.
        // Case does not matter as names are compared after converting to lower case.
        // Returns 'false' if the parameter is not found.
        public bool TryGetParameter(string paramName, out ParameterDefnBase defn) {
            bool ret = false;
            ParameterDefnBase foundDefn = null;
            string pName = paramName.ToLower();

            foreach (ParameterDefnBase parm in ParameterDefinitions) {
                string parmL = parm.name.ToLower();
                if (pName == parmL) {
                    foundDefn = parm;
                    ret = true;
                }
                if (ret == false && parm.symbols != null) {
                    foreach (string sym in parm.symbols) {
                        if (sym == pName) {
                            foundDefn = parm;
                            ret = true;
                            break;
                        }
                    }
                }
                if (ret) break;
            }
            defn = foundDefn;
            return ret;
        }

        // Return a value for the parameter.
        // This is used by most callers to get parameter values.
        // Note that it outputs a console message if not found. Not found means that the caller
        //     used the wrong string name.
        public T P<T>(string paramName) {
            T ret = default(T);
            if (TryGetParameter(paramName, out ParameterDefnBase pbase)) {
                if (pbase is ParameterDefn<T> pdef) {
                    ret = pdef.Value();
                }
                else {
                    Ragu.Globals.log.ErrorFormat("{0} Fetched unknown parameter. Param={1}", _logHeader, paramName);
                }
            }
            return ret;
        }

        // Find the named parameter and set its value.
        // Returns 'false' if the parameter could not be found.
        public bool SetParameterValue(string paramName, string valueAsString) {
            bool ret = false;
            if (TryGetParameter(paramName, out ParameterDefnBase parm)) {
                parm.SetValue(valueAsString);
                ret = true;
            }
            return ret;
        }

        // Pass through the settable parameters and set the default values.
        public void SetParameterDefaultValues() {
            foreach (ParameterDefnBase parm in ParameterDefinitions) {
                parm.context = this;
                parm.AssignDefault();
            }
        }

        public void MergeCommandLine(string[] args) {
            MergeCommandLine(args, null, null);
        }

        // Given parameters from the command line, read the parameters and set values specified
        // <param name="args">array of command line tokens</param>
        // <param name="firstOpFlag">if 'true' presume the first token in the parameter line
        // is a special value that should be assigned to the keyword "--firstparam".</param>
        // <param name="multipleLastParameters">if 'true' presume multiple specs at the end of the line
        // are filenames and pack them together into a CSV string in LAST_PARAM.</param>
        public bool MergeCommandLine(string[] args, string firstOpParameter, string multipleLastParameters) {
            bool ret = true;    // start out assuming parsing worked

            bool firstOpFlag = false;   // no first op
            if (!String.IsNullOrEmpty(firstOpParameter)) {
                firstOpFlag = true;
            }
            bool multipleLast = false;
            if (!String.IsNullOrEmpty(multipleLastParameters)) {
                multipleLast = true;
            }

            for (int ii = 0; ii < args.Length; ii++) {
                string para = args[ii];
                // is this a parameter?
                if (para[0] == '-') {
                    ii += AddCommandLineParameter(para, (ii==(args.Length-1)) ? null : args[ii + 1]);
                }
                else {
                    if (ii == 0 && firstOpFlag) {
                        // if the first thing is not a parameter, make like it's an op or something
                        ii += AddCommandLineParameter(firstOpParameter, args[ii + 1]);
                    }
                    else {
                        if (multipleLast) {
                            // Pack all remaining arguments into a comma-separated list as LAST_PARAM
                            StringBuilder multFiles = new StringBuilder();
                            for (int jj = ii; jj < args.Length; jj++) {
                                if (multFiles.Length != 0) {
                                    multFiles.Append(",");
                                }
                                multFiles.Append(args[jj]);
                            }
                            AddCommandLineParameter(multipleLastParameters, multFiles.ToString());

                            // Skip them all
                            ii = args.Length;
                        }
                        else {
                            throw new ArgumentException("Unknown parameter " + para);
                        }
                    }
                }
            }

            return ret;
        }

        // Store the value for the parameter.
        // If we accept the value as a good value for the parameter, return 1 else 0.
        // A 'good value' is one that does not start with '-' or is not after a boolean parameter.
        // Return the number of parameters to advance the parameter line. That means, return
        //    a zero of we didn't used the next parameter and a 1 if the next parameter
        //    was used as a value so don't consider it the next parameter.
        private int AddCommandLineParameter(string pParm, string val) {
            // System.Console.WriteLine(String.Format("AddCommandLineParameter: parm={0}, val={1}", pParm, val));
            int ret = 1;    // start off assuming the next token is the value we're setting
            string parm = pParm.ToLower();
            // Strip leading hyphens
            while (parm[0] == '-') {
                parm = parm.Substring(1);
            }

            // If the boolean parameter starts with "no", turn it off rather than on.
            string positiveAssertion = "true";
            if (parm.Length > 2 && parm[0] == 'n' && parm[1] == 'o') {
                string maybeParm = parm.Substring(2);
                if (TryGetParameter(parm, out ParameterDefnBase parmDefnX)) {
                    if (parmDefnX.GetValueType() == typeof(Boolean)) {
                        // The parameter without the 'no' exists and is a boolean
                        positiveAssertion = "false";
                        parm = maybeParm;
                    }
                }
            }

            // If the next token starts with a parameter mark, it's not really a value
            if (val == null) {
                ret = 0;    // the next token is not used here to set the value
            }
            else {
                if (val[0] == '-') {
                    val = null; // don't use the next token as a value
                    ret = 0;    // the next token is not used here to set the value
                }
            }

            if (TryGetParameter(parm, out ParameterDefnBase parmDefn)) {
                // If the parameter is a boolean type and the next value is not a parameter,
                //      don't try to take up the next value.
                // This handles boolean flags.
                // If there is a value next (val != null) and that value is not the
                //    values 'true' or 'false' or 't' or 'f', then ignore the next value
                //    as not belonging to this flag. THis allows (and the logic above)
                //    allows:
                //        "--flag --otherFlag ...",
                //        "--flag something ...",
                //        "--flag true --otherFlag ...",
                //        "--noflag --otherflag ...",
                //        etc
                if (parmDefn.GetValueType() == typeof(Boolean)) {
                    if (val != null) {
                        string valL = val.ToLower();
                        if (valL != "true" && valL != "t" && valL != "false" && valL != "f") {
                            // The value is not associated with this boolean so ignore it
                            val = null; // don't use the val token
                            ret = 0;    // the next token is not used here to set the value
                        }
                    }
                    if (val == null) {
                        // If the value is assumed, use the value based on the optional 'no'
                        val = positiveAssertion;
                    }
                }
                // Set the named parameter to the passed value
                parmDefn.SetValue(val);
            }
            else {
                throw new ArgumentException("Unknown parameter " + parm);
            }
            return ret;
        }
    }
}
