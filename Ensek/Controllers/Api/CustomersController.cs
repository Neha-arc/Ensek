using CsvHelper;
using Ensek.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ensek.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // POST /api/customers
        
        [HttpPost]
        public IHttpActionResult UploadCustomers()
        {
            var httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["File"];
            //the parsing logic
            List<Customer> recordList = new List<Customer>();
            
            try
            {
                using (TextReader reader = new StreamReader(postedFile.InputStream))
                using (var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                {
                    
                    csvReader.Context.RegisterClassMap<CustomerMap>();

                    while (csvReader.Read())
                    {
                        try
                        {
                            var customer = csvReader.GetRecord<Customer>();
                            
                            recordList.Add(customer);
                            
                        }
                        catch (CsvHelperException ex)
                        {

                            //log error
                        }


                    }
                }

                var distinctItems = recordList.GroupBy(x => x.AccountId).Select(y => y.First());
                
                SaveCustomers(distinctItems);

            }
            catch (Exception e)
            {
                string str = e.Message;
                throw (e);
            }

            return Ok("Successfully uploaded file");

        }

        private void SaveCustomers(IEnumerable<Customer> customers)
        {
            foreach (var customer in customers)
            {
                _context.Customers.Add(customer);
            }
            _context.SaveChanges();
        }
    }
}
