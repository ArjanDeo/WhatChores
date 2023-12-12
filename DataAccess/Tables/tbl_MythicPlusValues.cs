using System.ComponentModel.DataAnnotations;

namespace DataAccess.Tables
{
    public class tbl_MythicPlusValues
    {
        [Key]
        public int KeyLevel { get; set; }
        public int ItemLevel { get; set; }
    }
}
