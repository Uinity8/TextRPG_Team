//using TextRPG_Mockup.Objects;

using TextRPG_Team.Manager;
using TextRPG_Team.Objects;

namespace TextRPG_Team;

public class GameState
{
    //씬전환간에 필요한 인스터스들

    //Objects
    // public readonly List<Item> _itemList = new List<Item>()
    // {
    //     new Weapon(1,"노트북" , new Stats(0, 20, 0), "보기보다 가벼운게 아마도 L* gr*m 인거 같다.", 2800),
    //     new Weapon(2,"수특책", new Stats(0, 15, 0), "앞표지만 너덜거린다. 올해 수능이 며칠 남은거지 ?", 1700),
    //     new Weapon(3,"회사 다이어리", new Stats(0, 10, 0), "00회사 창립 00주년 이라 적혀있다. 펼쳐보니 이곳 저곳 욕이 적혀있다. ", 1500),
    //     new Armor(4,"책가방", new Stats(0, 0, 20), "매우 크고 무겁다. 등에 매면 거북이처럼 변할 수 있을거 같다.", 2300),
    //     new Armor(5,"면접용 정장" , new Stats(0, 0, 15), " 팔 다리 부분이 짧은거 같다. 기분탓인가?", 2800),
    //     new Armor(6,"블루라이트 차단 안경", new Stats(0, 0, 10), "왠지 이걸 쓰면 눈이 덜 피로한거 같다.", 1700),
    //     new CursedItem(7,"수상한텀블러", new Stats(0, 15, -10), "누군가의 각성제로 쓰이는거 같다. 이 냄새는 알코올?", 2300),
    //     new CursedItem(8,"스터디 플래너", new Stats(0, -10, 15), "계획은 완벽하다! ...실행만 안 됐을 뿐", 1800),
    //     new CursedItem(9,"SNS 알고리즘", new Stats(0, -15, 10), "이상하게 내 취향을 너무 잘 안다. 진짜 이것만 봐야지", 1500),
    //     new HealthPotion(10, "박사", "박ㅋ스와 사이다를 섞은 음료. 캬~!", 20, 100),
    //     new HealthPotion(11, "엄마가 끓인 카레", "먹어도 먹어도 줄지 않는다. 양이 더 늘었나...?", 50, 500),
    //     new HealthPotion(12, "유명 쉐프의 오마카세", "티켓팅을 뚫고 겨우 예약한 보람이 있다.", 300, 100000),
    //    // new ManaPotion(13, "아몬드바", "뭔가 피곤하다면 당 충전!", 30, 500),
    //     //new ManaPotion(14, "레ㄷ불", "레ㄷ불이 날개를 펼쳐줄거야~", 600, 1100),
    // };

    public readonly List<Item> _itemList = LoadManager.AllItemList;
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

    public GameState()
    {
        // ID가 10인 포션 가져오기
        HealthPotion hpPotion = LoadManager.AllItemList.Find(i => i.Id == 10) as HealthPotion;
        if (hpPotion != null)
        {
            // 초기 단계에서 플레이어에게 10번 포션을 주는 로직
            Player.AddPotion(hpPotion, 3); 
        }
        else
        {
            Console.WriteLine("포션(ID: 10)을 찾을 수 없습니다.");
        }
    }
    
}