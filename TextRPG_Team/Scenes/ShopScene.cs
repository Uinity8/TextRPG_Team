using System.Diagnostics;

namespace TextRPG_Team.Scenes;
using System.Diagnostics;
using System.Xml;

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

    public ShopScene(GameState gameState, State state = State.Default) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
        Console.WriteLine("0. 나가기\n");
        Utility.PrintLogs();
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
        int input = Utility.GetInput(0, _gameState._itemList.Count);
        switch (input)
        {
            case 0:
                return new ShopScene(_gameState);
            default:
                Item item = _gameState._itemList[input - 1];
                //item.itemPurchase = _gameState.Player.TryBuy(item);
                return this; 
        }
        
    }
    
    private IScene? GetInputForSell() // 판매하기
    {
        int input = Utility.GetInput(0, _gameState.Player.Inventory.Count);
        switch (input)
        {
            case 0:
                return new ShopScene(_gameState);
            default:
                Item item = _gameState._itemList[input - 1];
                //item.itemPurchase = _gameState.Player.TrySell(item);
                return this;
        }

    }


    private void ShowScreen()
    {
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
        Utility.ColorWriteLine("상점", ConsoleColor.Blue);
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_gameState.Player.Gold}G\n");
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _gameState._itemList)
        {
            Console.WriteLine($"- {item.GetItemDisplay()} | {item.GetPricPurchase()}");
        }
        Console.WriteLine("\n1. 구매하기");
        Console.WriteLine("2. 판매하기");
    }

    private void BuyScreen() //구매하기
    {
        Utility.ColorWriteLine("상점 - 구매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 구매할 수 있습니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_gameState.Player.Gold}G\n");
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _gameState._itemList)
        {
            Console.WriteLine($"- {item.Id}. {item.GetItemDisplay()} | {item.GetPricPurchase()}");
        }
    }
    
    private void SellScreen() //판매하기
    {
        Utility.ColorWriteLine("상점 - 판매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 판매할 수 있습니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_gameState.Player.Gold}G\n");
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _gameState.Player.Inventory)
        {
            Console.WriteLine($"- {item.Id}. {item.GetItemDisplay()} | {item.Price * 0.85}");
        }
    }
}