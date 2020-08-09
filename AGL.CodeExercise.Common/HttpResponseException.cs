using System;

namespace AGL.CodeExercise.Common
{
    public class HttpResponseException:Exception
    {
        public string ResponseErrorCode { get; set; }
        public HttpResponseException(string responseErrorCode) :base()
        {
            ResponseErrorCode = responseErrorCode;
        }

        public HttpResponseException(string exceptionMessage, string responseErrorCode):base(exceptionMessage)
        {
            ResponseErrorCode = responseErrorCode;
        }

        public HttpResponseException(string exceptionMessage, Exception innerException, string responseErrorCode)
            :base(exceptionMessage, innerException)
        {
            ResponseErrorCode = responseErrorCode;
        }

        

    }
}
