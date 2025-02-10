//using TextRPG_Mockup.Objects;

using TextRPG_Team.Objects;

namespace TextRPG_Team;

public class GameState
{
    //씬전환간에 필요한 인스터스들
    
    //Objects
    public readonly Player Player = new Player("Chad", new Stats(100, 10, 5), 1500);
}