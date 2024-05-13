namespace DataAccessLayer
{
    public class AssetCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public int Id { get; set; }

        public string serialId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Name { get; set; }
        public virtual ICollection<AssetDetail> AssetDetails { get; set; }
    }
}
