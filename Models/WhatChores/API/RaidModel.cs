using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.WhatChores.API
{
    public class RaidEncounter
    {
        public string Boss { get; set; }
        public string Difficulty { get; set; }
       // public DateTime LastKillTimestamp { get; set; }
    }

    public class RaidModel
    {
        public string Raid { get; set; }
        public List<RaidEncounter> Encounters { get; set; }
    }

}
