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
        ShowScreen();
        Utility.PrintLogs();
    }

    private void ShowScreen()
    {
        switch (_shopState)
        {
            case ShopState.Buy:
                Utility.ColorWriteLine("상점 - 구매하기", ConsoleColor.Blue);
                Console.WriteLine("아이템을 구매할 수 있습니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{_gameState.GetPlayer().Gold} G\n");
                Console.WriteLine("[아이템 목록]");
                int ShopIndex = 1;
                foreach (var item in _gameState.itemList)
                {
                    Console.WriteLine($"- {ShopIndex++}. {item.GetIteDisplay()} | {item.GetPricPurchase()}");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                break;

            case ShopState.SaleI:
                Utility.ColorWriteLine("상점 - 판매하기", ConsoleColor.Blue);
                Console.WriteLine("아이템을 판매할 수 있습니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{_gameState.GetPlayer().Gold} G\n");
                Console.WriteLine("[아이템 목록]");
                int IvtIndex = 1;
                foreach (var item in _gameState.inventoryitemList)
                {
                    Console.WriteLine($"- {IvtIndex++}. {item.GetIteDisplay()} | {item.GetPricPurchase()}");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                break;

            case ShopState.Normal:
                Utility.ColorWrite("상점", ConsoleColor.Blue);
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{}G");
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
                break;
        }
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 2);
        switch (input)   //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            case 0:
                if (_shopState == ShopState.Normal) return new MainScene(_gameState);
                else return new ShopScene(_gameState, ShopState.Normal);
            case 1:
                int Buyinput = Utility.GetInput(0, (_gameState.itemList.Count));
                // 아이템을 구매했는지?
                if (_gameState.itemList[input].itemPurchase)
                    Utility.AddLog("이미 구매한 아이템입니다.", ConsoleColor.Red);
                else
                {
                    _gameState.GetPlayer().Gold -= _gameState.itemList[Buyinput - 1].Price;
                    _gameState.inventoryitemList.Add(_gameState.itemList[Buyinput - 1]);
                    _gameState.itemList[Buyinput - 1].itemPurchase = true;
                    return new ShopScene(_gameState, ShopState.Buy);
                }
                if (_shopState == ShopState.Normal) return new MainScene(_gameState);
                else return new ShopScene(_gameState, ShopState.Normal);

                
            case 2:
                // 아이템을 장착중인가?
                int SaleIinput = Utility.GetInput(0, _gameState.inventoryitemList.Count);
                if (_gameState.inventoryitemList[input].itemEquip)
                {
                    if (_gameState.inventoryitemList[SaleIinput].Type == ItemType.Weapon) 
                    {
                        //_gameState.GetPlayer().AddStats = _gameState.inventoryitemList[SaleIinput].Value;
                        
                    }
                    else
                    {

                    }
                }
                else
                {

                }
                _gameState.itemList[input].itemPurchase = false;
                return new ShopScene(_gameState, ShopState.SaleI);
            default:
                return new MainScene(_gameState);
        }

    }
}