using TextRPG_Team.Objects;

namespace TextRPG_Team;

public class GameState
{
    //씬전환간에 필요한 인스터스들
    public Queue<(string, ConsoleColor)> Logs { get; } = new(); //Log Info를 저장할 Queue입니다.
    
    //Objects
    readonly Player  _player = new Player("Payer", new Stats(100, 10, 5), 1000);
    public Player GetPlayer() => _player;
        
    //이벤트 추가
    public GameState()
    {
        _player.TryBuyAction = (tryResult, color) => Logs.Enqueue((tryResult, color));
    }
}