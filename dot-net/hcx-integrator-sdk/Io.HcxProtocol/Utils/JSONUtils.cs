﻿using Hl7.Fhir.Utility;
using Newtonsoft.Json;

using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The JSON Utils to convert a .net Type object to JSON string and vise versa.
    /// </summary>
    public static class JSONUtils
    {
       // private static readonly Maper mapper = new Mapper();
        

      
       




        public static T DecodeBase64String<T>(string encodedString)
        {
            int mod4 = encodedString.Length % 4;
            if (mod4 > 0)
            {
                encodedString += new string('=', 4 - mod4);
            }
            byte[] decodedBytes = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(decodedBytes);
           
            return JsonConvert.DeserializeObject<T>(decodedString);
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T Deserialize<T>(string value, Type clazz)
        {
           // return mapper.readValue(value, clazz);
           return  JsonConvert.DeserializeObject<T>(value);
          
            
        }

    }
}
