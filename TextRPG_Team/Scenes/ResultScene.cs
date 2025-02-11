namespace TextRPG_Team.Scenes;

using TextRPG_Team.Objects;
using static ConsoleColor;

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

    private void ShowScreen()
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("⚔️     BATTLE!!   ⚔️\n", Red);
        Utility.AlignCenter("RESULT\n");
        Console.WriteLine(new string('=', Utility.Width));
        Console.WriteLine();
        
        var player = _gameState.Player;
        var beforePlayer = _gameState.PlayerBeforeDungeon ?? player;
        
        if (beforePlayer.GetStats.Lv < player.GetStats.Lv)
            Utility.AddLog($"LEVEL UP!!! LV.{beforePlayer.GetStats.Lv} -> Lv.{player.GetStats.Lv}\n", Yellow);
        

        //Utiltiy.PrintLog로 대체가능
        if (_state == State.Victory)
        {
            int enemyCount = _gameState.Spawner.GetSpawnedEnemies().Count;
            Utility.ColorWriteLine(" 🏆  Victory!!\n", Yellow);
            Console.WriteLine($" 던전에서 몬스터 {enemyCount}마리를 처치했습니다.\n");
        }
        else
        {
            Utility.ColorWriteLine(" 💀 You Lose... \n", DarkRed);
            Console.WriteLine($" 던전에서 전투에 패배했습니다.\n");
        }
        ShowPlayerInfo();

        Console.WriteLine(" 0. 다음\n");
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 0);
        return input switch //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            0 => new MainScene(_gameState), // 메인 씬으로 돌아감
            _ => null // 잘못된 입력 시 종료
        };
    }

    private void ShowPlayerInfo()
    {
        var player = _gameState.Player;
        var beforePlayer = _gameState.PlayerBeforeDungeon ?? player;


        Console.WriteLine(new string('-', Utility.Width));

        Console.WriteLine(" [ 내정보 ]");
        Utility.AlignLeft(" ", 5);
        Utility.AlignLeft($"Lv.{player.GetStats.Lv}", 7);
        Console.WriteLine($"{player.Name}");
        Utility.AlignLeft(" ❤️   HP : ", 11);
        Utility.AlignLeft($" {beforePlayer.Health} -> {player.Health}\n", 4);
        Utility.AlignLeft(" 🆙  Exp : ", 11);
        Utility.AlignLeft($" {beforePlayer.GetStats.Lv} -> {player.GetStats.Lv}\n", 4);
        Utility.AlignLeft(" 💰  Gold : ", 11);
        Utility.AlignLeft($" {beforePlayer.Gold} -> {player.Gold}\n", 4);
        Console.WriteLine(new string('-', Utility.Width));
        Utility.PrintLogs();
    }
}