using TextRPG_Team.Objects;

namespace TextRPG_Team;

public class GameState
{
    //씬전환간에 필요한 인스터스들
    
    //Objects
    readonly Player  _player = new Player("Payer", new Stats(100, 10, 5), 1000);
    public Player GetPlayer() => _player;
}