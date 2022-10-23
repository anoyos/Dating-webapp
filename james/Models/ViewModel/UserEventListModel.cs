using james.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.ViewModel
{
    public class UserEventListModel
    {
        public List<Event> events { get; set; }
    }
    public class UserEventModel
    {
        public Event data { get; set; }
    }
}
