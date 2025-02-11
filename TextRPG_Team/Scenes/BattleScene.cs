using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes;

using static ConsoleColor;

public class BattleScene : IScene
{
    public enum State
    {
        Default, // 기본 화면
        PlayerPhase, // 플레이어의 행동 차례
        PlayerResult, // 플레이어의 공격 결과
        EnemyPhase // 적의 차례
    }

    private State _state; // 현재 상태
    private readonly GameState _gameState; // 게임 상태 공유


    public BattleScene(GameState gameState, State state = State.Default)
    {
        _gameState = gameState;
        _state = state;
    }

    public void Run()
    {
        Console.Clear(); // 화면 초기화
        ShowScreen(); // 현재 상태에 맞는 화면 출력
        Console.WriteLine();
    }

    public IScene? GetNextScene()
    {
        return _state switch
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
        int input = Utility.GetInput(1, 1); // 사용자 입력 받음
        return input switch
        {
            1 => new BattleScene(_gameState, State.PlayerPhase), // 플레이어 턴으로 이동
            _ => null
        };
    }

    private IScene? GetInputForPlayerPhase()
    {
        int max = _gameState.Spawner.GetSpawnedEnemies().Count;
        int input = Utility.GetInput(0, max, " 공격할 대상을 선택하세요.");
        switch (input)
        {
            case 0:
                return new BattleScene(_gameState); // 취소 시 기본 상태로 복귀
            default:
                var enemy = _gameState.Spawner.GetSpawnedEnemies()[input - 1];
                if (enemy.IsDead())
                {
                    Utility.AddLog("이미 뒤졌는데요\n", ConsoleColor.Red);
                    return this;
                }

                _gameState.Player.PerformAttack(enemy); // 특정 적 공격
                return new BattleScene(_gameState, State.PlayerResult); // 결과 화면으로 이동
        }
    }

    private IScene? GetInputForPlayerResult()
    {
        Utility.ColorWrite(" 엔터키를 눌러서 계속...", DarkGreen);
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
                return new BattleScene(_gameState, State.EnemyPhase); // 적의 턴으로 이동
        }
    }

    private IScene? GetInputForEnemyPhase()
    {
        if (_gameState.Player.IsDead())
            return new ResultScene(_gameState, ResultScene.State.Lose);

        var enemies = _gameState.Spawner.GetSpawnedEnemies();
        if (enemies.FindAll(e => !e.IsDead()).Count == 0)
            return new ResultScene(_gameState, ResultScene.State.Victory);

        return new BattleScene(_gameState, State.Default);
    }

    private void ShowScreen()
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("⚔️     BATTLE!!   ⚔️\n", Red);
        Console.WriteLine(new string('=', Utility.Width));

        switch (_state)
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
        var enemies = _gameState.Spawner.GetSpawnedEnemies();
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

        Console.WriteLine(" 1. ⚔️   공격");
    }

    private void PlayerPhaseScreen()
    {
        Utility.ColorWriteLine("Battle!! - 플레이어 공격\n", ConsoleColor.Yellow);

        // 적 선택 목록 표시
        var enemies = _gameState.Spawner.GetSpawnedEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            Console.WriteLine($" {i + 1}. ");
            if (enemies[i].IsDead())
                enemies[i].PrintInfo(DarkGray);
            else
                enemies[i].PrintInfo();
        }

        Console.WriteLine();

        // 플레이어 정보 표시
        ShowPlayerInfo();
        Console.WriteLine(" 0. 취소");
    }

    private void PlayerResultScreen()
    {
        Console.WriteLine($" {_gameState.Player.Name}의 공격!\n");
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
        var enemies = _gameState.Spawner.GetSpawnedEnemies();
        foreach (var enemy in enemies)
        {
            if (_gameState.Player.IsDead())
                break;

            if (enemy.IsDead()) continue;

            Console.Clear();
            Console.WriteLine(new string('=', Utility.Width));
            Utility.AlignCenter("⚔️     BATTLE!!   ⚔️\n", Red);
            Console.WriteLine(new string('=', Utility.Width));
            Console.WriteLine($" {enemy.Name}의 반격!\n");

            for (int i = 0; i < 2; i++)
                Console.WriteLine(new string(' ', Utility.Width));
            enemy.PerformAttack(_gameState.Player);
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

    private void ShowPlayerInfo()
    {
        Console.WriteLine(new string('-', Utility.Width));
        var player = _gameState.Player;
        Console.WriteLine(" [ 내정보 ]");
        Utility.AlignLeft(" ", 4);
        Utility.AlignLeft($"Lv.{player.GetStats.Lv}", 7);
        Console.WriteLine($"{player.Name}");
        Utility.AlignLeft(" ❤️  HP : ", 12);
        Utility.AlignLeft($"{player.Health}", 3);
        Console.WriteLine($" / {player.GetStats.MaxHp}\n");
        Console.WriteLine(new string('-', Utility.Width));
        Utility.PrintLogs();
    }
}