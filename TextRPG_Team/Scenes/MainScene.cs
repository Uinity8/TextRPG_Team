using TextRPG_Team.Sound;

namespace TextRPG_Team.Scenes;

using static ConsoleColor;
using Objects;
using Manager;

public class MainScene(GameState gameState) : IScene
{
    // 씬 실행 메서드
    public void Run()
    {
        // 배경 음악 재생
        SoundManager.PlayBackgroundMusic("resources/Sounds/BattleBgm.wav", true);
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
        Console.WriteLine($"전투시작 (현재 층수 : {gameState.Spawner.ClearNum}층)\n");
        Utility.AlignLeft(" 5.", width);
        Console.WriteLine("퀘스트\n");
        Console.WriteLine(new string('-',Utility.Width));
        Console.WriteLine("\n 0. 💾 저장/종료\n");
    }

    // 다음 씬 결정
    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 5);
        switch (input)
        {
            case 1:
                return new StatusScene(gameState); // 상태보기
            case 2:
                return new InventoryScene(gameState); // 인벤토리
            case 3:
                return new ShopScene(gameState); // 상점
            case 4:
                Player player = gameState.Player;
                gameState.PlayerBeforeDungeon = new Player(player.Name, player.TotalStats, player.Gold, player.Job); 
                gameState.Spawner.AddRandomEnemies();
                return new BattleScene(gameState); // 배틀 시작
            case 0:// 저장 / 종료
                LoadManager.SavePlayerData(gameState.Player);
                Environment.Exit(0);
                return null; 
            case 5:
                return new QuestScene(gameState);
            default:
                return null; // 잘못된 입력 처리
        }
    }
}
