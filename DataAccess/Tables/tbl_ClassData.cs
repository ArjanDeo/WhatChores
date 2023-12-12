using System.ComponentModel.DataAnnotations;

namespace DataAccess.Tables
{
    public class tbl_ClassData
    {
        [Key]
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassColor { get; set; }
    }
}
