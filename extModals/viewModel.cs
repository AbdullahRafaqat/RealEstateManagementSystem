using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.extModals
{
    public class viewModel
    {
        public string PropFor { get; set; }
        public string PropCity { get; set; }
        public string agentPhone { get; set; }
        public int ProID { get; set; }
        public int? ProType { get; set; }
        public int? noofBed { get; set; }
        public int? nooffloor { get; set; }
        public int? noofRooms { get; set; }
        public int? sizeSquare { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string description { get; set; }
        public string Postcode { get; set; }
        public string Address { get; set; }
        public DateTime? availablefrom { get; set; }
        public Int32? Price { get; set; }
        public string propName { get; set; }

        public string agentName { get; set; }
        public string agentEmail { get; set; }
        public string agentCity { get; set; }
        //public string agentName { get; set; }

        public int? imageId { get; set; }
        public byte[] imageProper { get; set; }
        public byte[] imageThum { get; set; }
        public int totalImages { get; set; }
    }
}