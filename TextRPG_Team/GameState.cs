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
    public  List<Item> ItemList = new();
    public  Player Player = new("Chad", new Stats(100, 10, 5), 1500, "Job");
    public readonly EnemySpawner Spawner = new();

    public List<Quest> QuestList = new();

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

    public void Init()
    {
        QuestList = LoadManager.AllQuestList;
        ItemList = LoadManager.AllItemList;
    }
    
}