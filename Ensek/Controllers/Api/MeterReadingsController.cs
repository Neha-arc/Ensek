using CsvHelper;
using Ensek.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ensek.Controllers.Api
{
    public class MeterReadingsController : ApiController
    {

        private ApplicationDbContext _context;

        public MeterReadingsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult UploadMeterReading()
        {
            var httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["File"];
            //the parsing logic
            List<MeterReading> recordList = new List<MeterReading>();
            int failedRecords = 0;
            int passedRecords = 0;
            try
            { 
                using (TextReader reader = new StreamReader(postedFile.InputStream))
                using (var csvReader = new CsvReader(reader,System.Globalization.CultureInfo.CurrentCulture))
                {
                    //csvReader.Configuration.MissingFieldFound = null;
                    csvReader.Context.RegisterClassMap <MeterReadingMap> ();

                    while (csvReader.Read())
                    {
                        try
                        {
                            var meterReading = csvReader.GetRecord<MeterReading>();
                           if( Validate(meterReading))
                                recordList.Add(meterReading);
                           else
                                failedRecords = failedRecords + 1;
                        }
                        catch (CsvHelperException ex) {

                            failedRecords = failedRecords+ 1;
                        }
                       
                       
                    }
                }
                
                var distinctItems = recordList.GroupBy(x => x.AccountId).Select(y => y.First());
                
               // passedRecords = distinctItems.Count();
                passedRecords = SaveMeterReadings(distinctItems);

            }
            catch(Exception e)
            {
                string str = e.Message;
                throw (e);
            }
      
               return Ok("Successful Readings: "+ passedRecords+System.Environment.NewLine+"Failed Readings: "+failedRecords);
            
        }

        private int SaveMeterReadings(IEnumerable<MeterReading> meterReadings)
        {
            int recordsAdded = 0;
            foreach(var reading in meterReadings)
            {
                //if accountid present in customers table only then add meter reading
                if(_context.Customers.Any(c=> c.AccountId == reading.AccountId))
                { 
                    _context.MeterReadings.Add(reading);
                    recordsAdded = recordsAdded+1;
                }
            }
            _context.SaveChanges();

            return recordsAdded;
        }

        private bool Validate(MeterReading reading) 
        {
            var context = new ValidationContext(reading, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(reading, context, validationResults, true);

            return isValid;
        }
    }
}
