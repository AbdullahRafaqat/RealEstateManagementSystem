using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.extModals
{
    public class showAgents
    {
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string city { get; set; }
        public DateTime? Dated { get; set; }
        public bool? activation { get; set; }
        public int agentID { get; set; }
    }
}