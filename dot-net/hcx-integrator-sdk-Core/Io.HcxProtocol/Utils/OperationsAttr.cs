using System;

namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol.Core
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The OperationsAttr class which wrap the operation details and helps in creating an instance of it.
    /// </summary>
    class OperationsAttr : Attribute
    {
        internal OperationsAttr(string _operation, string _fhirResourceType)
        {
            this.operation = _operation;
            this.fhirResourceType = _fhirResourceType;
        }

        public string operation { get; private set; }
        public string fhirResourceType { get; private set; }
    }
}
