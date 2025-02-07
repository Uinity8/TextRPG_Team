using System;
using System.Collections.Generic;

namespace TextRPG_Team.Objects
{
    internal class Enemy
    {
        //적 속성

        public int Level { get; private set; }
        public string Name { get; private set; }
        public int HP { get; private set; }
        public int Attack { get; private set; }


        public Enemy(int level, string name, int hP, int attack)
        {
            Level = level;
            Name = name;
            HP = hP;
            Attack = attack;
        }

        public void ShowInfo() 
        {
            Utility.ColorWriteLine($"Lv.{Level}, {Name}, HP{HP}", ConsoleColor.Magenta);
        }
    }
}
