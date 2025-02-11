

namespace TextRPG_Team.Scenes;
using static Utility.Alignment;
using static ConsoleColor;

public class MainScene : IScene
{
    private readonly GameState _gameState;

    public MainScene(GameState gameState) // DI 의존성 주입
    {
        _gameState = gameState;
    }

    // 씬 실행 메서드
    public void Run()
    {
        Console.Clear(); // 화면 초기화
        ShowScreen(); // 메뉴 출력
    }
    
    // 메뉴 화면 출력
    private void ShowScreen()
    {
        int width = 5;
        Console.WriteLine(new string('=',Utility.Width));
        Utility.AlignCenter("⚔️  스파르타 던전에 오신 여러분 환영합니다.⚔️\n", Blue);
        Utility.AlignCenter("이제 전투를 시작할 수 있습니다.\n");
        Console.WriteLine(new string('=',Utility.Width));
        Console.WriteLine();
        Utility.AlignLeft(" 1.", width);
        Console.WriteLine("상태 보기\n");
        Utility.AlignLeft(" 2.", width);
        Console.WriteLine("인벤토리\n");
        Utility.AlignLeft(" 3.", width);
        Console.WriteLine("상 점\n");
        Utility.AlignLeft(" 4.", width);
        Console.WriteLine("전투시작\n");
        Console.WriteLine(new string('-',Utility.Width));
        Console.WriteLine("\n 0. 💾 저장/종료\n");
    }

    // 다음 씬 결정
    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 4);
        return input switch
        {
            1 => new StatusScene(_gameState), //상태보기 
            2 => new InventoryScene(_gameState), //인벤토리
            3 => new ShopScene(_gameState), //상점
            4 => new BattleScene(_gameState), //배틀 시작

            0 => null, //저장/ 종료
            _ => null,
        };
    }
}
