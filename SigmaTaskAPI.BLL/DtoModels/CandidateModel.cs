using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTaskAPI.BLL.DtoModels
{
    public class CandidateModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string TimeInterval { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}