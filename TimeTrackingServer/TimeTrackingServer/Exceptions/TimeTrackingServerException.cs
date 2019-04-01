using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackingServer.Exceptions
{
    public class TimeTrackingServerException : Exception
    {
        public int ErrorCode { get; set; }

        public TimeTrackingServerException(int errorCode, string message) : this(errorCode, message, null) { }

        public TimeTrackingServerException(int errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public class LoginAttemptFailedException : TimeTrackingServerException
        {
            public LoginAttemptFailedException() : this(null) { }
            public LoginAttemptFailedException(Exception innerException) : base(1, "Имя пользователя или пароль не распознаны", innerException) { }
        }
    }
}
