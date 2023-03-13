package io.hcxprotocol.dto;

import io.hcxprotocol.impl.HCXIntegrator;
import org.junit.jupiter.api.Test;

import java.util.HashMap;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.assertEquals;

 class HCXIntegratorTest {

    @Test
    void testInitializeConfigMap() throws Exception {
        Map<String,Object> configMap = new HashMap<>();
        configMap.put("protocolBasePath", "http://localhost:8095");
        configMap.put("participantCode", "participant@01");
        configMap.put("authBasePath", "http://localhost:8080");
        configMap.put("username", "participant@gmail.com");
        configMap.put("password", "12345");
        configMap.put("encryptionPrivateKey", "Mz-VPPyU4RlcuYv1IwIvzw");
        configMap.put("igUrl", "http://localhost:8090");

        HCXIntegrator hcxIntegrator = HCXIntegrator.getInstance(configMap);

        assertEquals("http://localhost:8095", hcxIntegrator.getHCXProtocolBasePath());
        assertEquals("participant@01", hcxIntegrator.getParticipantCode());
        assertEquals("http://localhost:8080", hcxIntegrator.getAuthBasePath());
        assertEquals("participant@gmail.com", hcxIntegrator.getUsername());
        assertEquals("12345", hcxIntegrator.getPassword());
        assertEquals("Mz-VPPyU4RlcuYv1IwIvzw", hcxIntegrator.getPrivateKey());
        assertEquals("http://localhost:8090", hcxIntegrator.getIGUrl());

        configMap.put("password", "67890");
        hcxIntegrator = HCXIntegrator.getInstance(configMap);

        assertEquals("67890", hcxIntegrator.getPassword());
    }


}
