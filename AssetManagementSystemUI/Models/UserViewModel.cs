namespace AssetManagementSystemUI
{
    public class UserViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid name")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid name")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid City")]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid State")]
        public string State { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage ="Phone Number cannot be more than 10")]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [StringLength(7, ErrorMessage = "Password should only be of 7 characters")]
        [Required]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Incorrect Date Format")]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public bool IsAdmin { get; set; }

    }
}
