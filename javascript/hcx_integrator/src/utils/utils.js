const axios = require('axios');
const qs = require('querystring');

async function generateHcxToken(authBasePath, username, password) {
    const url = authBasePath;
    const payload = {
        client_id: "registry-frontend",
        username: username,
        password: password,
        grant_type: "password"
    };

    const config = {
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        }
    };

    try {
        const response = await axios.post(url, qs.stringify(payload), config);
        return response.data.access_token;
    } catch (e) {
        console.error(`Generate HCX Token: ${e}`);
    }
}

async function searchRegistry(protocolBasePath, token, searchValue, searchField = "participant_code") {
    const url = protocolBasePath + "/participant/search";
    const payload = {
        filters: {
            [searchField]: {
                eq: searchValue
            }
        }
    };

    const config = {
        headers: {
            Authorization: 'Bearer ' + token,
            'Content-Type': 'application/json'
        }
    };

    try {
        const response = await axios.post(url, payload, config);
        return response.data;
    } catch (e) {
        console.error(`Search Registry: ${e}`);
    }
}

module.exports = { generateHcxToken, searchRegistry };
