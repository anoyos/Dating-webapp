using james.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom
{


    public struct Result<T> where T : class
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public IEnumerable<T> Data { get; set; }
        public long Count { get; set; }
    }

    public struct Result
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }

}
