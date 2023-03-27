package io.hcxprotocol.dto;

import io.hcxprotocol.init.HCXIntegrator;
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
        System.out.println("password 1 " + hcxIntegrator.getPassword());

        Map<String,Object> configMap1 = new HashMap<>();
        configMap1.put("protocolBasePath", "http://localhost:8095");
        configMap1.put("participantCode", "participant@01");
        configMap1.put("authBasePath", "http://localhost:8080");
        configMap1.put("username", "participant@gmail.com");
        configMap1.put("password", "67890");
        configMap1.put("encryptionPrivateKey", "Mz-VPPyU4RlcuYv1IwIvzw");
        configMap1.put("igUrl", "http://localhost:8090");
        HCXIntegrator hcxIntegrator1 = HCXIntegrator.getInstance(configMap1);

        assertEquals("67890", hcxIntegrator1.getPassword());
        System.out.println("password 2 " + hcxIntegrator1.getPassword());

        assertEquals("12345", hcxIntegrator.getPassword());
        System.out.println("password 1 " + hcxIntegrator.getPassword());
    }


}
