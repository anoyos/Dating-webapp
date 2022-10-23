using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnUser
    {
        public int id { get; internal set; }
        public string photo { get; internal set; }
        public string name { get; internal set; }
        public int? age { get; internal set; }
        public int likes { get; internal set; }
        public double rating { get; internal set; }
        public string eduction { get; internal set; }
        public string profession { get; internal set; }
        public string vaccine { get; internal set; }
        public double? distance { get; internal set; }
        public string relation { get; internal set; }
    }
}
