using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealEstate.extModals
{
    public class editModel
    {
        [Required(ErrorMessage = "The Property Name is required")]
        public string propName { get; set; }

        [Required(ErrorMessage = "The Price is required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Price must be a Valid number")]
        public Int32? Price { get; set; }

        [Required(ErrorMessage = "The Size of Property is required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Size of Property must be a Valid number")]
        public Int32? SizeSquare { get; set; }

        [Required(ErrorMessage = "The Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The Post code is required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Post Code must be a Valid number")]
        public Int32 Postcode { get; set; }

        [Required(ErrorMessage = "The Available from Date is required")]
        [DataType(DataType.Date)]
        public string availablefrom { get; set; }

        [Required(ErrorMessage = "The Description of Property is required")]
        public string description { get; set; }

        //public city cities { get; set; }

        public bool? Status { get; set; }
       
        public string PropFor { get; set; }
        public string PropCity { get; set; }
  
        public string imagProp { get; set; }
        public int ProID { get; set; }
        public int? CityID { get; set; }
        public int? ProType { get; set; }
        public int? noofBed { get; set; }
        public int? nooffloor { get; set; }
        public int? noofrooms { get; set; }  
    }
}