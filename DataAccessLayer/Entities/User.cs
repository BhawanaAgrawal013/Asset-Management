namespace DataAccessLayer
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Gender { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string City { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string State { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Column(TypeName = "varchar(12)")]
        public string Phone { get; set; }

        [Column(TypeName = "varchar(8)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column(TypeName = "datetime")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "bit")]
        public bool IsAdmin { get; set; }

        public virtual ICollection<Assets> Assets { get; set; }

    }
}
