using System;
using SQLite;

namespace _3Guards_app.Models
{
    public class Result
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }



    }
}
