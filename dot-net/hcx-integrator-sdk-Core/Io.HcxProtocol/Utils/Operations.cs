using System;
using System.Reflection;

namespace Io.HcxProtocol.Utils
{
    /// <summary>
    /// The Operations of HCX Gateway to handle claims processing.
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

    public enum Operations
    {
        [OperationsAttr("/coverageeligibility/check", "Bundle")] COVERAGE_ELIGIBILITY_CHECK,
        [OperationsAttr("/coverageeligibility/on_check", "Bundle")] COVERAGE_ELIGIBILITY_ON_CHECK,
        [OperationsAttr("/preauth/submit", "Bundle")] PRE_AUTH_SUBMIT,
        [OperationsAttr("/preauth/on_submit", "Bundle")] PRE_AUTH_ON_SUBMIT,
        [OperationsAttr("/claim/submit", "Bundle")] CLAIM_SUBMIT,
        [OperationsAttr("/claim/on_submit", "Bundle")] CLAIM_ON_SUBMIT,
        [OperationsAttr("/paymentnotice/request", "PaymentNotice")] PAYMENT_NOTICE_REQUEST,
        [OperationsAttr("/paymentnotice/on_request", "PaymentReconciliation")] PAYMENT_NOTICE_ON_REQUEST,
        [OperationsAttr("/hcx/status", "StatusRequest")] HCX_STATUS,
        [OperationsAttr("/hcx/on_status", "StatusResponse")] HCX_ON_STATUS,
        [OperationsAttr("/communication/request", "CommunicationRequest")] COMMUNICATION_REQUEST,
        [OperationsAttr("/communication/on_request", "Communication")] COMMUNICATION_ON_REQUEST,
        [OperationsAttr("/predetermination/submit", "Bundle")] PREDETERMINATION_SUBMIT,
        [OperationsAttr("/predetermination/on_submit", "Bundle")] PREDETERMINATION_ON_SUBMIT
    }
}
