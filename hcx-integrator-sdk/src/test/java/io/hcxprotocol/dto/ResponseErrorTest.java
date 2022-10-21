package io.hcxprotocol.dto;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;


class ResponseErrorTest {


    @Test
    void ResponseErrorTest() {

        ResponseError responseError = new ResponseError("code", "message", "trace");

        responseError.setCode("code");
        responseError.setMessage("message");
        responseError.setTrace("trace");

        assertEquals("code", responseError.getCode());
        assertEquals("message", responseError.getMessage());
        assertEquals("trace", responseError.getTrace());

    }
}
