//using TextRPG_Mockup.Objects;

using TextRPG_Team.Manager;
using TextRPG_Team.Objects;

namespace TextRPG_Team;

public class GameState
{
    //씬전환간에 필요한 인스터스들

    //Objects
    public readonly List<Item> _itemList = new List<Item>()
    {
        new Weapon(1,"노트북" , new Stats(0, 20, 0), "보기보다 가벼운게 아마도 L* gr*m 인거 같다. ", 2800),
        new Weapon(2,"수특책", new Stats(0, 15, 0), "앞표지만 너덜거린다. 올해 수능이 며칠 남은거지 ? ", 1700),
        new Armor(3,"책가방", new Stats(0, -10, 30), "매우 크고 무겁다. 등에 매면 거북이처럼 변할 수 있을거 같다. ", 2300),
        new Armor(4,"수상한 텀블러", new Stats(0, 15, -10), "누군가의 각성제로 쓰이는거 같다. 이 냄새는 알코올 ? ", 800),
        new Armor(5,"회사 다이어리", new Stats(0, 10, 0), "00회사 창립 00주년 이라 적혀있다. 펼쳐보니 이곳 저곳 욕이 적혀있다. ", 1500),
    };
    //public readonly List<Item> _itemList = LoadManager.LoadItems();
    public  Player Player = new Player("Chad", new Stats(100, 10, 5, 1), 1500, "Job");
    public readonly EnemySpawner Spawner = new EnemySpawner();

    public Player? PlayerBeforeDungeon;

    public float PlayerLevelBeforeDungeon;
    
}