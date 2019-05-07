using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackingServer.Exceptions
{
    public interface IErrorBase
    {
        int ErrorCode { get; set; }
        string FailReason { get; set; }
    }

    public class ErrorBase : IErrorBase
    {
        public ErrorBase()
        {
            ErrorCode = 0;
            FailReason = "";
        }

        public int ErrorCode { get; set; }
        public string FailReason { get; set; }
        public string ExceptionType { get; set; }
    }
}
