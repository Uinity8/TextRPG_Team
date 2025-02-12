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
        new HealthPotion(6, "체력 포션", "00회사 창립 00주년 이라 적혀있다. 펼쳐보니 이곳 저곳 욕이 적혀있다. ", 100, 100),
        new Weapon(1,"노트북" , new Stats(0, 20, 0), "보기보다 가벼운게 아마도 L* gr*m 인거 같다. ", 2800),
        new Weapon(2,"수특책", new Stats(0, 15, 0), "앞표지만 너덜거린다. 올해 수능이 며칠 남은거지 ? ", 1700),
        new Armor(3,"책가방", new Stats(0, -10, 30), "매우 크고 무겁다. 등에 매면 거북이처럼 변할 수 있을거 같다. ", 2300),
        new Armor(4,"수상한 텀블러", new Stats(0, 15, -10), "누군가의 각성제로 쓰이는거 같다. 이 냄새는 알코올 ? ", 800),
        new Armor(5,"회사 다이어리", new Stats(0, 10, 0), "00회사 창립 00주년 이라 적혀있다. 펼쳐보니 이곳 저곳 욕이 적혀있다. ", 1500),
        new HealthPotion(6, "체력 포션", "00회사 창립 00주년 이라 적혀있다. 펼쳐보니 이곳 저곳 욕이 적혀있다. ", 100, 100),
    };
    //public readonly List<Item> _itemList = LoadManager.LoadItems();
    public  Player Player = new Player("Chad", new Stats(100, 10, 5, 1), 1500, "Job");
    public readonly EnemySpawner Spawner = new EnemySpawner();
    
    public float PlayerHpBeforeDungeon;

    public readonly List<Quest> QuestList = new List<Quest>
    {
        new Quest("앗 저 사람은...?!","길을 가다 우연히 상사, 선생님, 명절에 만난 친척 중 한 명과 눈이 마주쳤다…!\n" +
                            "피할 수 없다! 어서 대처하자!\n" +
                            "\n- 전투 1회 승리",
                            false, false,false, 200,1),
        new Quest("장비를 장착하고 사회의 방어력을 높여보자!","험난한 사회에서 살아남기 위해서는 적절한 장비가 필요하다.\n" +
                                "상점에서 아이템을 하나 장착해 보자.\n"+
                                "\n- 아이템 장착하기 ",
                                false, false,false, 400,2),
        new Quest("춥고 힘들고 배고프다...","하루하루를 버티낸 당신! 허기를 채워야 다음을 버틸 수 있다.\n"+
                                "체력 포션 또는 MP 포션 중 하나를 사용해 보자."+
                                "\n- 소비 아이템 사용하기 ",
                                false, false,false, 1000,3)
    };

    public Player? PlayerBeforeDungeon;

    public float PlayerLevelBeforeDungeon;
    
}