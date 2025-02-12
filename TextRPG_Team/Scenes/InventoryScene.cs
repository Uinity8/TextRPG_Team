using TextRPG_Team.Objects;

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
    private string _strTitle = "";

    // 생성자 (DI 의존성 주입)
    public InventoryScene(GameState gameState, State state = State.Default)
    {
        _gameState = gameState;
        _state = state;
        switch (_state)
        {
            case State.Default:
                _strTitle = "보유 중인 아이템을 관리할 수 있습니다.\n";
                break;
            case State.Equip:
                _strTitle = "[ 장착관리 ]\n";
                break;
        }
    }

    // 인벤토리 씬 실행 메서드
    public void Run()
    {
        Console.Clear(); // 화면 초기화
        DisplayInventoryHeader(); // 헤더 화면 출력
        ShowScreen(); // 상태별 화면 출력
    }

    // 다음 씬 반환
    public IScene? GetNextScene()
    {
        return _state switch
        {
            State.Default => GetInputForDefault(),
            State.Equip => GetInputForEquip(),
            _ => null
        };
    }

    // 현재 상태에 따라 헤더 화면 출력
    private void DisplayInventoryHeader()
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("인벤토리\n", DarkCyan);
        Utility.AlignCenter(_strTitle);
        Console.WriteLine(new string('=', Utility.Width));
    }

    // 기본 상태에서 입력 처리
    private IScene? GetInputForDefault()
    {
        int input = Utility.GetInput(0, 2);
        return input switch
        {
            1 => new InventoryScene(_gameState, State.Equip), // 장착 관리 상태로 이동
            2 => new HealingPotionScene(_gameState), // 회복 포션 화면으로 이동
            0 => new MainScene(_gameState), // 메인 화면으로 복귀
            _ => null
        };
    }

    // 장착 관리 상태에서 입력 처리
    private IScene? GetInputForEquip()
    {
        int max = _gameState.Player.Inventory.Count;
        int input = Utility.GetInput(0, max, " 장착할 아이템을 선택하세요.");
        switch (input)
        {
            case 0:
                return new InventoryScene(_gameState); // 기본 상태로 복귀
            default:
                _gameState.Player.EquipItem(input - 1);
                return this;
        }
    }

    // 상태에 따라 UI 처리
    private void ShowScreen()
    {
        if (_gameState.Player.Inventory.Count == 0)
        {
            DisplayEmptyInventory();
        }
        else
        {
            switch (_state)
            {
                case State.Default:
                    DefaultScreen();
                    break;
                case State.Equip:
                    EquipScreen();
                    break;
            }
        }

        ShowFooterMenu();
        Utility.PrintLogs();
    }

    // 빈 인벤토리 화면 처리
    private void DisplayEmptyInventory()
    {
        for (int i = 0; i < 6; i++)
            Console.WriteLine(new string(' ', Utility.Width));
        Utility.AlignCenter("보유중인 아이템이 없습니다.\n");
        for (int i = 0; i < 5; i++)
            Console.WriteLine(new string(' ', Utility.Width));
    }

    // 기본 화면 출력
    private void DefaultScreen()
    {
        DisplayItemList(_gameState.Player.Inventory);
    }

    // 장착 관리 화면 출력
    private void EquipScreen()
    {
        DisplayItemList(_gameState.Player.Inventory, true);
    }

    // 아이템 리스트 출력
    //화면에 아이템 리스트 표시
    private void DisplayItemList(List<Item> itemList, bool isNumer = false)
    {
        int i = 1;
        foreach (var item in itemList)
        {
            Utility.AlignLeft(item.Icon, 7);
            string strNum = "";
            if (isNumer)
                strNum = i++.ToString() + ". ";
            Utility.ColorWrite(strNum, DarkMagenta);

            ConsoleColor color = White;
            if(item is EquipableItem equipItem && equipItem.itemEquip)
                color = Green; //아이템 장착착여부에 따라 초록/화이트
            Utility.AlignLeft($"{item.GetItemDisplay()}", Utility.Width - (15 + strNum.Length),color);
            Console.WriteLine();
            item.PrintInfo();
        }
    }

    // 화면 하단 메뉴 출력
    private void ShowFooterMenu()
    {
        Console.WriteLine(new string('-', Utility.Width));
        switch (_state)
        {
            case State.Default:
                Console.WriteLine(" 1. 장착관리");
                Console.WriteLine(" 2. 회복하기");
                Console.WriteLine(" 0. 나가기");
                break;
            case State.Equip:
                Console.WriteLine(" 0. 취소");
                break;
        }
        Console.WriteLine(new string('-', Utility.Width));
    }
}