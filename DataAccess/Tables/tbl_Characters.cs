using System.ComponentModel.DataAnnotations;

namespace DataAccess.Tables
{
    public class tbl_Characters
    {
        [Key]
        public Guid CharacterId { get; set; }
        [Required]
        public required DateTime CreatedAt { get; set; }
        [Required]
        public required DateTime LastUpdatedAt { get; set; }
        [Required]
        [StringLength(int.MaxValue)]
        public required string CharacterData { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Realm { get; set; }
        [Required]
        public required string Region { get; set; }
    }
}
