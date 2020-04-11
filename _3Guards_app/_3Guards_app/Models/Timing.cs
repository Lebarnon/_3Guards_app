using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace _3Guards_app.Models
{
    public class Timing
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Time { get; set; }

        [ForeignKey(typeof(Result))]
        public int ResultID { get; set; }
    }
}
