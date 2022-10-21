package io.hcxprotocol.dto;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;


 class HttpResponseTest {


    @Test
    void HttpResponseTest() throws Exception {

        HttpResponse httpResponse = new HttpResponse(200, "body");
        assertEquals(200, httpResponse.getStatus());
        assertEquals("body", httpResponse.getBody());


    }
}
