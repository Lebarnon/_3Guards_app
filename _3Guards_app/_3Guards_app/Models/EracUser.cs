using SQLite;
using SQLiteNetExtensions.Attributes;

namespace _3Guards_app.Models
{
    public class EracUser
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Nric { get; set; }

        [ForeignKey(typeof(Erac))]
        public int EracID { get; set; }
    }
}
