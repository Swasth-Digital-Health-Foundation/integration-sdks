using Io.HcxProtocol.Utils;
using System.Collections.Generic;

namespace Io.HcxProtocol.Interfaces
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// This is base class to extract the protocol headers from the jwe/json payload and Validate Headers Data.
    /// </summary>
    public interface IBaseRequest
    {
        Dictionary<string, object> ProtocolHeaders { get; set; }
        Dictionary<string, object> GetPayload();
        string GetWorkflowId();
        string GetApiCallId();
        string GetCorrelationId();
        string GetHcxSenderCode();
        string GetTimestamp();
        string GetDebugFlag();
        string GetStatus();
        Dictionary<string, object> GetErrorDetails();
        Dictionary<string, object> GetDebugDetails();
        string GetRedirectTo();
        string[] GetPayloadValues();
        bool ValidateHeadersData(List<string> mandatoryHeaders, Operations operation, Dictionary<string, object> error);
        bool ValidateJwePayload(Dictionary<string, object> error, string[] payloadArr);
        bool ValidateCondition(bool condition, Dictionary<string, object> error, string key, string msg);
        bool ValidateDetails(Dictionary<string, object> inputObj, Dictionary<string, object> error, string errorKey, string msg, List<string> rangeValues, string rangeMsg);
        bool ValidateOptionalHeaders(Dictionary<string, object> error);
        bool ValidateOnAction(Operations operation, Dictionary<string, object> error);
        bool ValidateValues(string inputStr, Dictionary<string, object> error, string key, string msg, List<string> statusValues, string rangeMsg);
    }
}
