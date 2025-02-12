namespace TextRPG_Team.Scenes;

using Objects;
using static ConsoleColor;

public class ShopScene : IScene
{
    public enum State
    {
        Default,
        Buy,
        Sell,
    }

    State _state;
    private readonly GameState _gameState;

    private string _strTitle = "";
    
    public ShopScene(GameState gameState, State state = State.Default) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state;
        switch (_state)
        {
            case State.Default:
                _strTitle = "필요한 아이템을 얻을 수 있는 상점입니다.\n";
                break;
            case State.Buy:
                _strTitle = "[ 구매하기 ]\n";
                break;
            case State.Sell:
                _strTitle = "[ 판매하기 ]\n";
                break;
        }
        
        _gameState.Player.Gold = 10000;
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
                Console.WriteLine(" 1. 아이템 구매");
                Console.WriteLine(" 2. 아이템 판매");
                Console.WriteLine(" 0. 나가기");
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
        int input = Utility.GetInput(0, 2);
        return input switch
        {
            1 => new ShopScene(_gameState, State.Buy),
            2 => new ShopScene(_gameState, State.Sell),
            0 => new MainScene(_gameState),
            _ => null
        };
    }

    private IScene? GetInputForBuy() // 구매하기
    {
        int input = Utility.GetInput(0, _gameState._itemList.Count, " 구매하실 아이템을 선택해주세요.");
        switch (input)
        {
            case 0:
                return new ShopScene(_gameState);
            default:
                Item item = _gameState._itemList[input - 1];
                _gameState.Player.BuyItem(item) ;// item.itemPurchase = true;
                return this;
        }
    }

    private IScene? GetInputForSell() // 판매하기
    {
        int input = Utility.GetInput(0, _gameState.Player.Inventory.Count, " 판매하실 아이템을 선택해주세요.");
        switch (input)
        {
            case 0:
                return new ShopScene(_gameState);
            default:
                Item item = _gameState.Player.Inventory[input - 1];
                _gameState.Player.SellItem(item);
                return this;
        }
    }

    private void DefaultScreen() //기본화면
    {
        DisplayItemList(_gameState._itemList);
    }

    private void BuyScreen() //구매하기
    {
        DisplayItemList(_gameState._itemList, true);
    }

    
    private void SellScreen() //판매하기
    {
        //플레이어 인벤토리가 비었으면
        if (_gameState.Player.Inventory.Count == 0)
            Utility.AlignCenter("보유중인 아이템이 없습니다.\n");
        else
            DisplayItemList(_gameState.Player.Inventory, true, true);
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
        Console.WriteLine(new string('-', Utility.Width));
    }

    
    //화면에 아이템 리스트 표시
    private void DisplayItemList(List<Item> itemList, bool isNumer = false, bool isSell = false)
    {
        int i = 1;
        foreach (var item in itemList)
        {
            Console.Write(item.Icon);
            string strNum = "";
            if (isNumer)
                strNum = i++.ToString() + ". ";
            Utility.ColorWrite(strNum, DarkMagenta);
            Utility.AlignLeft($"{item.GetItemDisplay()} ", Utility.Width - (15 + strNum.Length));
            DisplayPrice(item, isSell);
            item.PrintInfo();
        }
    }

    //가격 표시(가격,구매완료)
    private void DisplayPrice(Item item, bool isSell)
    {
        if (IsPlayerHaveItem(item.Id) && !isSell)  //플레이어가 해당아이템을 보유중이라면 "구매완료" 표시
        {
            Utility.AlignRight($"구매완료\n", 5); // 가격 정렬
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
    
}