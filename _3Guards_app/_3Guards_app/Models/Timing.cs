using System;
using SQLite;

namespace _3Guards_app.Models
{
    public class Timing
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Time { get; set; }
        public int ResultID { get; set; }
       
    }
}
