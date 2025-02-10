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
    private int Input;
    private readonly GameState _gameState;

    public ShopScene(GameState gameState, int input = 0, State state = State.Default) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state;
        Input = input;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
        Console.WriteLine("0. 나가기\n");
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

    private IScene? GetInputForDefault()
    {
        int input = Utility.GetInput(0, 2);
        return input switch
        {
            1 => new ShopScene(_gameState, input, State.Buy),
            2 => new ShopScene(_gameState, input, State.Sell),
            0 => new MainScene(_gameState),
            _ => null
        };
    }

    private IScene? GetInputForBuy()
    {
        int input = Utility.GetInput(0, _gameState._itemList.Count);
        return input switch
        {
            0 => new ShopScene(_gameState, input),
            _ => new ShopScene(_gameState, input, State.Buy)
        };
        
    }
    
    private IScene? GetInputForSell()
    {
        int input = Utility.GetInput(0, _gameState._player._inventory.Count);
        return input switch
        {
            0 => new ShopScene(_gameState, input),
            _ => new ShopScene(_gameState, input, State.Sell)
        };
        
    }


    private void Action(int input)
    {
        if(_state == State.Buy) _gameState._player.BuyItem(_gameState._itemList[input-1]);
        if(_state == State.Sell) _gameState._player.SellItem(_gameState._player._inventory[input-1]);
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
                Action(Input);
                break;
            case State.Sell:
                SellScreen();
                Action(Input);
                break;
        }
    }

    private void DefaultScreen() //기본화면
    {
        Utility.ColorWriteLine("상점", ConsoleColor.Blue);
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_gameState._player.Gold}G\n");
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _gameState._itemList)
        {
            Console.WriteLine($"- {item.GetIteDisplay()} | {item.GetPricPurchase()}");
        }
        Console.WriteLine("\n1. 구매하기");
        Console.WriteLine("2. 판매하기");
    }

    private void BuyScreen() //구매하기
    {
        Utility.ColorWriteLine("상점 - 구매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 구매할 수 있습니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_gameState._player.Gold}G\n");
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _gameState._itemList)
        {
            Console.WriteLine($"- {item.Id}. {item.GetIteDisplay()} | {item.GetPricPurchase()}");
        }
    }
    
    private void SellScreen() //판매하기
    {
        Utility.ColorWriteLine("상점 - 판매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 판매할 수 있습니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{_gameState._player.Gold}G\n");
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _gameState._player._inventory)
        {
            Console.WriteLine($"- {item.Id}. {item.GetIteDisplay()} | {item.GetPricPurchase()}");
        }
    }
}