using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.RicoClient.Scripts.Utils
{
    public static class JWTDecoder
    {   
        /// <summary>
        /// Decodes main part of the given token
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Decoded token</returns>
        public static Dictionary<string, object> Decode(string token)
        {
            string[] tokenParts = token.Split('.');
            if (tokenParts.Length == 3)
            {
                string decode = tokenParts[1];
                int padLength = 4 - decode.Length % 4;
                if (padLength < 4)
                    decode += new string('=', padLength);
          
                byte[] bytes = Convert.FromBase64String(decode);
                string data = Encoding.ASCII.GetString(bytes);

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            }
            else
                return null;
        }
    }
}
