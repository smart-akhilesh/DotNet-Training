using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC_Web.Models
{
    public class CountryModel
    {
       
            public int ID { get; set; }
            public string CountryName { get; set; }
            public string Capital { get; set; }
        
    }
}