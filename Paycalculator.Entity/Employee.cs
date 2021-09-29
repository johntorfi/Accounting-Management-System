using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Paycalculator.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string EmployeeNo { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [ MaxLength(50)]
        public string MiddleName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateJoined { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string NationalInsuranceNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StudentLoan StudentLoan { get; set; }
        [Required, MaxLength(150)]
        public string Address { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, MaxLength(50)]
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public IEnumerable<PaymentRecord>PaymentRecords { get; set; }



    }
}
