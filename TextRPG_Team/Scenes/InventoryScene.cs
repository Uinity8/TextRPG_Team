namespace TextRPG_Team.Scenes;

public class InventoryScene : IScene
{
    // 인벤토리 상태를 나타내는 Enum
    public enum State
    {
        Default, // 기본 상태
        Equip // 장착 관리 상태
    }

    private State _state; // 현재 상태
    private readonly GameState _gameState; // 게임 상태 공유

    // 생성자 (DI 의존성 주입)
    public InventoryScene(GameState gameState, State state = State.Default)
    {
        _gameState = gameState;
        _state = state;
    }

    // 인벤토리 씬 실행 메서드
    public void Run()
    {
        Console.Clear(); // 화면 초기화
        ShowScreen(); // 상태에 따라 화면 출력
    }

    // 현재 상태에 맞는 화면 표시
    private void ShowScreen()
    {
    }

    // 현재 상태에 따라 다음 씬 반환
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
}