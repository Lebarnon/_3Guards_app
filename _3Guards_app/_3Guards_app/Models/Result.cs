using System;
using SQLite;

namespace _3Guards_app.Models
{
    public class Result
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }

        //use constructors to instantiate list
        //but dont use list use multiple tables (many to one concept)
        //go read up more on sqlite see how it works
    }
}
