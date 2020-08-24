using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace _3Guards_app.Models
{
    class Erac
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        [OneToMany]
        public List<EracUser> EracUsers { get; set; }
    }
}
