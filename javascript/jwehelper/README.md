# JWE Helper Library
## This is a Javascript library used to Encrypt and Decrypt JWE payload
### Prerequisite
```
Nodejs -V : v19.6.0
```
## Usage
### Encrypt JWE payload
````
const encryptData = await JWEHelper.encrypt({ publicKey, headers, payload });
````
### Decrypt JWE payload
````
const decryptData = await JWEHelper.decrypt({ privatekey, encryptedData });
````
### PublicKey and PrivateKey can be String or URL
Example for String
```
 "-----BEGIN CERTIFICATE----- MIICZjCCAc+gAwIBAgIUbNfXiWPtowJSN9UaBMghOt2liAEwDQYJKoZIhvcNAQEL BQAwRTELMAkGA1UEBhMCbHMxEzARBgNVBAgMClNvbWUtU3RhdGUxITAfBgNVBAoM GEludGVybmV0IFdpZGdpdHMgUHR5IEx0ZDAeFw0yMzAyMTcxMTQ3MTlaFw0yNDAy MTcxMTQ3MTlaMEUxCzAJBgNVBAYTAmxzMRMwEQYDVQQIDApTb21lLVN0YXRlMSEw HwYDVQQKDBhJbnRlcm5ldCBXaWRnaXRzIFB0eSBMdGQwgZ8wDQYJKoZIhvcNAQEB BQADgY0AMIGJAoGBAN6FJPqRVEfjDX1rX8yeSVVvLhxDB7PH9VDXNUK/bW17sAnH SXiNNbtMv22exgaK2JhXXlqoh+9r6QcyFOJfWT2NKb4HbetxAo7bqhIvCbtE2HJD Wu7Id8RE3XQuuVgzAAohnu7wuGtvL3yCjXjU6Wn9c4eZpeoUJwE9MrKrqY9hAgMB AAGjUzBRMB0GA1UdDgQWBBSwqDVEPro/0j8Zy/QC4FEXl1ZpozAfBgNVHSMEGDAW gBSwqDVEPro/0j8Zy/QC4FEXl1ZpozAPBgNVHRMBAf8EBTADAQH/MA0GCSqGSIb3 DQEBCwUAA4GBAJNHWbd1aYIi7XKP2dZT2eDlY4NcP96ELmJGnxwP28t2nnM9aTod Tsx24gU2/6byJ9ZvnLhaoJgA5c6TVdBF1EUPeUJjQA6kJ5BcGW3ZVMNeK6DwMAD9 QKpo4uS3JHWd62DhjASBFLcRW9hn3SgUts7zTHW8GL0fwne20h+Er0xb -----END CERTIFICATE-----"
```
Example for URL
```
  "https://private-key-url"
```