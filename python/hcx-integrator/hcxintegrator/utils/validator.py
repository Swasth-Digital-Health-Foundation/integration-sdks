from hcxintegrator.utils.constants import Constants
from jwcrypto import jwk, jwe
import base64
import logging

def validateJWERequest(operation, requestBody):
    payload = requestBody[Constants.PAYLOAD]
    payloadArr = payload.split(".")
    if validateJWEPayload(payloadArr):
        return False
    # validate the headers
    mandetoryHeaders = [Constants.ALG, Constants.ENC,
                        Constants.HCX_SENDER_CODE, Constants.HCX_RECIPIENT_CODE,
                        Constants.HCX_API_CALL_ID, Constants.HCX_TIMESTAMP,
                        Constants.HCX_CORRELATION_ID]
    validateHeadersData(payloadArr, mandetoryHeaders, operation)
    return True


def validateJWEPayload(payloadArr:str):
    if payloadArr is not None and len(payloadArr) is not Constants.PROTOCOL_PAYLOAD_LENGTH:
        return True
    
    if payloadArr is not None:
        for values in payloadArr:
            if not values or values is None:
                # TODO: raise errors
                return True
    return False


def validateJsonRequest(operation, error, requestBody):
    result = False

    if requestBody.get_status().lower() == Constants.ERROR_RESPONSE.lower():
        # error_mandatory_headers: x-hcx-status, x-hcx-sender_code, x-hcx-recipient_code, x-hcx-error_details,
        # x-hcx-correlation_id, x-hcx-api_call_id, x-hcx-timestamp
        result = validateHeadersData(requestBody,
            [Constants.STATUS, Constants.HCX_SENDER_CODE, Constants.HCX_RECIPIENT_CODE, Constants.ERROR_DETAILS,
             Constants.HCX_CORRELATION_ID, Constants.HCX_API_CALL_ID, Constants.HCX_TIMESTAMP],
            operation)
    else:
        # redirect_mandatory_headers: x-hcx-sender_code, x-hcx-recipient_code, x-hcx-api_call_id, x-hcx-timestamp,
        # x-hcx-correlation_id, x-hcx-status, x-hcx-redirect_to
        result = validateHeadersData(
            [Constants.HCX_SENDER_CODE, Constants.HCX_RECIPIENT_CODE, Constants.HCX_API_CALL_ID,
             Constants.HCX_TIMESTAMP, Constants.HCX_CORRELATION_ID, Constants.STATUS, Constants.REDIRECT_TO],
            operation)
        if result:
            return True
        # result = requestBody.validate_redirect(error)

    if result:
        logging.info("Request is validated successfully :: api call id: {}".format(json_request.get_api_call_id()))

    return result


def validateHeadersData(payloadArr, mandetoryHeaders, operation):
    header = base64.b64decode(payloadArr[0])
    header = header.decode("utf-8")
    if mandetoryHeaders != payloadArr:
        # TODO: raise mandetory headers missing error
        return True
    # validate header values
    return False
    
