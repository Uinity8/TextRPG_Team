using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Team.Objects;

// ExampleScene.cs 
// 씬 예제 파일 입니다. 새로운 씬 만들때 해당 파일 복사해서 이름만 바꾸세요!
namespace TextRPG_Team.Scenes;

public class ItemTestScene : IScene
{
    private readonly GameState _gameState;
    private readonly List<Item> inventoryitemList = new List<Item>();
    private readonly List<Item> itemList = new List<Item>()
    {
        new Item("수련자의 갑옷", ItemType.Armor, 4, "수련에 도움을 주는 갑옷입니다. ", 1000),
        new Item("무쇠갑옷", ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다. ", 2000),
        new Item("스파르타의 갑옷", ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500),
        new Item("낣은 검", ItemType.Weapon, 5, "쉽게 볼 수 있는 낡은 검 입니다. ", 600),
        new Item("청동 도끼", ItemType.Weapon, 10, "어디선가 사용됐던거 같은 도끼입니다. ", 1500),
        new Item("스파르타의 창", ItemType.Weapon, 20, "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500)
    };

    public ItemTestScene(GameState gameState) //DI 의존성 주입
    {
        _gameState = gameState;
    }

    public void Run()
    {

        Console.Clear(); //처음 진입시 화면 지우기

        //예제 로직
        // 현재 씬에 대한 이름 출력
        Console.WriteLine("ExampleScene.");
        // GetIteDisplay() & GetTypeValue() 적용여부
        itemList[0].itemEquip = true;
        itemList[0].itemPurchase = true;
        Console.WriteLine($"1. {itemList[0].GetIteDisplay()} | {itemList[0].GetPricPurchase()}");
        itemList[0].itemEquip = false;
        itemList[0].itemPurchase = false;
        Console.WriteLine($"1. {itemList[0].GetIteDisplay()} | {itemList[0].GetPricPurchase()}");
        // GetTypeValue() 적용여부
        Console.WriteLine($"1. {itemList[4].GetIteDisplay()} | {itemList[0].GetPricPurchase()}");
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch     //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            1 => this, // 같은 씬 유지
            2 => new MainScene(_gameState), // 메인 씬으로 돌아감
            _ => null // 잘못된 입력 시 종료
        };
    }
}