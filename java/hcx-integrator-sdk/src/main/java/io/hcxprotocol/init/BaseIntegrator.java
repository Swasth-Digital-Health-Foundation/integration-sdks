package io.hcxprotocol.init;

import com.typesafe.config.Config;
import com.typesafe.config.ConfigFactory;
import io.hcxprotocol.exception.ClientException;
import io.hcxprotocol.impl.HCXIncomingRequest;
import io.hcxprotocol.impl.HCXOutgoingRequest;
import io.hcxprotocol.interfaces.IncomingRequest;
import io.hcxprotocol.interfaces.OutgoingRequest;
import io.hcxprotocol.utils.Constants;
import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

public class BaseIntegrator {
    private Config config;

    private static Logger logger = LoggerFactory.getLogger(BaseIntegrator.class);

    private void validateConfig() throws Exception {
        logger.debug("Integrator SDK configuration.", getConfig());
        if(config == null)
            throw new Exception("Please initialize the configuration variables, in order to initialize the SDK");
        List<String> props = config.getStringList("configKeys");
        for(String prop: props){
            if(!config.hasPathOrNull(prop) || StringUtils.isEmpty(config.getString(prop)))
                throw new Exception(prop + " is missing or has empty value, please add to the configuration.");
        }
    }

    protected void setConfig(Map<String, Object> map) throws Exception {
        Config fallbackConfig = ConfigFactory.load();
        this.config = ConfigFactory.parseMap(map).withFallback(fallbackConfig);
        validateConfig();
    }

    protected Config getConfig(){
        return this.config;
    }

    protected IncomingRequest getIncomingRequest() throws ClientException {
        if (getConfig().hasPathOrNull(("incomingRequestClass"))) {
            String className = getConfig().getString("incomingRequestClass");
            try {
                Class<?> clazz = Class.forName(className);
                Object instance = clazz.newInstance();
                logger.info("Incoming request class provided in the config exists :: class name: " + className);
                return (IncomingRequest) instance;
            } catch (ClassNotFoundException | InstantiationException | IllegalAccessException e) {
                logger.error("Incoming request class provided in the config map does not exist :: class name: " + className);
                throw new ClientException("Incoming request class provided in the config map does not exist :: class name: " + className);
            }
        }
        return new HCXIncomingRequest();
    }

    protected OutgoingRequest getOutgoingRequest() throws ClientException {
        if (getConfig().hasPathOrNull(("outgoingRequestClass"))) {
            String className = getConfig().getString("outgoingRequestClass");
            try {
                Class<?> clazz = Class.forName(className);
                Object instance = clazz.newInstance();
                logger.info("Outgoing request class provided in the config exists :: class name: " + className);
                return (OutgoingRequest) instance;
            } catch (ClassNotFoundException | InstantiationException | IllegalAccessException e) {
                logger.error("Outgoing request class provided in the config map does not exist :: class name: " + className);
                throw new ClientException("Outgoing request class provided in the config map does not exist :: class name: " + className);
            }
        }
        return new HCXOutgoingRequest();
    }

    /**
     * This method is to get the hcx protocol base path.
     */
    public String getHCXProtocolBasePath() {
        return config.getString(Constants.PROTOCOL_BASE_PATH);
    }

    /**
     * This method is to get the participant code.
     */
    public String getParticipantCode() {
        return config.getString(Constants.PARTICIPANT_CODE);
    }

    /**
     * This method is to get the authorization base path.
     */
    public String getAuthBasePath() {
        return config.getString(Constants.AUTH_BASE_PATH);
    }

    /**
     * This method is to get the username.
     */
    public String getUsername() {
        return config.getString(Constants.USERNAME);
    }

    /**
     * This method is to get the password.
     */
    public String getPassword() {
        return config.getString(Constants.PASSWORD);
    }

    /**
     * This method is to get the encryption private key.
     */
    public String getPrivateKey() {
        return config.getString(Constants.ENCRYPTION_PRIVATE_KEY);
    }

    /**
     * This method is to get the HCX implementation guide base path.
     */
    public String getHCXIGBasePath() {
        return config.getString(Constants.HCX_IG_BASE_PATH);
    }

    /**
     * This method is to get the NRCES implementation guide base path.
     */
    public String getNRCESIGBasePath() {
        return config.getString(Constants.NRCES_IG_BASE_PATH);
    }
}
