using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTaskAPI.DAL.Models
{
    public class Candidate
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string TimeInterval { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public string Comment { get; set; }

       
        public string ToCSVString()
        {
            var propertyInfos = typeof(Candidate).GetProperties();
            var values = new List<string>();

            foreach (var prop in propertyInfos)
            {
                var propertyValue = prop.GetValue(this, null);

                if (propertyValue != null)
                {
                    values.Add(propertyValue.ToString());
                }
                else
                {
                    values.Add("");
                }
            }

            return String.Join(",", values);
        }
    }
}
