using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Entities
{
    public class Customer : TrackerEntity
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public int IDType { get; set; }
        public string? IDNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsSelfRegistered { get; set; }
        public string? OTPCode { get; set; }
        public bool IsOTPVerified { get; set; }
        public bool IsForgetPasswordOTPVerified { get; set; }

    }

}