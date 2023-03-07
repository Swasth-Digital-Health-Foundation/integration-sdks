package io.hcxprotocol.utils;

import io.hcxprotocol.exception.ClientException;
import io.hcxprotocol.init.HCXIntegrator;
import io.hcxprotocol.dto.HttpResponse;
import io.hcxprotocol.exception.ServerException;
import lombok.experimental.UtilityClass;
import org.apache.http.client.ClientProtocolException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * The common utils functionality used in HCX Integrator SDK.
 * <ol>
 *     <li>Generation of authentication token using HCX Gateway and Participant System Credentials.</li>
 *     <li>HCX Gateway Participant Registry Search.</li>
 * </ol>
 */

@UtilityClass
public class Utils {

    private static final Logger logger = LoggerFactory.getLogger(Utils.class);

    // TODO: In the initial version we are not handling the token caching, it will be handled in the next version
    public static String generateToken() throws Exception {
        Map<String,String> headers = new HashMap<>();
        headers.put("content-type", "application/x-www-form-urlencoded");
        Map<String,Object> fields = new HashMap<>();
        fields.put("client_id", "registry-frontend");
        fields.put("username", HCXIntegrator.getInstance().getUsername());
        fields.put("password", HCXIntegrator.getInstance().getPassword());
        fields.put("grant_type", "password");
        HttpResponse response = HttpUtils.post(HCXIntegrator.getInstance().getAuthBasePath(), headers, fields);
        Map<String,Object> respMap = JSONUtils.deserialize(response.getBody(), Map.class);
        String token;
        if (response.getStatus() == 200) {
            token = (String) respMap.get("access_token");
        } else if (response.getStatus() == 401) {
            logger.error("Error while generating API access token: Invalid credentials");
            throw new ClientException("Error while generating API access token: Invalid credentials");
        } else {
            logger.error("Error while generating API access token :: status: " + response.status + " :: message: " + respMap);
            throw new ServerException("Error while generating API access token :: status: " + response.status + " :: message: " + respMap);
        }
        return token;
    }

    public static Map<String,Object> searchRegistry(Object participantCode) throws Exception {
        String filter = "{\"filters\":{\"participant_code\":{\"eq\":\"" + participantCode + "\"}}}";
        Map<String,String> headers = new HashMap<>();
        headers.put(Constants.AUTHORIZATION, "Bearer " + generateToken());
        HttpResponse response = HttpUtils.post(HCXIntegrator.getInstance().getHCXProtocolBasePath() + "/participant/search", headers, filter);
        Map<String,Object> respMap = JSONUtils.deserialize(response.getBody(), Map.class);
        List<Map<String,Object>> details;
        if (response.getStatus() == 200) {
            details = (List<Map<String, Object>>) respMap.get(Constants.PARTICIPANTS);
        } else {
            String errMsg;
            if(respMap.get("error") instanceof String) {
                errMsg = respMap.get("error").toString();
            } else {
                errMsg = ((Map<String,Object>) respMap.getOrDefault("error",  new HashMap<>())).getOrDefault("message", respMap).toString();
            }
            logger.error("Error while fetching the participant details from the registry :: status: " + response.getStatus() + " :: message: " + errMsg);
            throw new ServerException("Error while fetching the participant details from the registry :: status: " + response.getStatus() + " :: message: " + errMsg);
        }
        return !details.isEmpty() ? details.get(0) : new HashMap<>();
    }

}
