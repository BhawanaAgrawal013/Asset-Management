using Newtonsoft.Json.Serialization;

namespace AssetManagementSystemUI
{
    public class AssetViewModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid name")]
        [DisplayName("Asset")]
        public string AssetName { get; set; }

        [Required]
        public long AssetId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date cannot be null")]
        [DisplayName("Date of Return")]
        public DateTime DateOfReturn { get; set; }

        [Required]
        [DisplayName("Type")]
        public string AssetType { get; set; }

        [Required]
        [DisplayName("Asset Name")]
        public string AssignedAsset { get; set; }

        [Required]
        [DisplayName("User Email")]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Give a more precise reason within 40 letters")]
        public string Reason { get; set; }

        public virtual UserViewModel user { get; set; }

        public virtual AssetDetailViewModel asset { get; set; }
    }
}
