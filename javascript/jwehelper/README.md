# JWE Helper Library
## This is a Javascript library used to Encrypt and Decrypt JWE payload
### Prerequisite
```
Nodejs -V : v19.6.0
```
## Usage

### PublicKey and PrivateKey can be String or URL
### Example for String
```
 "-----BEGIN CERTIFICATE----- MIICZjCCAc+gAwIBAgIUbNfXiWPtowJSN9UaBMghOt2liAEwDQYJKoZIhvcNAQEL BQAwRTELMAkGA1UEBhMCbHMxEzARBgNVBAgMClNvbWUtU3RhdGUxITAfBgNVBAoM GEludGVybmV0IFdpZGdpdHMgUHR5IEx0ZDAeFw0yMzAyMTcxMTQ3MTlaFw0yNDAy MTcxMTQ3MTlaMEUxCzAJBgNVBAYTAmxzMRMwEQYDVQQIDApTb21lLVN0YXRlMSEw HwYDVQQKDBhJbnRlcm5ldCBXaWRnaXRzIFB0eSBMdGQwgZ8wDQYJKoZIhvcNAQEB BQADgY0AMIGJAoGBAN6FJPqRVEfjDX1rX8yeSVVvLhxDB7PH9VDXNUK/bW17sAnH SXiNNbtMv22exgaK2JhXXlqoh+9r6QcyFOJfWT2NKb4HbetxAo7bqhIvCbtE2HJD Wu7Id8RE3XQuuVgzAAohnu7wuGtvL3yCjXjU6Wn9c4eZpeoUJwE9MrKrqY9hAgMB AAGjUzBRMB0GA1UdDgQWBBSwqDVEPro/0j8Zy/QC4FEXl1ZpozAfBgNVHSMEGDAW gBSwqDVEPro/0j8Zy/QC4FEXl1ZpozAPBgNVHRMBAf8EBTADAQH/MA0GCSqGSIb3 DQEBCwUAA4GBAJNHWbd1aYIi7XKP2dZT2eDlY4NcP96ELmJGnxwP28t2nnM9aTod Tsx24gU2/6byJ9ZvnLhaoJgA5c6TVdBF1EUPeUJjQA6kJ5BcGW3ZVMNeK6DwMAD9 QKpo4uS3JHWd62DhjASBFLcRW9hn3SgUts7zTHW8GL0fwne20h+Er0xb -----END CERTIFICATE-----"
```
### Example for URL
```
  "https://private-key-url"
```
### Encrypt JWE payload
````
const encryptData = await JWEHelper.encrypt({ publicKey, headers, payload });
````
### Output of Encrypted data
```
eyJhbGciOiJSU0EtT0FFUC0yNTYiLCJ0eXAiOiJqd3QiLCJlbmMiOiJBMjU2R0NNIiwia2lkIjoicVNHNjh0UlAyQzJaempEVnN5TzQtS2NteFhURHp6NGloNGFwUWRxbmxiMCJ9.TQnzqEwtfzCC06pI_wlXXGFt9gWhChGVhM4moFLvy1E29hA8pVK1WhhsZSZnGR7RIGT-YKOgZ5TN_iR3KRLpFeWcsfM3g2q3EoTosp4mFizqCUEL3GOe_1yLf_j9oDBLMae5H5i56BYBej-M2OIc4mqpguegCQT04AM7Ci3UW-aAzjpzC87JWavCXGnI4dp2xU0egilvedOqrofDzy6y4NrdvtTZFpxiAU-28cEf-bWf3aCrEkV-catX53S-lfnARZnvC9T99c6vmm9T7NY9l9ZABFbsIVQ3N44m-4QB7JEOi62Ynxjmn3ilOg7BX4K8LZaqv5RqIAG1b6ONtsmwqg.f_7MQAn_Qw_kXWkf.IDBMwZ9enF04Pnuqg17Ie7SNFwZziuBRMnxnUBKlVC0uadNmBhhGWkNEYC5PvGxGQfpvI3HX8brIYTv5OXUNNTdVhg.EqdBRhIZNs8kf9FXADOSVg
```
### Decrypt JWE payload
````
const decryptData = await JWEHelper.decrypt({ privatekey, encryptedData });
````
### Output of Decrypted data 
```
{
  header: {
    alg: 'RSA-OAEP-256',
    typ: 'jwt',
    enc: 'A256GCM',
    kid: 'qSG68tRP2C2ZzjDVsyO4-KcmxXTDzz4ih4apQdqnlb0'
  },
  payload: { iss: 'testpayor1.icici@swasth-hcx-dev', sub: 'payor-test-user-3' }
}
```