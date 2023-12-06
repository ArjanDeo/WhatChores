using System.ComponentModel.DataAnnotations;

namespace DataAccess.Tables
{
    public class tbl_USRealms
    {
        [Key]
        public int RealmID { get; set; }
        [Required]
        public string RealmName { get; set; }
    }
}
