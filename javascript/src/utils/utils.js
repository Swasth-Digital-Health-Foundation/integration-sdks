import { stringify } from "qs";
import { base64url } from "jose";
import axios from "axios";

export async function generateHcxToken(authBasePath, username, password) {
  const url = authBasePath;
  const payload = {
    client_id: "registry-frontend",
    username: username,
    password: password,
    grant_type: "password",
  };
  const payloadUrlencoded = stringify(payload);
  const headers = {
    "content-type": "application/x-www-form-urlencoded",
  };
  try {
    const response = await axios.post(url, payloadUrlencoded, { headers });
    return response.data.access_token;
  } catch (error) {
    throw new Error(`Generate HCX Token Error: ${error.message}`);
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
