using System.Diagnostics;

namespace TextRPG_Team.Scenes;
using System.Diagnostics;

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
            1 => new ShopScene(_gameState, State.Buy),
            2 => new ShopScene(_gameState, State.Sell),
            0 => new MainScene(_gameState),
            _ => null
        };
    }

    private IScene? GetInputForBuy()
    {
        int input = Utility.GetInput(0, 2);
        return input switch
        {
            0 => new ShopScene(_gameState),
            _ => this
        };
        
    }
    
    private IScene? GetInputForSell()
    {
        int input = Utility.GetInput(0, 2);
        return input switch
        {
            0 => new ShopScene(_gameState),
            _ => this
        };
        
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

    private void DefaultScreen()
    {
        Utility.ColorWriteLine("상점", ConsoleColor.Blue);
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine("1000G\n");
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine("-아이템 | 설명 | 효과 | 가격");
        Console.WriteLine("-아이템 | 설명 | 효과 | 가격");
        Console.WriteLine();
        Console.WriteLine("1. 구매하기");
        Console.WriteLine("2. 판매하기");
        Console.WriteLine();
        Console.WriteLine("0. 나가기\n");
    }

    private void BuyScreen()
    {
        Utility.ColorWriteLine("상점 - 구매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 구매할 수 있습니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine("1000G\n");
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine("1. 아이템 | 설명 | 효과 | 가격");
        Console.WriteLine("2. 아이템 | 설명 | 효과 | 보유중");
        Console.WriteLine();
        Console.WriteLine("0. 나가기\n");
    }
    
    private void SellScreen()
    {
        Utility.ColorWriteLine("상점 - 판매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 판매할 수 있습니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine("1000G\n");
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine("1. 아이템 | 설명 | 효과 | 판매가격");
        Console.WriteLine("2. 아이템 | 설명 | 효과 | 판매가격");
        Console.WriteLine();
        Console.WriteLine("0. 나가기\n");
    }
}