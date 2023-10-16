namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// Enum class for the Operations of HCX Gateway to handle claims processing.
    /// </summary>
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
        [OperationsAttr("/communication/on_request", "Bundle")] COMMUNICATION_ON_REQUEST,
        [OperationsAttr("/predetermination/submit", "Bundle")] PREDETERMINATION_SUBMIT,
        [OperationsAttr("/predetermination/on_submit", "Bundle")] PREDETERMINATION_ON_SUBMIT,
        [OperationsAttr("/notification/topic/list", "")] NOTIFICATION_LIST,
        [OperationsAttr("/notification/notify", "")] NOTIFICATION_NOTIFY
    }
}
