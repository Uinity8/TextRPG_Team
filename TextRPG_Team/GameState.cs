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
    
    public float PlayerHpBeforeDungeon;

    public readonly List<Quest> QuestList = new List<Quest>
    {
        new Quest("전투 경험","이봐! 마을 근처에 적들이 너무 많아졌다고 생각하지 않나?\n" +
                            "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n" +
                            "모험가인 자네가 좀 처치해주게\n" +
                            "\n- 적 1회 처치",
                            false, false,false, 200,1),
        new Quest("장비 장착","너에게 맞는 장비를 장착해봐!\n" +
                                "장비를 장착하면 더욱 강해질 수 있어!\n"+
                                "\n- 장비 장착 ",
                                false, false,false, 400,2),
        new Quest("스킬 사용","적을 공격하는 강력한 스킬을 사용해봐!\n" +
                                "스킬은 MP를 소모해서 많이 사용하면 MP가 바닥날거야.\n"+
                                "\n- 스킬 사용",
                                false, false,false, 1000,3)
    };

    public Player? PlayerBeforeDungeon;

    public float PlayerLevelBeforeDungeon;
    
}