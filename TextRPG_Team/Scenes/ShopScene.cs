using System;
using System.Numerics;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes;

public enum ShopState
{
    Normal,
    Buy,
    SaleI
}
public class ShopScene : IScene
{
    private readonly GameState _gameState;
    private ShopState _shopState = ShopState.Normal;


    public ShopScene(GameState gameState, ShopState shopState) //DI 의존성 주입
    {
        _gameState = gameState;
        _shopState = shopState;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기

        // 현재 씬에 대한 이름 출력
        Utility.ColorWrite("상점", ConsoleColor.Blue);
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine("{player.Gold}G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        foreach (var item in _gameState.itemList)
        {
            Console.WriteLine($"- {item.GetIteDisplay()} | {item.GetPricPurchase()}");
        }
        Console.WriteLine(); 
        Console.WriteLine("1. 구매하기");
        Console.WriteLine("2. 판매하기");
        Console.WriteLine("0. 나가기");
        if (_shopState == ShopState.Buy)
        {
            BuyScreen();
            _shopState = ShopState.Normal;
            Run();
        }
        else if (_shopState == ShopState.SaleI)
        {
            SaleScreen();
            _shopState = ShopState.Normal;
            Run();
        }
        
    }

    public void BuyScreen()
    {
        Console.Clear(); // 구매하기

        // 현재 씬에 대한 이름 출력
        Utility.ColorWriteLine("상점 - 구매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 구매할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine("{player.Gold}G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        int index = 1;
        foreach (var item in _gameState.itemList)
        {
            Console.WriteLine($"- {index++}. {item.GetIteDisplay()} | {item.GetPricPurchase()}");
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        int input = Utility.GetInput(0, index);
        switch (input)
        {
            case 0:
                break;

            default:
                Buyitem(input-1);
                BuyScreen();
                break;
        }
        new ShopScene(_gameState, ShopState.Normal);
    }

    public void Buyitem(int input)
    {
        // 아이템을 구매했는지?
        if (_gameState.itemList[input].itemPurchase)
        {
            Utility.ColorWriteLine("이미 구매한 아이템입니다.", ConsoleColor.Red);
            Console.ReadKey();
        }
        // 골드가 부족한지?
        // else if(_gameState.player.Gold < _gameState.itemList[input].Price) 
        // {
        //  Utility.ColorWriteLine("골드가 부족합니다.", ConsoleColor.Red);
        //  Console.ReadKey();
        //}
        else
        {
            _gameState.inventoryitemList.Add(_gameState.itemList[input]);
            _gameState.itemList[input].itemPurchase = true;
            //_gameState.player.Gold -= _gameState.itemList[input].Price;
        }

    }

    public void SaleScreen() // 판매하기
    {
        Console.Clear(); //처음 진입시 화면 지우기

        Utility.ColorWriteLine("상점 - 판매하기", ConsoleColor.Blue);
        Console.WriteLine("아이템을 판매할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine("{player.Gold}G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        int Index = 1;
        foreach (var item in _gameState.inventoryitemList)
        {
            Console.WriteLine($"- {Index++}. {item.GetIteDisplay()} | {item.GetPricPurchase()}");
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        int input = Utility.GetInput(0, Index);
        switch (input)
        {
            case 0:
                break;

            default:
                Saleitem(input - 1);
                BuyScreen();
                break;
        }
    }
    public void Saleitem(int input)
    {
        // 아이템을 장착중인가?
        if (_gameState.inventoryitemList[input].itemEquip)
        {
            
        }
        // 골드가 부족한지?
        // else if(_gameState.player.Gold < _gameState.itemList[input].price) 
        // {
        //  Utility.ColorWriteLine("골드가 부족합니다.", ConsoleColor.Red);
        //  Console.ReadKey();
        //}
        else
        {
            _gameState.itemList[input].itemPurchase = true;
        }

    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 2);
       switch(input)   //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            case 1 :
                return new ShopScene(_gameState, ShopState.Buy);
            case 2 :
                return new ShopScene(_gameState, ShopState.SaleI);
            default:
                return new MainScene(_gameState);


        }

    }
}