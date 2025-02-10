using System.Diagnostics;

namespace TextRPG_Team.Scenes;

public class ShopScene : IScene
{
    public enum State
    {
        Default,    //기본   
        Buy,        //구매
        Sell,       //판매
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

    // 현재 상태에 맞는 화면 표시
    private void ShowScreen()
    {
        switch (_state)
        {
            case State.Default:
                Utility.ColorWrite("상점", ConsoleColor.Blue);
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine("1000G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine("-아이템 | 설명 | 효과 | 가격");
                Console.WriteLine();
                Console.WriteLine("1. 구매하기");
                Console.WriteLine("2. 판매하기");
                Console.WriteLine("0. 나가기");
                break;

            case State.Buy:
                Utility.ColorWriteLine("상점 - 구매하기", ConsoleColor.Blue);
                Console.WriteLine("아이템을 구매할 수 있습니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine("1000G");
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine("1. 아이템 | 설명 | 효과 | 가격");
                Console.WriteLine("2. 아이템 | 설명 | 효과 | 보유중");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                break;

            case State.Sell:
                Utility.ColorWriteLine("상점 - 판매하기", ConsoleColor.Blue);
                Console.WriteLine("아이템을 판매할 수 있습니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine("1000G");
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine("1. 아이템 | 설명 | 효과 | 판매가격");
                Console.WriteLine("2. 아이템 | 설명 | 효과 | 판매가격");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                break;

        }
    }

    // 현재 상태에 따라 다음 씬 반환
    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(0, 2);
        switch(input) 
        {
            case 0:
                if (_state == State.Default) return new MainScene(_gameState); // MainScene()
                else return new ShopScene(_gameState, State.Default);
            case 1:
                return new ShopScene(_gameState, State.Buy); // 구매하기
            case 2:
                return new ShopScene(_gameState, State.Sell); // 판매하기
            default :
                return this;
        };
    }
}