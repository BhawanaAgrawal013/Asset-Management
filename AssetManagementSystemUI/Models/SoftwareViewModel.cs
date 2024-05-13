namespace AssetManagementSystemUI
{
    public class SoftwareViewModel
    {
        public long Id { get; set; }

        [DisplayName("Serial Id")]
        public string SerialId { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid manufacturing name")]
        [DisplayName("Company")]
        public string Source { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Incorrect Date Format")]
        [DisplayName("Date of Purchase")]
        public DateTime Date { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid type")]
        public string Type { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity not in the correct range")]
        [Required]
        public int Quantity { get; set; }

        [DisplayName("Category Name")]
        public int? CategoryId { get; set; }
    }
}
