
using Newtonsoft.Json;
using TextRPG_Team.Objects;
using TextRPG_Team.Objects.Items;


namespace TextRPG_Team.Manager;

public static class LoadManager
{
    private static readonly string ItemFilePath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "items.json");
    private static readonly string PlayerFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "playerData.json");
    private static readonly string questFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "quests.json");
    public static List<Item> AllItemList { get; private set; } = [];
    public static List<Quest> AllQuestList { get; private set; } = [];

    public static List<Item> LoadItems()
    {
        if (!File.Exists(ItemFilePath))
        {
            Console.WriteLine("아이템 데이터 파일이 없습니다.");
            Thread.Sleep(1000);
            return new();
        }

        string json = File.ReadAllText(ItemFilePath);

        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        AllItemList = JsonConvert.DeserializeObject<List<Item>>(json, settings) ?? new();
        Console.WriteLine("아이템 데이터 로드 완료!");
        Thread.Sleep(1000);
        return AllItemList;
    }
    
    public static void SaveItemsData(List<Item> items)
    {
        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        string json = JsonConvert.SerializeObject(items, Formatting.Indented, settings);
        File.WriteAllText(ItemFilePath, json);
        Console.WriteLine("아이템 데이터가 저장되었습니다.");
        Thread.Sleep(1000);
    }
    
    public static List<Quest> LoadQuests()
    {
        if (!File.Exists(ItemFilePath))
        {
            Console.WriteLine("퀘스트 데이터 파일이 없습니다.");
            Thread.Sleep(1000);
            return new();
        }

        string json = File.ReadAllText(questFilePath);

        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        AllQuestList = JsonConvert.DeserializeObject<List<Quest>>(json, settings) ?? new();
        Console.WriteLine("퀘스트 데이터 로드 완료!");
        Thread.Sleep(1000);
        return AllQuestList;
    }
    
    public static void SaveQuests(List<Quest> Quests)
    {
        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        string json = JsonConvert.SerializeObject(Quests, Formatting.Indented, settings);
        File.WriteAllText(questFilePath, json);
        Console.WriteLine("퀘스트 데이터가 저장되었습니다.");
        Thread.Sleep(1000);
    }
    
    
    public static void SaveGameData(GameState gameState)
    {
        
        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        string json = JsonConvert.SerializeObject(gameState, Formatting.None, settings);
        File.WriteAllText(PlayerFilePath, json);
        Console.WriteLine("플레이어 데이터가 저장되었습니다.");
        Thread.Sleep(1000);
    }
    
    public static GameState LoadGameData()
    {
        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        if (!File.Exists(PlayerFilePath))
        {
            Console.WriteLine("저장된 데이터가 없습니다.");
            return new GameState();
        }
        string json = File.ReadAllText(PlayerFilePath);
        return JsonConvert.DeserializeObject<GameState>(json,settings) ?? new();
    }

    public static bool HasPlayData()
    {
        return File.Exists(PlayerFilePath);
    }
    
    public static void DeletePlayerData()
    {
        // 파일이 존재하는지 확인
        if (HasPlayData())
        {
            File.Delete(PlayerFilePath); // 파일 삭제
            Console.WriteLine($"JSON 파일이 성공적으로 삭제되었습니다: {PlayerFilePath}");
        }
        else
        {
            Console.WriteLine($"JSON 파일이 존재하지 않습니다: {PlayerFilePath}");
        }
    }

    public static void Load()
    {
        LoadItems();
        LoadQuests();
    }
}
