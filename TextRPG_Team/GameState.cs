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
        new Item("수련자의 갑옷", ItemType.Armor, new Stats(0, 0, 4), "수련에 도움을 주는 갑옷입니다. ", 1000, 1),
        new Item("무쇠갑옷", ItemType.Armor, new Stats(0, 0, 9), "무쇠로 만들어져 튼튼한 갑옷입니다. ", 2000, 2),
        new Item("스파르타의 갑옷", ItemType.Armor, new Stats(0, 15, 15), "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500, 3),
        new Item("낣은 검", ItemType.Weapon, new Stats(0, 5, 0), "쉽게 볼 수 있는 낡은 검 입니다. ", 600, 4),
        new Item("청동 도끼", ItemType.Weapon, new Stats(0, 10, 0), "어디선가 사용됐던거 같은 도끼입니다. ", 1500, 5),
        new Item("스파르타의 창", ItemType.Weapon, new Stats(0, 20, 0), "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500, 6)
    };
    public readonly Player Player = new Player("Chad", new Stats(100, 10, 5), 1500, "Job");
    public readonly EnemySpawner Spawner = new EnemySpawner();
    
    public float PlayerHpBeforeDungeon;

    public readonly List<Quest> QuestList = new List<Quest>
    {
        new Quest("전투 경험","이봐! 마을 근처에 적들이 너무 많아졌다고 생각하지 않나?\n" +
                            "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n" +
                            "모험가인 자네가 좀 처치해주게\n" +
                            "\n- 적 1회 처치",
                            false, false, 200,1),
        new Quest("장비 장착","너에게 맞는 장비를 장착해봐!\n" +
                                "장비를 장착하면 더욱 강해질 수 있어!\n"+
                                "\n- 장비 장착 ",
                                false, false, 400,2),
        new Quest("스킬 사용","적을 공격하는 강력한 스킬을 사용해봐!\n" +
                                "스킬은 MP를 소모해서 많이 사용하면 MP가 바닥날거야.\n"+
                                "\n- 스킬 사용",
                                false, false, 1000,3)
    };
}