namespace AssetManagementSystemUI
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "Password should be of length 7")]
        public string Password { get; set; }
    }
}
