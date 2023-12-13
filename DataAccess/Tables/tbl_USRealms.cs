using System.ComponentModel.DataAnnotations;

namespace DataAccess.Tables
{
    public class tbl_USRealms
    {
        [Key]
        public int RealmID { get; set; }        
        public string RealmName { get; set; }
    }
}
