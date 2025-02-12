//using TextRPG_Mockup.Objects;

using TextRPG_Team.Manager;
using TextRPG_Team.Objects;

namespace TextRPG_Team;

public class GameState
{
    //씬전환간에 필요한 인스터스들

    //Objects

    public readonly List<Item> _itemList = LoadManager.AllItemList;
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