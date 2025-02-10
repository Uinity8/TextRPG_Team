namespace TextRPG_Team.Scenes;

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
        GetNextScene()?.Run(); // 선택한 씬 실행
    }
    
    // 메뉴 화면 출력
    private void ShowScreen()
    {
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 상점");
        Console.WriteLine("3. 전투 시작");
        Console.WriteLine("\n0. 저장/종료\n");
    }

    // 다음 씬 결정
    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 3);
        switch (input)
        {
            case 1:
                return new StatusScene(_gameState);
            case 2:
                return new ShopScene(_gameState);
            case 3:
                return new BattleScene(_gameState);
            default:
                return this;
        }
    }
}
