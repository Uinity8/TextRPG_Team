using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes;

public class MainScene : IScene
{
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
        Console.WriteLine("2.Shop Scene");   
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch
        {
            1 => this,
            2 => new ShopScene(_gameState,ShopState.Normal),
            _ => null // 잘못된 입력 시 종료
        };
    }
}

