using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes;

public class MainScene : IScene
{
    private readonly Player _player;
    private readonly GameState _gameState;
    public MainScene(GameState gameState) //DI 의존성 주입
    {
        _gameState = gameState;
    }
    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        
        //예제 코드
        Console.WriteLine("Main Scene 입니다");      
        Console.WriteLine("1.Main Scene");      
        Console.WriteLine("2.Example Scene");   
    }

     public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 3);
        return input switch
        {
            1 => this,
            2 => new ExampleScene(_gameState),
            3 => new BattleScene(_gameState, _player), // BattleScene 호출 추가
            _ => null // 잘못된 입력 시 종료
        };
    }
}