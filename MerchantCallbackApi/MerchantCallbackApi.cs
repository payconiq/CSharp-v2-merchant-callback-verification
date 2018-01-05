using Force.Crc32;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MerchantCallbackApi
{
    public class MerchantCallbackApi
    {
        private byte[] dataToBeVerifiedByteArray;
        private string dataToBeVerified;
        private string hashAlgorithm;

        /**
         * Constructor for the MerchantCallbackApi class.
         *
         * string merchantId : Unique number used to identify merchant within Payconiq platform, acquired as part of the sign up process
         * string timestamp : When the request/event was generated. For instance : "2017-08-30T13:57:48.726Z";
         * string body : The webhook body is in JSON format with two additional fields: Payconiq’s unique transaction ID and the transaction status. A transaction ID is represented by
         * a 24 character hex string. It contains no empty spaces. For instance: "{"_id":"59a6c431f9285003bd46fe56","status":"SUCCEEDED"}";
         * string hashAlgorithm : The algorithm that Payconiq uses to generate the signature and that you can use to verify the signature.
         **/
        public MerchantCallbackApi(string merchantId, string timestamp, string body, string hashAlgorithm)
        {
            if (merchantId == null || merchantId.Equals(""))
            {

                throw new MerchantCallbackValidationException("Merchant id is a required parameter which should be filled.");
            }

            if (timestamp == null || timestamp.Equals(""))
            {

                throw new MerchantCallbackValidationException("Timestamp of the request/event generation is a required parameter which should be filled.");
            }

            if (body == null || body.Equals(""))
            {

                throw new MerchantCallbackValidationException("Webhook body is a required parameter which should be filled.");
            }

            if (hashAlgorithm == null || hashAlgorithm.Equals(""))
            {

                throw new MerchantCallbackValidationException("The algorithm that Payconiq uses to generate the signatureis a required parameter which should be filled.");
            }
            else if (!hashAlgorithm.Equals(MerchantCallbackApiConstants.CRYPTOGRAPHIC_HASH_ALGORITHM_SHA256))
            {
                throw new MerchantCallbackValidationException("The provided algorithm is not supported.");
            }

            // CRC32 Encryption
            String crc32Value = Crc32Algorithm.Compute(Encoding.Default.GetBytes(body)).GetHashCode().ToString("x2");

            // Data prepration
            this.dataToBeVerified = String.Format("{0}|{1}|{2}", merchantId, timestamp, crc32Value);
            this.dataToBeVerifiedByteArray = Encoding.Default.GetBytes(this.dataToBeVerified);
            this.hashAlgorithm = MerchantCallbackApiConstants.CRYPTOGRAPHIC_HASH_ALGORITHM_SHA256;
            this.hashAlgorithm = hashAlgorithm;
        }

        /**
         * Verify the signature based on the existing data and provided public key
         * string signature = The Payconiq-generated asymmetric signature using the algorithm specified with the X-Security-Algorithm(hashAlgorithm param)
         * string certificateLocation : location of the certificate which contains the public key. The X509 public key certificate from Payconiq. You can download the certificate from this URL and use it
         * to verify the signature (this will be different for the different environment — see section Payconiq Environments).
         * string hashAlgorithm : hash algorithm used for encryption
         * 
         * return bool, true if the signatre is verified, false otherwise
         **/
        public bool VerifySignature(string signature, string certificateLocation)
        {
            bool verified = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                try {
                    // encode the signature into byte array
                    byte[] signedSignatureBytes = Convert.FromBase64String(signature);

                    // import public key
                    rsa.ImportParameters(GetRSAPublicParam(certificateLocation));

                    // verify the signature based on provided data and public key of the certificate
                    verified = rsa.VerifyData(this.dataToBeVerifiedByteArray, CryptoConfig.MapNameToOID(this.hashAlgorithm), signedSignatureBytes);

                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                 
            }

            return verified;
        }

        /**
         * Retrieve the public key from the certificate and embed it in the public parameters of RSA
         * 
         * string certificateLocation : location of the certificate containing public key.
         **/
        private RSAParameters GetRSAPublicParam(string certificateLocation)
        {
            RSAParameters rsaPublicParam = new RSAParameters();

            try
            {
                // Load the certificate into an X509Certificate2 object.
                X509Certificate2 cer = new X509Certificate2();
                cer.Import(certificateLocation);

                // get the public key of the certificate
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cer.PublicKey.Key;
                rsaPublicParam = rsa.ExportParameters(false);
                            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }

            return rsaPublicParam;
        }
    }
}