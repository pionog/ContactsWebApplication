using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsWebApplication.Models
{
    public class AccountDetail
    {
        [Key]
        public int AccountID { get; set; }

        [Column(TypeName = "nchar(32)")]
        public string Name { get; set; } = "";

        [Column(TypeName = "nchar(32)")]
        public string Surname { get; set; } = "";

        [Column(TypeName = "nchar(64)")]
        public string Email { get; set; } = "";

        [Column(TypeName = "nchar(128)")]
        public string Password { get; set; } = "";

        [Column(TypeName = "nchar(8)")]
        public string Category { get; set; } = "";

        [Column(TypeName = "nchar(32)")]
        public string SubCategory { get; set; } = "";

        //assuming only Polish phone numbers
        [Column(TypeName = "nchar(9)")]
        public string PhoneNumber { get; set; } = "";

        //DD/MM/YYYY
        [Column(TypeName = "nchar(10)")]
        public string BirthDate { get; set; } = "";

    }
}
