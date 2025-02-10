using System.Diagnostics;

namespace TextRPG_Mockup.Scenes;

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

    private readonly string[] _enemies =
    {
        "Lv.2 미니언",
        "Lv.5 대포미니언",
        "Lv.3 공허충"
    };

    public BattleScene(GameState gameState, State state = State.Default)
    {
        _gameState = gameState;
        _state = state;
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
        int input = Utility.GetInput(0, 3);
        switch (input)
        {
            case 0:
                return new BattleScene(_gameState); // 취소 시 기본 상태로 복귀
            default:
                PlayerAttack(input - 1); // 특정 적 공격
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
        return new ResultScene(_gameState, ResultScene.State.Victory); // 실제론 GetNextScene에서 체크해야합니다.
        //return new BattleScene(_gameState); // BattleScene 초기 화면으로 이동
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
        foreach (var enemy in _enemies)
        {
            Console.WriteLine(enemy);
        }

        Console.WriteLine();

        // 플레이어 정보 표시
        Console.WriteLine("[내정보]");
        Console.WriteLine("Lv.1 Chad (전사)");
        Console.WriteLine("HP: 100/100\n");

        Console.WriteLine("1. 공격");
    }

    private void PlayerPhaseScreen()
    {
        Utility.ColorWriteLine("Battle!! - 플레이어 공격\n", ConsoleColor.Yellow);

        // 적 선택 목록 표시
        for (int i = 0; i < _enemies.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {_enemies[i]}");
        }

        Console.WriteLine();

        // 플레이어 정보 표시
        Console.WriteLine("[내정보]");
        Console.WriteLine("Lv.1 Chad (전사)");
        Console.WriteLine("HP: 100/100\n");

        Console.WriteLine("0. 취소");
    }

    private void PlayerAttack(int num)
    {
        // 플레이어 공격 로직 추가
        Utility.AddLog($"{_enemies[num]} 을(를) 맞췄습니다. [데미지 : 10]", ConsoleColor.Blue);
        Utility.AddLog($"{_enemies[num]} HP 10 -> Dead", ConsoleColor.Blue);
    }

    private void PlayerResultScreen()
    {
        Console.Clear();
        Utility.ColorWriteLine("Battle!! - 플레이어 공격\n", ConsoleColor.Yellow);

        Console.WriteLine("Chad 의 공격!\n");

        // 공격 결과 로그 출력
        Utility.PrintLogs();
        Console.WriteLine();

        // 플레이어 정보 표시
        Console.WriteLine("[내정보]");
        Console.WriteLine("Lv.1 Chad (전사)");
        Console.WriteLine("HP: 100/100\n");

        Console.WriteLine("0. 다음");
    }

    private void EnemyPhaseScreen()
    {
        for (int i = 0; i < 3; i++)
        {
            Console.Clear();
            Utility.ColorWriteLine("Battle!! - 적 Phase\n", ConsoleColor.Yellow);

            Console.WriteLine($"{_enemies[i]} + 공격합니다!(데미지: 5)\n");
            // 적 공격 로직 추가 가능

            Console.WriteLine("0. 다음");
            Utility.GetInput(0, 0); // 사용자 입력 대기
        }
    }
}