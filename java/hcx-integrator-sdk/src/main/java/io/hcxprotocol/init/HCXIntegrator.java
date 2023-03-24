package io.hcxprotocol.init;

import com.typesafe.config.Config;
import com.typesafe.config.ConfigFactory;
import io.hcxprotocol.impl.HCXIncomingRequest;
import io.hcxprotocol.impl.HCXOutgoingRequest;
import io.hcxprotocol.utils.Operations;
import org.apache.commons.lang3.StringUtils;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

/**
 * The methods and variables to access the configuration. Enumeration of error codes and operations.
 */
public class HCXIntegrator {

    private static Config config = null;

    private static HCXIntegrator hcxIntegrator = null;

    private HCXIntegrator() {
    }

    /**
     * This method is to initialize config factory by passing the configuration as Map.
     *
     * @param configMap A Map that contains configuration variables and its values.
     */
    public static HCXIntegrator getInstance(Map<String,Object> configMap) throws Exception {
        config = ConfigFactory.parseMap(configMap);
        if(config == null)
            throw new Exception("Please initialize the configuration variables, in order to initialize the SDK");
        validateConfig();
        hcxIntegrator = new HCXIntegrator();
        return hcxIntegrator;
    }

    public boolean processIncoming(String jwePayload, Operations operation, Map<String, Object> output) throws Exception {
        return new HCXIncomingRequest().process(jwePayload, operation, output, hcxIntegrator);
    }

    public boolean processOutgoing(String fhirPayload, Operations operation, String recipientCode, Map<String,Object> output) throws Exception {
        return new HCXOutgoingRequest().generate(fhirPayload, operation, recipientCode, output, hcxIntegrator);
    }

    public boolean processOutgoing(String fhirPayload, Operations operation, String actionJwe, String onActionStatus, Map<String,Object> output) throws Exception {
        return new HCXOutgoingRequest().generate(fhirPayload, operation, actionJwe, onActionStatus, output, hcxIntegrator);
    }

    private static void validateConfig() throws Exception {
        List<String> props = Arrays.asList("protocolBasePath", "participantCode", "authBasePath", "username", "password", "encryptionPrivateKey", "igUrl");
        for(String prop: props){
            if(!config.hasPathOrNull(prop) || StringUtils.isEmpty(config.getString(prop)))
                throw new Exception(prop + " is missing or has empty value, please add to the configuration.");
        }
    }

    public String getHCXProtocolBasePath() {
        return config.getString("protocolBasePath");
    }

    public String getParticipantCode() {
        return config.getString("participantCode");
    }

    public String getAuthBasePath() {
        return config.getString("authBasePath");
    }

    public String getUsername() {
        return config.getString("username");
    }

    public String getPassword() {
        return config.getString("password");
    }

    public String getPrivateKey() {
        return config.getString("encryptionPrivateKey");
    }

    public String getIGUrl() {
        return config.getString("igUrl");
    }

}
