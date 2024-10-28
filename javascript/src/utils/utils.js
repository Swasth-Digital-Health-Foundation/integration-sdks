import { stringify } from "qs";
import { base64url } from "jose";
import axios from "axios";
import { Constants } from "../utils/Constants.js";

// {
//   username: this.username,
//   password: null,
//   secret: this.secret,
//   partic
// }
export async function generateToken(authBasePath, config) {
  const url = authBasePath;
  const payload = {
    // grant_type: "password",
    // grant_type: "secret",
    ...config
  };
  console.log(payload)
  const payloadUrlencoded = stringify(payload);
  const headers = {
    "content-type": "application/x-www-form-urlencoded",
  };
  try {
    console.log(url)
    console.log(payloadUrlencoded);
    const response = await axios.post(url, payloadUrlencoded, { headers });
    console.log("generation token successful");
    console.log('=========================END')
    return response.data.access_token;
  } catch (error) {
    const serverErrorMessage = error.response && error.response.data && error.response.data.message ? 
      error.response.data.message : 
      error.message;
      console.log(error);
      console.log('-----------------------------------------------------')
    console.error(`Generate HCX Token Error: ${serverErrorMessage}`);
    throw new Error(`Generate HCX Token Error: ${serverErrorMessage}`);
  }
}
export async function searchRegistry(
  protocolBasePath,
  token,
  searchValue,
  searchField = "participant_code"
) {
  const url = protocolBasePath + "/participant/search";
  const payload = JSON.stringify({
    filters: {
      [searchField]: {
        eq: searchValue,
      },
    },
  });
  const headers = {
    Authorization: "Bearer " + token,
    "Content-Type": "application/json",
  };

  try {
    const response = await axios.post(url, payload, { headers });
    return response.data;
  } catch (error) {
    throw(`Search Registry Error: ${error.message}`);
  }
}

export const decodeBase64String = (encodedString) => {
  const decodedString = base64url.decode(encodedString);
  return JSON.parse(decodedString);
};
