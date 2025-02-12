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

    public enum InvType
    {
        Equip,
        Consume,
        End,
    }


    private State _state; // 현재 상태
    private InvType _invType;
    private readonly GameState _gameState; // 게임 상태 공유
    private string _strTitle = "";
    
    //page 관련 필드 추가
    private int _maxPage = 0;
    private int _page = 0;// 현재 페이지 번호
    private const int ItemsPerPage = 5; // 페이지당 표시할 항목 수
    int Page
    {
        get => _page;
        set
        {
            if (value < 0)
                _page = 0;
            else if (value > _maxPage)
                _page = _maxPage;
            else
                _page = value;
        }
    }

    // 생성자 (DI 의존성 주입)
    public InventoryScene(GameState gameState, State state = State.Default, InvType invType = InvType.Equip)
    {
        _gameState = gameState;
        _state = state;
        _invType = invType;
       // _maxPage = (int)Math.Ceiling(FilteredItemList(_gameState.Player.Inventory).Count / 6.0);
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
        Console.WriteLine($" [ Page {Page+1} / {_maxPage+1} ]");
    }

    // 기본 상태에서 입력 처리
    private IScene? GetInputForDefault()
    {
        int input = Utility.GetInput(0, 4);
        switch (input)
        {
            case 1:
                return new InventoryScene(_gameState, State.Equip, _invType); // 장착 관리 상태로 이동
            case 2:
                _invType++;
                if (_invType >= InvType.End)
                    _invType = InvType.Equip;
                return this;
            case 3:
                Page--;
                return this;
            case 4:
                Page++;
                return this;
            case 0:
                return new MainScene(_gameState);
        }

        return null;
    }

    // 장착 관리 상태에서 입력 처리
    private IScene? GetInputForEquip()
    {      
        var itemList = FilteredItemList(_gameState.Player.Inventory);
        var pagedItems = GetPagedItemList(_gameState.Player.Inventory);
        int input = Utility.GetInput(0, pagedItems.Count," 장착할 아이템을 선택하세요.");
        switch (input)
        {
            case 0:
                return new InventoryScene(_gameState); // 기본 상태로 복귀
            default:
                Item item =pagedItems[input-1];
                    _gameState.Player.UseItem(item);
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
    private void DisplayItemList(List<Item> allItems, bool isNumer = false)
    {
        var itemList = FilteredItemList(allItems);
        var pagedItems = GetPagedItemList(itemList);
        
        int i = 1;
        foreach (var item in pagedItems)
        {
            Console.Write(item.Icon);
            string strNum = "";
            if (isNumer)
                strNum = i++.ToString() + ". ";
            Utility.ColorWrite(strNum, DarkMagenta);

            ConsoleColor color = White;
            if (item is EquipableItem equipItem && equipItem.itemEquip)
                color = Green; //아이템 장착착여부에 따라 초록/화이트
            Utility.AlignLeft($"{item.GetItemDisplay()}", Utility.Width - (15 + strNum.Length), color);
            
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

                if (_invType == InvType.Equip)
                {
                     Utility.AlignLeft(" 1. 장착관리", 16);
                     Utility.AlignLeft(" 2. 소비아이템", 16);
                }
                else if (_invType == InvType.Consume)
                {
                     Utility.AlignLeft(" 1. 아이템사용", 16);
                     Utility.AlignLeft(" 2. 장비아이템", 16);
                }

                Utility.AlignLeft(" 0. 나가기", 16);
                Console.WriteLine();
                Utility.AlignLeft(" 3. 이전페이지", 16);
                Utility.AlignLeft(" 4. 다음 페이지\n", 16);
                break;
            case State.Equip:
                Console.WriteLine(" 0. 취소");
                break;
        }

        Console.WriteLine(new string('-', Utility.Width));
    }

    // 현재 _invType에 맞는 아이템 리스트 반환
    private List<Item> FilteredItemList(List<Item> allItems)
    {
        return _invType switch
        {
            InvType.Equip => allItems.OfType<EquipableItem>().Cast<Item>().ToList(),
            InvType.Consume => allItems.OfType<ConsumableItem>().Cast<Item>().ToList(),
            _ => new List<Item>() // 기본 빈 리스트
        };
    }
    
    //현재페이지 맞는 아이템리스트
    private List<Item> GetPagedItemList(List<Item> allItems)
    {
        return allItems
            .Skip(_page * ItemsPerPage) // 현재 페이지에 해당하는 첫 항목을 건너뜀
            .Take(ItemsPerPage)        // 현재 페이지에서 표시할 항목 수만큼 선택
            .ToList();
    }

}