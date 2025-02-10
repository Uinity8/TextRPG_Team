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
        Console.WriteLine("0. 나가기\n");
    }

    // 현재 상태에 따라 다음 씬 반환
    public IScene? GetNextScene()
    {
        return _state switch
        {
            State.Default => GetInputForDefault(),
            State.Equip => GetInputForEquip(),
            _ => null
        };
    }

    // 기본 상태에서 입력 처리
    private IScene? GetInputForDefault()
    {
        int input = Utility.GetInput(0, 1); // 사용자 입력 받음
        return input switch
        {
            1 => new InventoryScene(_gameState, State.Equip), // 장착 관리 상태로 이동
            0 => new MainScene(_gameState), // 메인 화면으로 복귀
            _ => null
        };
    }

    // 장착 관리 상태에서 입력 처리
    private IScene? GetInputForEquip()
    {
        int input = Utility.GetInput(0, 3);
        return input switch
        {
            0 => new InventoryScene(_gameState), // 기본 상태로 복귀
            _ => this
        };
    }

    // 현재 상태에 맞는 화면 표시
    private void ShowScreen()
    {
        switch (_state)
        {
            case State.Default:
                DefaultScreen(); // 기본 화면 출력
                break;
            case State.Equip:
                EquipScreen(); // 장착 관리 화면 출력
                break;
        }
    }

    // 기본 상태 화면 출력
    private void DefaultScreen()
    {
        Utility.ColorWriteLine("인벤토리", ConsoleColor.Yellow);
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

        // 아이템 목록 표시
        Console.WriteLine("- [E]스파르타의 창 | 공격력 +7 | 스파르타의 전사들이 사용했다는 전설의 창입니다.");
        Console.WriteLine("- [E]스파르타의 창 | 공격력 +7 | 스파르타의 전사들이 사용했다는 전설의 창입니다.");
        Console.WriteLine("- 낡은 검          | 공격력 +2 | 쉽게 볼 수 있는 낡은 검 입니다.");
        Console.WriteLine();
        Console.WriteLine("1. 장착관리");
    }

    // 장착 관리 상태 화면 출력
    private void EquipScreen()
    {
        Utility.ColorWriteLine("인벤토리 - 장착관리", ConsoleColor.Yellow);
        Console.WriteLine("장착하실 아이템을 선택해 주세요.\n");

        // 장착 가능한 아이템 목록 표시
        foreach (var item in _gameState._player._inventory)
        {
            Console.WriteLine($"{_gameState._player._inventory}");
        }
        Console.WriteLine();
    }
}