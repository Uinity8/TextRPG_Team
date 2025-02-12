using TextRPG_Team.Objects.Items;
using TextRPG_Team.Objects.Items.Consumable;
using TextRPG_Team.Objects.Items.Equipable;

namespace TextRPG_Team.Scenes;

using static ConsoleColor;

public class ShopScene : IScene
{
    public enum State
    {
        Default,
        Buy,
        Sell,
    }

    readonly State _state;
    private readonly GameState _gameState;
    private readonly string _strTitle = "";
    
    //page 관련 필드 추가
    private readonly int _maxPage;
    private int _page;// 현재 페이지 번호
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
    private readonly List<Item> _allItems = new List<Item>();


    public ShopScene(GameState gameState,int page = 0, State state = State.Default) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state;
        switch (_state)
        {
            case State.Default:
                _allItems = _gameState.ItemList;
                _strTitle = "필요한 아이템을 얻을 수 있는 상점입니다.\n";
                break;
            case State.Buy:
                _allItems = _gameState.ItemList;
                _strTitle = "[ 구매하기 ]\n";
                break;
            case State.Sell:
                _allItems = _gameState.Player.Inventory;
                _strTitle = "[ 판매하기 ]\n";
                break;
        }
        _maxPage = (_allItems.Count/ ItemsPerPage);
        _page = page;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
    }
    
    private void ShowScreen()
    {
        DisplayShopHeader(_strTitle, _gameState.Player.Gold);

        switch (_state)
        {
            case State.Default:
                DefaultScreen();
                break;
            case State.Buy:
                BuyScreen();
                break;
            case State.Sell:
                SellScreen();
                break;
        }
        Utility.PrintLogs();
        ShowFooterMenu(); // 하단 메뉴 출력
    }
    
    
    //하단 메뉴
    private void ShowFooterMenu()
    {
        Console.WriteLine(new string('-', Utility.Width));
        switch (_state)
        {
            case State.Default:
                Utility.AlignLeft(" 1. 아이템 구매", 16);
                Utility.AlignLeft(" 2. 아이템 판매", 16);
                Utility.AlignLeft(" 0. 나가기", 16);
                Console.WriteLine();
                Utility.AlignLeft(" 3. 이전페이지", 16);
                Utility.AlignLeft(" 4. 다음 페이지\n", 16);
                break;
            case State.Buy:
                Console.WriteLine(" 0. 취소");
                break;
            case State.Sell:
                Console.WriteLine(" 0. 취소");
                break;
        }
        Console.WriteLine(new string('-', Utility.Width));
    }


    public IScene? GetNextScene()
    {
        return _state switch
        {
            State.Default => GetInputForDefault(),
            State.Buy => GetInputForBuy(),
            State.Sell => GetInputForSell(),
            _ => null
        };
    }

    private IScene? GetInputForDefault() // 기본상태
    {
        int input = Utility.GetInput(0, 4);
        switch (input)
        {
            case 1:
                return new ShopScene(_gameState, Page,State.Buy);
            case 2:
                return new ShopScene(_gameState, 0,State.Sell);
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

    private IScene GetInputForBuy() // 구매하기
    {
        var pagedItems = GetPagedItemList(_allItems);
        int input = Utility.GetInput(0, pagedItems.Count, " 구매하실 아이템을 선택해주세요.");
        switch (input)
        {
            case 0:
                return new ShopScene(_gameState,Page);
            default:
                Item item =pagedItems[input-1];
                _gameState.Player.BuyItem(item.ShallowCopy()) ;
                return this;
        }
    }

    private IScene GetInputForSell() // 판매하기
    {
        var pagedItems = GetPagedItemList(_allItems);
        int input = Utility.GetInput(0, pagedItems.Count," 판매하실 아이템을 선택해주세요.");
        switch (input)
        {
            case 0:
                return new ShopScene(_gameState);
            default:
                Item item =pagedItems[input-1];
                _gameState.Player.SellItem(item);
                return this;
        }
    }

    private void DefaultScreen() //기본화면
    {
        DisplayItemList(_allItems);
    }

    private void BuyScreen() //구매하기
    {
        DisplayItemList(_allItems, true);
    }

    
    private void SellScreen() //판매하기
    {
        //플레이어 인벤토리가 비었으면
        if (_allItems.Count == 0)
            Utility.AlignCenter("보유중인 아이템이 없습니다.\n");
        else
            DisplayItemList(_allItems, true, true);
    }
    
    
    //=============헬프 메서드===============
    //헤더표시
    private void DisplayShopHeader(string title, int playerGold)
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("상점\n", DarkCyan);
        Utility.AlignCenter(title); // 타이틀 동적으로 입력받기
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignRight("[ 보유 골드 ]\n", Utility.Width);
        Console.WriteLine(" [ 아이템 목록 ]");
    
        Utility.AlignRight($"💰", Utility.Width - 11);
        Utility.AlignRight($"{playerGold}", 7);
        Utility.ColorWriteLine(" G", Yellow);
        
        Console.WriteLine($" [ Page {Page+1} / {_maxPage+1} ]");
        Console.WriteLine(new string('-', Utility.Width));
    }

    
    //화면에 아이템 리스트 표시
    private void DisplayItemList(List<Item> allItems, bool isNumber = false, bool isSell = false)
    {

        var pagedItems = GetPagedItemList(allItems);

        for (int i = 0; i<pagedItems.Count; i++)
        {
            Console.Write(pagedItems[i].Icon);
            string strNum = "";
            if (isNumber)
                strNum = (i+1).ToString() + ". ";
            Utility.ColorWrite(strNum, DarkMagenta);
            
            string strItem = pagedItems[i].GetItemDisplay();
            // 플레이어가 소비 아이템을 보유 중이라면 "보유 개수" 표시 추가
            if (IsPlayerHaveItem(pagedItems[i].Id) && pagedItems[i] is ConsumableItem consumItem)
            {
                strItem += $"  (보유 개수 {consumItem.Count})"; 
            }

            Utility.AlignLeft(strItem, Utility.Width - (15 + strNum.Length));
            DisplayPrice(pagedItems[i], isSell);
            pagedItems[i].PrintInfo();
        }
    }

    //가격 표시(가격,구매완료)
    private void DisplayPrice(Item item, bool isSell)
    {
        if (IsPlayerHaveItem(item.Id) && !isSell && item is EquipableItem)  //플레이어가 해당아이템을 보유중이라면 "구매완료" 표시
        {
            Utility.AlignRight($"보유중\n", 5); // 가격 정렬
        }
        else //플레이어가 가진 골드에 따라 색상출력 (화이트,레드)
        {
            ConsoleColor color = _gameState.Player.Gold >= item.Price ? White : Red;

            int price = isSell ? item.SellPrice: item.Price;
            Utility.AlignRight($"{price}", 5,color); // 가격 정렬
            Utility.ColorWriteLine("G", ConsoleColor.Yellow); // "G"을 출력
        }
    }

    //플레이어가 소유중인지 확인메서드
    private bool IsPlayerHaveItem(int itemId)
    {
        return _gameState.Player.Inventory.Find(x => x.Id == itemId) != null;
    }
    
    private List<Item> GetPagedItemList(List<Item> allItems)
    {
        return allItems
            .Skip(_page * ItemsPerPage) // 현재 페이지에 해당하는 첫 항목을 건너뜀
            .Take(ItemsPerPage)        // 현재 페이지에서 표시할 항목 수만큼 선택
            .ToList();
    }
}