using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Stores
{
    public class SkipTakeRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }

    public class ListCountResponse
    {
        public int Count { get; set; }
    }
}
