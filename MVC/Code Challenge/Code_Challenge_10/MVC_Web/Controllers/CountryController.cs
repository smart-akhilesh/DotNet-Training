using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MVC_Web.Models;

namespace MVC_Web.Controllers
{
    public class CountryController : ApiController
    {
        
        private static List<CountryModel> countries = new List<CountryModel>()
        {
            new CountryModel { ID = 1, CountryName = "India", Capital = "New Delhi" },
            new CountryModel { ID = 2, CountryName = "Nepal", Capital = "Kathmandu" },
            new CountryModel { ID = 3, CountryName = "Japan", Capital = "Tokyo" },
            new CountryModel { ID = 4, CountryName = "UK", Capital = "London" },
            new CountryModel { ID = 5, CountryName = "Germany", Capital = "Berlin" },
        };

        [HttpGet]
        public IHttpActionResult GetCountries()
        {
            return Ok(countries);
        }

    
        [HttpPost]
        public IHttpActionResult AddCountry(CountryModel country)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");

            country.ID = countries.Max(c => c.ID) + 1;
            countries.Add(country);

            return Ok(country);
        }


        [HttpPut]
        public IHttpActionResult UpdateCountry(int id, CountryModel updatedCountry)
        {
            var country = countries.FirstOrDefault(c => c.ID == id);
            if (country == null)
                return NotFound();

            country.CountryName = updatedCountry.CountryName;
            country.Capital = updatedCountry.Capital;

            return Ok(country);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCountry(int id)
        {
            var country = countries.FirstOrDefault(c => c.ID == id);
            if (country == null)
                return NotFound();

            countries.Remove(country);
            return Ok("Deleted Successfully");
        }
    }
}
