using System;
namespace MerchantCallbackApi
{
    /**
    * Class to specify the exceptions which can occure in the signature verification
    **/
    public class MerchantCallbackValidationException : Exception
    {
        public MerchantCallbackValidationException(String message) : base(message) 
        {}
    }
}
