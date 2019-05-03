using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackingServer.Exceptions
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }

        public ApiException(int errorCode, string message) : this(errorCode, message, null) { }

        public ApiException(int errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public class ApiBadRequest : ApiException
        {
            public ApiBadRequest() : this(null) { }
            public ApiBadRequest(Exception innerException) : base(1, "Ошибка в отправляемых данных", innerException) { }
        }
    }
}
