# JWE Helper Library
## This is a Javascript library used Encrypt and Decrypt JWE payload
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
const decryptData = await JWEHelper.decrypt({ privatekey, encryptData });
````