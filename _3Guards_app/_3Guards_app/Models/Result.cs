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

        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        public string ConductingName { get; set; }
        public string SupervisingName { get; set; }
        public string NeutralName { get; set; }

        public string ConductingSig { get; set; }
        public string SupervisingSig { get; set; }
        public string NeutralSig { get; set; }

        [OneToMany]
        public List<Timing> Timings { get; set; }
    }
}
