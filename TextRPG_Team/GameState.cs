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
    
    }

    public void Init()
    {
        QuestList = LoadManager.AllQuestList;
        ItemList = LoadManager.AllItemList;
    }
    
}