using System.ComponentModel.DataAnnotations;

namespace DataAccess.Tables
{
    public class tbl_VaultRaidBosses
    {
        [Key]
        public string Boss {  get; set; }
    }
}
