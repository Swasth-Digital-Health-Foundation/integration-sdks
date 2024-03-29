import os, inspect
import requests
import json
from urllib.parse import urlencode 

def generateHcxToken(authBasePath, username, password):
    """
    Parameters
    ----------
    authBasePath: str
        Authentication base path to generate token
    username: str
        Username of the sender/recipient
    password: str
        Password of the sender/recipient

    Returns
    -------
    HCXToken: str
        generated HCX token as a string. 
    """
    url = authBasePath
    payload = {"client_id":"registry-frontend",
                "username":username,
                "password":password,
                "grant_type":"password"}
    payload_urlencoded = urlencode(payload)
    headers = {
        'content-type': 'application/x-www-form-urlencoded'
    }

    try:
        response = requests.request("POST", url, headers=headers, data=payload_urlencoded)
        y = json.loads(response.text)
        return y["access_token"]  
    except Exception as e:
        print(f'Generate HCX Token: {e}')


def searchRegistry(protocolBasePath, token, searchValue, searchField="participant_code"):
    """
    Parameters
    ----------
    protocolBasePath: str
        Base path of HCX instance to get access to protocol APIs
    token: str
        HCX token from genrateHCXToken method
    searchValue: str
        Unique id for which the registry is to be searched
    searchField: str (default: participant_code)
        The key for which the value is provided.

    Returns
    -------
    registry: dict
        Registry value for the search value 
    """
    url = protocolBasePath + "/participant/search"
    payload = json.dumps({
        "filters": {
            searchField: {
            "eq": searchValue
            }
        }
    })
    headers = {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    }
    try:
        response = requests.request("POST", url, headers=headers, data=payload)    
        return dict(json.loads(response.text))
    except Exception as e:
        print(f'Search Registry: {e}')

def get_curdir():
    """
    Returns
    -------
    str : The directory in which executed file exists.
    """
    return os.path.dirname(os.path.abspath(inspect.getfile(inspect.currentframe())))

def compare_output(a, b):
    """
    Parameters
    ----------
    a : json/dict
    b : json/dict

    Returns
    -------
    bool : True, if a & b is equal
           False, if a & b is not equal.
    """
    typea = isinstance(a, dict)
    typeb = isinstance(b, dict)
    if not typea == typeb:
        print("Type of A doesn't match B")
        return False
    if not sorted(a.keys())==sorted(b.keys()):
        return False
    return True