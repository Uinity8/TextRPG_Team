namespace TextRPG_Team.Scenes;

using static ConsoleColor;

public class BattleScene(GameState gameState, BattleScene.State state = BattleScene.State.Default)
    : IScene
{
    public enum State
    {
        Default, // 기본 화면
        PlayerPhase, // 플레이어의 행동 차례
        PlayerResult, // 플레이어의 공격 결과
        EnemyPhase // 적의 차례
    }

    // 현재 상태
    // 게임 상태 공유

    public void Run()
    {
        Console.Clear(); // 화면 초기화
        ShowScreen(); // 현재 상태에 맞는 화면 출력
        Console.WriteLine();
    }

    public IScene? GetNextScene()
    {
        return state switch
        {
            State.Default => GetInputForDefault(),
            State.PlayerPhase => GetInputForPlayerPhase(),
            State.PlayerResult => GetInputForPlayerResult(),
            State.EnemyPhase => GetInputForEnemyPhase(),
            _ => null
        };
    }

    private IScene? GetInputForDefault()
    {
        int input = Utility.GetInput(0, 1); // 사용자 입력 받음
        return input switch
        {
            0 => RunAway(),
            1 => new BattleScene(gameState, State.PlayerPhase), // 플레이어 턴으로 이동
            _ => null
        };
    }

    private IScene RunAway()
    {
        var enemies = gameState.Spawner.GetSpawnedEnemies();

        // 남아있는 몬스터들이 공격
        foreach (var enemy in enemies)
        {
            if (gameState.Player.IsDead()) break; // 플레이어 사망 시 중단
            if (enemy.IsDead()) continue; // 죽은 몬스터는 공격 안 함

            Console.Clear();
            Console.WriteLine(new string('=', Utility.Width));
            Utility.AlignCenter("⚔️     도저히 못 이길 것 같다! 빤쓰런!   ⚔️\n", Red);
            Console.WriteLine(new string('=', Utility.Width));
            Console.WriteLine("");
            Utility.AlignCenter($" LV.{enemy.TotalStats.Lv} {enemy.Name} 의 기습공격!\n");

            enemy.PerformAttack(gameState.Player);
            Utility.PrintLogs();
            ShowPlayerInfo();

            Console.WriteLine();
            Utility.ColorWrite(" 엔터키를 눌러서 계속...", DarkGreen);
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter) break;
            }
        }

        // 플레이어가 살아있으면 메인 씬으로 이동
        return gameState.Player.IsDead()
            ? new ResultScene(gameState, ResultScene.State.Lose)
            : new MainScene(gameState);
    }

    private IScene GetInputForPlayerPhase()
    {
        int max = gameState.Spawner.GetSpawnedEnemies().Count;
        int input = Utility.GetInput(0, max, " 공격할 대상을 선택하세요.");
        switch (input)
        {
            case 0:
                return new BattleScene(gameState); // 취소 시 기본 상태로 복귀
            default:
                var enemy = gameState.Spawner.GetSpawnedEnemies()[input - 1];
                if (enemy.IsDead())
                {
                    Utility.AddLog("이미 죽었습니다.\n", ConsoleColor.Red);
                    return this;
                }

                gameState.Player.PerformAttack(enemy);

                //상태를 PlayerResult로 변경하여 공격 반복 방지
                return new BattleScene(gameState, State.PlayerResult);
        }
    }

    private IScene GetInputForPlayerResult()
    {
        Utility.ColorWrite(" 엔터키를 눌러서 계속...", DarkGreen);
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
                return new BattleScene(gameState, State.EnemyPhase); // 적의 턴으로 이동
        }
    }

    private IScene GetInputForEnemyPhase()
    {
        if (gameState.Player.IsDead())
            return new ResultScene(gameState, ResultScene.State.Lose);

        var enemies = gameState.Spawner.GetSpawnedEnemies();
        if (enemies.FindAll(e => !e.IsDead()).Count == 0)
            return new ResultScene(gameState, ResultScene.State.Victory);

        return new BattleScene(gameState);
    }

    private void ShowScreen()
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("⚔️     BATTLE!!   ⚔️\n", Red);
        Console.WriteLine(new string('=', Utility.Width));

        switch (state)
        {
            case State.Default:
                DefaultScreen();
                break;
            case State.PlayerPhase:
                PlayerPhaseScreen();
                break;
            case State.PlayerResult:
                PlayerResultScreen();
                break;
            case State.EnemyPhase:
                EnemyPhaseScreen();
                break;
        }
    }

    private void DefaultScreen()
    {
        Console.WriteLine();

        // 적 정보 표시
        Utility.ColorWriteLine(" [ 적 정보 ]");
        var enemies = gameState.Spawner.GetSpawnedEnemies();
        foreach (var enemy in enemies)
        {
            if (enemy.IsDead())
                enemy.PrintInfo(DarkGray);
            else
                enemy.PrintInfo();
        }

        Console.WriteLine();

        // 플레이어 정보 표시
        ShowPlayerInfo();

        Utility.AlignLeft(" 1. ⚔️   공격", 15);
        Console.WriteLine(" 0. 🏃‍♂️  빤쓰런");
    }

    private void PlayerPhaseScreen()
    {
        Utility.ColorWriteLine("Battle!! - 플레이어 공격\n", ConsoleColor.Yellow);

        // 적 선택 목록 표시
        var enemies = gameState.Spawner.GetSpawnedEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            Console.WriteLine($" {i + 1}. ");
            if (enemies[i].IsDead())
                enemies[i].PrintInfo(DarkGray);
            else
                enemies[i].PrintInfo();
        }

        // Console.WriteLine();
        // 플레이어 정보 표시
        ShowPlayerInfo();
        Console.WriteLine(" 0. 취소");
    }

    private void PlayerResultScreen()
    {
        Console.WriteLine("");
        Utility.AlignCenter($" {gameState.Player.Name}의 공격!\n");
        for (int i = 0; i < 2; i++)
            Console.WriteLine(new string(' ', Utility.Width));
        // 공격 결과 로그 출력
        Utility.PrintLogs();
        for (int i = 0; i < 4; i++)
            Console.WriteLine(new string(' ', Utility.Width));

        // 플레이어 정보 표시
        ShowPlayerInfo();
    }

    private void EnemyPhaseScreen()
    {
        var enemies = gameState.Spawner.GetSpawnedEnemies();
        foreach (var enemy in enemies)
        {
            if (gameState.Player.IsDead())
                break;

            if (enemy.IsDead()) continue;

            Console.Clear();
            Console.WriteLine(new string('=', Utility.Width));
            Utility.AlignCenter("⚔️     BATTLE!!   ⚔️\n", Red);
            Console.WriteLine(new string('=', Utility.Width));
            Console.WriteLine("");
            Utility.AlignCenter($"[ LV.{enemy.TotalStats.Lv} {enemy.Name}의 반격! ]\n");

            for (int i = 0; i < 2; i++)
                Console.WriteLine(new string(' ', Utility.Width));
            enemy.PerformAttack(gameState.Player);
            Utility.PrintLogs();
            for (int i = 0; i < 4; i++)
                Console.WriteLine(new string(' ', Utility.Width));

            // 플레이어 정보 표시
            ShowPlayerInfo();
            Console.WriteLine();

            Utility.ColorWrite(" 엔터키를 눌러서 계속...", DarkGreen);
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter) break;
            }
        }
    }

    void ShowPlayerInfo()
    {
        Console.WriteLine(new string('-', Utility.Width));
        var player = gameState.Player;
        Console.WriteLine(" [ 내정보 ]");
        Utility.AlignLeft(" ", 4);
        Utility.AlignLeft($"Lv.{player.TotalStats.Lv}", 7);
        Console.WriteLine($"{player.Name}");
        Utility.AlignLeft(" ❤️   HP : ", 10);
        Utility.AlignLeft($"{player.Health}", 2);
        Console.WriteLine($" / {player.TotalStats.MaxHp}");
        Utility.AlignLeft(" 🆙 Exp : ", 11);
        Utility.AlignLeft($"{player.Exp}", 2);
        Console.WriteLine($"/ {player.TotalStats.MaxExp}");
        Console.WriteLine(new string('-', Utility.Width));
        Utility.PrintLogs();
    }
}