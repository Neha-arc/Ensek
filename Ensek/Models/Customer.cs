using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper.Configuration;
using System.Globalization;


namespace Ensek.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

      
       
    }

    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            Map(m => m.Id).Ignore();
            
        }
    }
}