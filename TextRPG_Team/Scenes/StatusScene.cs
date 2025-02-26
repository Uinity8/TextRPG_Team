namespace TextRPG_Team.Scenes;

using static ConsoleColor;

public class StatusScene(GameState gameState) : IScene
{
    //DI 의존성 주입

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
    }

    // 현재 상태에 맞는 화면 표시
    private void ShowScreen()
    {
        Console.WriteLine(new string('=',Utility.Width));
        Utility.AlignCenter("상태보기\n", DarkCyan);
        Utility.AlignCenter("플레이어의 정보를 확인할 수 있습니다.\n");
        Console.WriteLine(new string('=',Utility.Width));
        gameState.Player.PrintInfo();
        Console.WriteLine();
        Console.WriteLine(" 0. 나가기\n");
    }

    // 현재 상태에 따라 다음 씬 반환
    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 0);
        return input switch
        {
            0 => new MainScene(gameState), // MainScene()
            _ => null // 잘못된 입력 시 종료
        };
    }
}