using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace _3Guards_app.Models
{
    public class Result
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }

        [OneToMany]
        public List<Timing> Timings { get; set; }
    }
}
