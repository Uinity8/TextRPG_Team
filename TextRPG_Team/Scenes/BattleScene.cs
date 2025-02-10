using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes;

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
        
        //플레이어,적 어택 액션에 서로의 TakeDamage등록
        var enemies = _gameState.Spawner.GetSpawnedEnemies();
    }

    public void Run()
    {
        Console.Clear(); // 화면 초기화
        ShowScreen(); // 현재 상태에 맞는 화면 출력
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
        int input = Utility.GetInput(0, max);
        switch (input)
        {
            case 0:
                return new BattleScene(_gameState); // 취소 시 기본 상태로 복귀
            default:
                var enemy = _gameState.Spawner.GetSpawnedEnemies()[input-1];
                if (enemy.IsDead())
                {
                    Utility.AddLog("이미 뒤졌는데요", ConsoleColor.Red);
                    return this;
                }
                
                _gameState.Player.PerformAttack(enemy); // 특정 적 공격
                return new BattleScene(_gameState, State.PlayerResult); // 결과 화면으로 이동
        }
    }

    private IScene? GetInputForPlayerResult()
    {
        int input = Utility.GetInput(0, 0);
        return input switch
        {
            0 => new BattleScene(_gameState, State.EnemyPhase), // 적의 턴으로 이동
            _ => null
        };
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
        Utility.ColorWriteLine("Battle!!\n", ConsoleColor.Yellow);

        // 적 정보 표시
        var enemies = _gameState.Spawner.GetSpawnedEnemies();
        foreach (var enemy in enemies)
        {
            if(enemy.IsDead())
                Utility.ColorWriteLine(enemy.ToString(), ConsoleColor.DarkGray);
            else
                Console.WriteLine(enemy.ToString());
        }

        Console.WriteLine();

        // 플레이어 정보 표시
        ShowPlayerInfo();


        Console.WriteLine("1. 공격");
    }

    private void PlayerPhaseScreen()
    {
        Utility.ColorWriteLine("Battle!! - 플레이어 공격\n", ConsoleColor.Yellow);

        // 적 선택 목록 표시
        var enemies = _gameState.Spawner.GetSpawnedEnemies();
        int i = 1;
        foreach (var enemy in enemies)
        {
            if(enemy.IsDead())
                Utility.ColorWriteLine($"{i}. {enemy}", ConsoleColor.DarkGray);
            else
                Console.WriteLine($"{i}. {enemy}");
            i++;
        }

        Console.WriteLine();

        // 플레이어 정보 표시
        ShowPlayerInfo();
        Console.WriteLine("0. 취소");
        Utility.PrintLogs();
    }

    private void PlayerResultScreen()
    {
        Console.Clear();
        Utility.ColorWriteLine("Battle!! - 플레이어 공격\n", ConsoleColor.Yellow);

        Console.WriteLine($"{_gameState.Player.Name}의 공격!\n");
        Utility.PrintLogs();

        // 공격 결과 로그 출력
        Utility.PrintLogs();
        Console.WriteLine();

        // 플레이어 정보 표시
        ShowPlayerInfo();

        Console.WriteLine("0. 다음");
    }

    private void EnemyPhaseScreen()
    {
        var enemies = _gameState.Spawner.GetSpawnedEnemies();
        foreach (var enemy in enemies)
        {
            if(enemy.IsDead()) continue;
            
            Console.Clear();
            Utility.ColorWriteLine("Battle!! - 적 Phase\n", ConsoleColor.Yellow);

            enemy.PerformAttack(_gameState.Player);
            Utility.PrintLogs();

            Console.WriteLine("0. 다음");
            Utility.GetInput(0, 0); // 사용자 입력 대기
        }
    }

    private void ShowPlayerInfo()
    {
        var player = _gameState.Player;
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.GetStats.Lv}: {player.Name}");
        Console.WriteLine($"{player.Health}/{player.GetStats.MaxHp}\n");
    }
    
}