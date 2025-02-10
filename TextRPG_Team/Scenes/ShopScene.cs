using System.Diagnostics;

namespace TextRPG_Team.Scenes;

public class ShopScene : IScene
{
    public enum State
    {
        Default,    //기본   
        Buy,        //구매
        Sell,       //판매
    }

    State _state;

    private readonly GameState _gameState;

    public ShopScene(GameState gameState, State state = State.Default) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
    }
    
    // 현재 상태에 맞는 화면 표시
    private void ShowScreen()
    {
    }

    // 현재 상태에 따라 다음 씬 반환
    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch
        {
            1 => this,
            2 => new MainScene(_gameState), // MainScene()
            _ => null // 잘못된 입력 시 종료
        };
    }
}