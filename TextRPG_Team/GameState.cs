//using TextRPG_Mockup.Objects;

using TextRPG_Team.Manager;
using TextRPG_Team.Objects;
using TextRPG_Team.Objects.Items;
using TextRPG_Team.Objects.Items.Consumable;
using TextRPG_Team.Objects.Items.Equipable;

namespace TextRPG_Team;

public class GameState
{
    //Objects
    public readonly List<Item> ItemList = LoadManager.AllItemList;
    public  Player Player = new("Chad", new Stats(100, 10, 5), 1500, "Job");
    public readonly EnemySpawner Spawner = new();

    public readonly List<Quest> QuestList =
    [
        new Quest("앗 저 사람은...?!", "길을 가다 우연히 상사, 선생님, 명절에 만난 친척 중 한 명과 눈이 마주쳤다…!\n" +
                                  "피할 수 없다! 어서 대처하자!\n" +
                                  "\n- 전투 1회 승리",
            false, false, false, 200, 1),

        new Quest("장비를 장착하고 사회의 방어력을 높여보자!", "험난한 사회에서 살아남기 위해서는 적절한 장비가 필요하다.\n" +
                                             "상점에서 아이템을 하나 장착해 보자.\n" +
                                             "\n- 아이템 장착하기 ",
            false, false, false, 400, 2),

        new Quest("춥고 힘들고 배고프다...", "하루하루를 버티낸 당신! 허기를 채워야 다음을 버틸 수 있다.\n" +
                                    "체력 포션 또는 MP 포션 중 하나를 사용해 보자.\n" +
                                    "\n- 소비 아이템 사용하기 ",
            false, false, false, 1000, 3)
    ];

    public Player? PlayerBeforeDungeon;

    public GameState()
    {
        // ID가 10인 포션 가져오기
        if (LoadManager.AllItemList.Find(i => i.Id == 10) is HealthPotion hpPotion)
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