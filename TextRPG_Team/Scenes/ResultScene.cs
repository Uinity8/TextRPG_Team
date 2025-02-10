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