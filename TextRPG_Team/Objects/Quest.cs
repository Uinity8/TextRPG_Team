namespace TextRPG_Team.Objects
{
    public class Quest(string name, string info, bool accep, bool clear, bool acquisitionint, int compensation, int id)
    {
        public string Name {  get; set; } = name;
        public string Info { get; set; } = info;
        public bool Accep { get; set; } = accep;

        public bool Clear { get; set; } = clear;
        public bool Acquisition {  get; set; } = acquisitionint;

        public int Compensation { get; set; } = compensation;
        public int Id {  get; set; } = id;
    }
}
