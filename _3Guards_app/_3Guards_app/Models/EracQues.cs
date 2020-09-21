using SQLite;
using SQLiteNetExtensions.Attributes;

namespace _3Guards_app.Models
{
    public class EracQues
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public bool Q1 { get; set; }
        public bool Q2 { get; set; }
        public bool Q3 { get; set; }
        public bool Q4 { get; set; }
        public bool Q5 { get; set; }
        public bool Q6 { get; set; }
        public bool Q7 { get; set; }
        public bool Q8 { get; set; }
        public bool Q9 { get; set; }
        public bool Q10 { get; set; }

        [ForeignKey(typeof(Erac))]
        public int EracID { get; set; }
    }
}
