import { stringify } from "qs";
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
  const result = await axios
    .post(url, payloadUrlencoded, { headers })
    .then((response) => response.data.access_token)
    .catch((error) => console.error("Generate HCX Token:", error));
  return result;
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

  const result = await axios
    .post(url, payload, { headers })
    .then((response) => response.data)
    .catch((error) => console.error("Search Registry:", error));
  return result;
}

export const decodeBase64String = (encodedString, clazz) => {
  const decodedString = base64.decode(encodedString);
  return JSON.parse(decodedString);
};
