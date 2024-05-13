namespace DataAccessLayer
{
    public class AssetDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string SerialId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Source { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; }

        [Column(TypeName = "datetime")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Column(TypeName = "bigint")]
        public int Quantity { get; set; }

        [ForeignKey("assetCategory")]
        public int? CategoryId { get; set; }

        public virtual AssetCategory assetCategory { get; set; }

        public virtual ICollection<Assets> Assets { get; set; }
    }
}
