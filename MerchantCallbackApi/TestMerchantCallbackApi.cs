using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MerchantCallbackApi
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            String signedSignature = "IrJO4pIA8mhRKAHM7731FmZNDfBIcdce/4H9N22mldZZEWgfLcolc6xEIaEtHvCcZ1HVdL1KKeXp10FdwAq/3NVHY1RkoBMstUtdQQ9UgKnFovIkGhYm3u7N5G9YM7iivX3AOMiBSCBgTAVsan731e57LTc3Q1klWlSJwyqdwncYYvXv41Aov0UpnzE0UWgmJjmffbezHZOxiWP/dXZV5sCkYSvK5oe0WbLGsJzR8yFuByNCfrv6NDN6V45YvnicXJ1+CPnuR6cEngCxmQEYE0K668IaD3B2zblLiTb89b23ft/E8LaUsM2iNEgI7f1LuL7FzNW09KrPYgDFPFurHA==";
            String merchantId = "5981e8a4c9716c3dca30679d";
            String timestamp = "2017-08-30T13:57:48.726Z";
            String body = "{\"_id\":\"59a6c431f9285003bd46fe56\",\"status\":\"SUCCEEDED\"}";
            String hashAlgorithm = MerchantCallbackApiConstants.CRYPTOGRAPHIC_HASH_ALGORITHM_SHA256;
            String certLocation = @"../../TestCertificate/payconiq.crt";

            MerchantCallbackApi merchantCallbackApi = new MerchantCallbackApi(merchantId, timestamp, body, hashAlgorithm);
            Console.WriteLine(merchantCallbackApi.VerifySignature(signedSignature, certLocation));

           
        }
    }
}

