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
        Console.WriteLine("상태보기");
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
        Console.WriteLine("Lv. 01");
        Console.WriteLine("직업 ( 전사 )");
        Console.WriteLine("공격력 : 10 ");
        Console.WriteLine("방어력 : 5");
        Console.WriteLine("체력 : 100");
        Console.WriteLine("Gold : 1500 G");
        Console.WriteLine("\n0. 나가기\n");

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