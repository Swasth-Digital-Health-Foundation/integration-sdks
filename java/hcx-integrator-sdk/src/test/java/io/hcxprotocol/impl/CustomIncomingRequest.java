package io.hcxprotocol.impl;

import io.hcxprotocol.utils.Operations;

import java.util.Map;

public class CustomIncomingRequest  extends HCXIncomingRequest {

    @Override
    public boolean validateRequest(String jwePayload, Operations operation, Map<String, Object> error) {
        System.out.println("Custom implementation is executed");
        return true;
    }

}
