using System.Diagnostics;

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
    }

    public void Run()
    {
        Console.Clear(); // 화면 초기화
        ShowScreen(); // 현재 상태에 맞는 화면 출력
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch     
        {
            1 => this,
            2 => new MainScene(_gameState), // MainScene()
            _ => null // 잘못된 입력 시 종료
        };
    }

    private void ShowScreen()
    {
    }
}
