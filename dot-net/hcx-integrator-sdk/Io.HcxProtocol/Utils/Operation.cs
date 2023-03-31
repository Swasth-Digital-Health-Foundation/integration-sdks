using System;
using System.Reflection;

namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The Operations of HCX Gateway to handle claims processing.
    /// </summary>
    public static class Operation
    {
        public static string getOperation(this Operations p)
        {
            return GetAttr(p).operation;
        }
        public static string getFhirResourceType(this Operations p)
        {
            return GetAttr(p).fhirResourceType;
        }
        private static OperationsAttr GetAttr(Operations p)
        {
            return (OperationsAttr)Attribute.GetCustomAttribute(ForValue(p), typeof(OperationsAttr));
        }
        private static MemberInfo ForValue(Operations p)
        {
            return typeof(Operations).GetField(Enum.GetName(typeof(Operations), p));
        }
    }
}
