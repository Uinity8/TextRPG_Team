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

    private string _strTitle ="";

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
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
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
                if(_gameState.Player.BuyItem(item)) item.itemPurchase = true;
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
                Item item = _gameState._itemList[input - 1];
                item.itemPurchase = _gameState.Player.TrySell(item);
                return this;
        }
    }


    private void ShowScreen()
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("상점\n", DarkCyan);
        Utility.AlignCenter(_strTitle);
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignRight("[ 보유 골드 ]\n", Utility.Width);
        Console.WriteLine(" [ 아이템 목록 ]");

        Utility.AlignRight($"💰", Utility.Width - 11);
        Utility.AlignRight($"{_gameState.Player.Gold}", 7);
        Utility.ColorWriteLine(" G", Yellow);
        Console.WriteLine(new string('-', Utility.Width));

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
    }

    private void DefaultScreen() //기본화면
    {
        foreach (var item in _gameState._itemList)
        {
            Utility.AlignLeft(item.Icon, 7);
            ConsoleColor color = item.Price > _gameState.Player.Gold ? Red : White;
            item.PrintNameAndEffect(color);
            item.PrintPriceForBuy(color);
            item.PrintInfo();
        }
        Utility.PrintLogs();
        Console.Write(" 1. 구매하기");
        Console.Write(" 2. 판매하기");
        Console.WriteLine(" 0. 나가기");
    }

    private void BuyScreen() //구매하기
    {
        int i = 1;
        foreach (var item in _gameState._itemList)
        {
            Utility.AlignLeft(item.Icon, 7);
            Utility.ColorWrite($"{(i++)}. ", DarkMagenta);
            ConsoleColor color = item.Price > _gameState.Player.Gold ? Red : White;
            item.PrintNameAndEffect(color);
            item.PrintPriceForBuy(color);
            item.PrintInfo();
        }
        Utility.PrintLogs();
        Console.WriteLine(" 0. 취소");
    }

    private void SellScreen() //판매하기
    {
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
            var invetory = _gameState.Player.Inventory;
            for (int i = 0; i < 10; i++)
            {
                if (i >= invetory.Count)
                {
                    Console.WriteLine(new string(' ', Utility.Width));
                    continue;
                }

                Utility.AlignLeft(invetory[i].Icon, 7);
                Utility.ColorWrite($"{(i+1)}. ", DarkMagenta);
                invetory[i].PrintNameAndEffect(White);
                invetory[i].PrintPriceForSell();
                invetory[i].PrintInfo();
            }
        }
        Utility.PrintLogs();
        Console.WriteLine(" 0. 취소");
    }
}