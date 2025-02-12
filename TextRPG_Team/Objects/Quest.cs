using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRPG_Team.Objects
{
    public class Quest
    {
        public string Name {  get; set; }
        public string Info { get; set; }
        public bool Accep { get; set; }

        public bool Clear { get; set; }

        public int Compensation { get; set; }
        public int Id {  get; set; }

        public Quest(string name, string info, bool accap, bool clear, int compensation, int id)
        {
            Name = name;
            Info = info;
            Accep = accap;
            Clear = clear;
            Compensation = compensation;
            Id = id;
        }



    }
}
