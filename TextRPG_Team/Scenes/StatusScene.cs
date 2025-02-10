namespace TextRPG_Team.Scenes;

public class StatusScene : IScene
{
    private readonly GameState _gameState;

    public StatusScene(GameState gameState) //DI 의존성 주입
    {
        _gameState = gameState;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
    }
    
    // 현재 상태에 맞는 화면 표시
    private void ShowScreen()
    {
        Utility.ColorWriteLine("상태보기", ConsoleColor.Blue);
        Console.WriteLine("플레이어의 정보를 확인할수 있습니다.\n");
        _gameState.Player.PrintInfo();
        Console.WriteLine();
        Console.WriteLine("0. 나가기\n");
    }

    // 현재 상태에 따라 다음 씬 반환
    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 0);
        return input switch
        {
            0 => new MainScene(_gameState), // MainScene()
            _ => null // 잘못된 입력 시 종료
        };
    }
}