using System;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Models.SchoolViewModels
{
    public class EnrollmentDateGroups
    {
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
        public int StudentCount { get; set; }
    }
}