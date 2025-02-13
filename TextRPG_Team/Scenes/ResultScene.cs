namespace TextRPG_Team.Scenes;

using static ConsoleColor;

public class ResultScene(GameState gameState, ResultScene.State state) : IScene
{
    readonly Random _random = new Random();
    public enum State
    {
        Victory, // 승리
        Lose, // 패배
    }

    //DI 의존성 주입

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기

        ShowScreen();
    }

    private void ShowScreen()
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("⚔️     BATTLE!!   ⚔️\n", Red);
        Utility.AlignCenter("RESULT\n");
        Console.WriteLine(new string('=', Utility.Width));
        Console.WriteLine();
        
        var player = gameState.Player;
        var beforePlayer = gameState.PlayerBeforeDungeon ?? player;
        
        if (beforePlayer.TotalStats.Lv < player.TotalStats.Lv)
            Utility.AddLog($"LEVEL UP!!! LV.{beforePlayer.TotalStats.Lv} -> Lv.{player.TotalStats.Lv}\n", Yellow);

        if (state == State.Victory)
        {
            HandleVictoryRewards();
        }
        else
        {
            Utility.ColorWriteLine(" 💀 You Lose... \n", DarkRed);
            Console.WriteLine($" 던전에서 전투에 패배했습니다.\n");
        }
        ShowPlayerInfo();

        Console.WriteLine(" 0. 다음\n");
    }

    void HandleVictoryRewards()
    {
        int totalGold = 0;
        var player = gameState.Player;
        var enemies = gameState.Spawner.GetSpawnedEnemies();
        string obtainedItem = "";

        foreach (var enemy in enemies)
        {
            if (enemy.IsDead())
            {
                totalGold += enemy.TotalStats.Lv * 100;

                // 30% 확률로 아이템 획득
                if (_random.Next(0, 100) < 60)
                {
                    string itemName = GetRandomItem();
                    obtainedItem += itemName + ", ";
                    var item = gameState.ItemList.FirstOrDefault(i => i.Name == itemName);
                    if (item != null)
                    {
                        player.AddItem(item.ShallowCopy()); // 인벤토리에 아이템 추가
                    }
                }
                
            }
        }
        player.Gold += totalGold;
         Utility.AddLog($" 보상: {totalGold} 골드 획득\n", Yellow);
        Utility.ColorWriteLine(" 🏆  Victory!!\n", Yellow);
        Console.WriteLine($" 던전에서 몬스터 {enemies.Count}마리를 처치했습니다.\n");

        if(!string.IsNullOrEmpty(obtainedItem))
        {
            obtainedItem = obtainedItem.TrimEnd(',', ' ');
            Utility.AddLog($"획득한 아이템: {obtainedItem}\n", Green);
        }
        gameState.Spawner.ClearNum += 1;
    }
    
    private string GetRandomItem()
    {
        string obtainedItem = gameState.ItemList[_random.Next(0,  gameState.ItemList.Count)].Name;
        return obtainedItem;
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 0);
        return input switch 
        {
            0 => new MainScene(gameState), // 메인 씬으로 돌아감
            _ => null // 잘못된 입력 시 종료
        };
    }
    private void ShowPlayerInfo()
    {
        var player = gameState.Player;
        var beforePlayer = gameState.PlayerBeforeDungeon ?? player;


        Console.WriteLine(new string('-', Utility.Width));

        Console.WriteLine(" [ 내정보 ]");
        Utility.AlignLeft(" ", 5);
        Utility.AlignLeft($"Lv.{player.TotalStats.Lv}", 7);
        Console.WriteLine($"{player.Name}");
        Utility.AlignLeft(" ❤️   HP : ", 11);
        Utility.AlignLeft($" {beforePlayer.Health} -> {player.Health}\n", 4);
        Utility.AlignLeft(" 🆙  lv : ", 11);
        Utility.AlignLeft($" {beforePlayer.TotalStats.Lv} -> {player.TotalStats.Lv}\n", 4);
        Utility.AlignLeft(" 💰  Gold : ", 11);
        Utility.AlignLeft($" {beforePlayer.Gold} -> {player.Gold}\n", 4);
        Console.WriteLine(new string('-', Utility.Width));
        Utility.PrintLogs();
    }
}