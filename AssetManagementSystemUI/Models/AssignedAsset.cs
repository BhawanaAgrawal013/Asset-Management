namespace AssetManagementSystemUI
{
    public class AssignedAsset
    {
        public int id {  get; set; }
        public long AssetId { get; set; }

        public long UserId { get; set; }

        public string Status { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Incorrect Date Format")]
        [DisplayName("Date of Return")]
        public DateTime DateOfReturn { get; set; }

        [Required]
        public string Reason { get; set; }

        public virtual UserViewModel user { get; set; }

        public virtual AssetDetailViewModel asset { get; set; }
    }
}
