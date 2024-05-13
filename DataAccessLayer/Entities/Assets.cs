namespace DataAccessLayer
{
    public class Assets
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public int Id { get; set; }

        [ForeignKey("asset")]
        public int AssetId { get; set; }

        [ForeignKey("user")]
        public long UserId { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Status { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateOfReturn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Reason { get; set; }
        public virtual User user { get; set; }
        public virtual AssetDetail asset { get; set; }
    }
}
