using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Ensek.Models
{
    public class MeterReading
    {

        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        [RegularExpression(@"\b\d{5}\b")]
        public int? MeterReadValue { get; set; }

        [Required]
        public DateTime? MeterReadingDateTime { get; set; }
    }

    public class MeterReadingMap : ClassMap<MeterReading>
    {
        public MeterReadingMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            Map(m => m.Id).Ignore();
            //Map(m => m.Name).Validate(field => !field.Contains("-"));
        }
    }
}