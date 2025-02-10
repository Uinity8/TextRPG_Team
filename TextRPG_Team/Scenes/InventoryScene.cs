namespace TextRPG_Team.Scenes;

using static ConsoleColor;

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
    private readonly string strTitle;

    // 생성자 (DI 의존성 주입)
    public InventoryScene(GameState gameState, State state = State.Default)
    {
        _gameState = gameState;
        _state = state;
        switch (_state)
        {
            case State.Default:
                strTitle = "보유 중인 아이템을 관리할 수 있습니다.\n";
                break;
            case State.Equip:
                strTitle = "[ 장착관리 ]\n";
                break;
        }
    }

    // 인벤토리 씬 실행 메서드
    public void Run()
    {
        Console.Clear(); // 화면 초기화
        ShowScreen(); // 상태에 따라 화면 출력
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
        int max = _gameState.Player.Inventory.Count;
        int input = Utility.GetInput(0, max, " 장착하실 아이템을 선택해주세요.");
        switch (input)
        {
            case 0:
                return new InventoryScene(_gameState); // 기본 상태로 복귀
            default:
                _gameState.Player.EquipItem(input - 1);
                return this;
        }

        ;
    }

    // 현재 상태에 맞는 화면 표시
    private void ShowScreen()
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("인벤토리\n", DarkCyan);
        Utility.AlignCenter(strTitle);
        Console.WriteLine(new string('=', Utility.Width));

        if (_gameState.Player.Inventory.Count == 0)
        {
            for (int i = 0; i < 6; i++)
                Console.WriteLine(new string(' ', Utility.Width));
            Utility.AlignCenter("보유중인 아이템이 없습니다.\n");
            for (int i = 0; i < 5; i++)
                Console.WriteLine(new string(' ', Utility.Width));
            //Console.WriteLine(new string('-', Utility.Width));
        }
        else
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
        switch (_state)
        {
            case State.Default:
                DefaultScreen(); // 기본 화면 출력
                Console.WriteLine(new string('-', Utility.Width));
                Console.WriteLine(" 1. 장착관리");
                Console.WriteLine(" 0. 나가기");
                Console.WriteLine(new string('-', Utility.Width));
                break;
            case State.Equip:
                EquipScreen(); // 장착 관리 화면 출력
                Console.WriteLine(new string('-', Utility.Width));
                Console.WriteLine(" 0. 취소");
                Console.WriteLine(new string('-', Utility.Width));
                break;
        }
        Utility.PrintLogs();
    }

    // 기본 상태 화면 출력
    private void DefaultScreen()
    {
        // 아이템 목록 표시
        int i = 1;
        foreach (var item in _gameState.Player.Inventory)
        {
            if (i >= _gameState.Player.Inventory.Count)
            {
                Console.WriteLine(new string(' ', Utility.Width));
                continue;
            }
            ConsoleColor color = White;
            if (item.itemEquip)
                color = DarkGreen;

            Utility.AlignLeft(item.Icon, 7);
            item.PrintNameAndEffect(color);
            if (item.itemEquip)
                Utility.ColorWrite("[E]", color);
            Console.WriteLine();
            item.PrintInfo();
        }

    }

    // 장착 관리 상태 화면 출력
    private void EquipScreen()
    {
        // 아이템 목록 표시
        int i = 1;
        foreach (var item in _gameState.Player.Inventory)
        {
            if (i >= _gameState.Player.Inventory.Count)
            {
                Console.WriteLine(new string(' ', Utility.Width));
                continue;
            }

            ConsoleColor color = White;
            if (item.itemEquip)
                color = DarkGreen;

            Utility.AlignLeft(item.Icon, 7);
            Utility.ColorWrite($"{(i++)}. ", DarkMagenta);
            item.PrintNameAndEffect(color);
            if (item.itemEquip)
                Utility.ColorWrite("[E]", color);
            Console.WriteLine();
            item.PrintInfo();
        }
        
    }
}