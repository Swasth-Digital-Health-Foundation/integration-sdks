import { stringify } from "qs";
import { base64url } from "jose";
import axios from "axios";

export async function generateToken(authBasePath, config) {
  const url = authBasePath;
  const payload = {
    ...config
  };
  const payloadUrlencoded = stringify(payload);
  const headers = {
    "content-type": "application/x-www-form-urlencoded",
  };
  try {
    const response = await axios.post(url, payloadUrlencoded, { headers });
    return response.data.access_token;
  } catch (error) {
    const serverErrorMessage = error.response && error.response.data && error.response.data.message ? 
      error.response.data.message : 
      error.message;
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
