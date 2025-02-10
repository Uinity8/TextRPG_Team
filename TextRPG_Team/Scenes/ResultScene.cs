using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes;


public class ResultScene : IScene
{
    private readonly GameState _gameState;
    
    public enum State
    {
        Victory, // 승리
        Lose, // 패배
    }

    State _state;

    public ResultScene(GameState gameState, State state) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        
        ShowScreen();
    }

    private void ShowScreen( )
    {
        Utility.ColorWriteLine("Battle!! - Result\n", ConsoleColor.Yellow);
        
        //Utiltiy.PrintLog로 대체가능
        if (_state == State.Victory)
        {
            int enemyCount = _gameState.Spawner.GetSpawnedEnemies().Count;
            var player = _gameState.Player;
            Console.WriteLine("Victory\n");
            Console.WriteLine($"던전에서 몬스터 {enemyCount}마리를 잡았습니다.\n"); 
            Console.WriteLine($"Lv.{player.GetStats.Lv} {player.Name}");
            Console.WriteLine($"HP {_gameState.PlayerHpBeforeDungeon} -> {player.Health}\n");
        }
        else
        {
            var player = _gameState.Player;
            Console.WriteLine("You Lose\n");
            Console.WriteLine($"Lv.{player.GetStats.Lv} {player.Name}");
            Console.WriteLine($"HP {_gameState.PlayerHpBeforeDungeon} -> {player.Health}\n");

        }
        
        Console.WriteLine("0. 다음");
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 0);
        return input switch     //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            0 => new MainScene(_gameState), // 메인 씬으로 돌아감
            _ => null // 잘못된 입력 시 종료
        };
    }
}