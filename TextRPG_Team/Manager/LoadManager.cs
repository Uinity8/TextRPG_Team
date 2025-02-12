
using Newtonsoft.Json;
using TextRPG_Team.Objects;


namespace TextRPG_Team.Manager;

public static class LoadManager
{
    private static string itemFilePath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "items.json");
    private static string playerFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "playerData.json");
    public static List<Item> AllItemList = new();
    public static List<Item> LoadItems()
    {
        if (!File.Exists(itemFilePath))
        {
            Console.WriteLine("아이템 데이터 파일이 없습니다.");
            Thread.Sleep(1000);
            return new();
        }

        string json = File.ReadAllText(itemFilePath);

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
        File.WriteAllText(itemFilePath, json);
        Console.WriteLine("아이템 데이터가 저장되었습니다.");
        Thread.Sleep(1000);
    }

    
    public static void SavePlayerData(Player player)
    {
        
        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        string json = JsonConvert.SerializeObject(player, Formatting.None, settings);
        File.WriteAllText(playerFilePath, json);
        Console.WriteLine("플레이어 데이터가 저장되었습니다.");
        Thread.Sleep(1000);
    }
    
    public static Player LoadPlayerData()
    {
        Player defaultPlayer = new Player("Chad", new Stats(100, 10, 5, 1), 1500, "Job");
        
        // `TypeNameHandling` 옵션 활성화
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        if (!File.Exists(playerFilePath))
        {
            Console.WriteLine("저장된 데이터가 없습니다.");
            return defaultPlayer;
        }
        string json = File.ReadAllText(playerFilePath);
        return JsonConvert.DeserializeObject<Player>(json,settings) ?? defaultPlayer;
    }

    public static bool HasPlayData()
    {
        return File.Exists(playerFilePath);
    }
    
    public static void DeletePlayerData()
    {
        // 파일이 존재하는지 확인
        if (HasPlayData())
        {
            File.Delete(playerFilePath); // 파일 삭제
            Console.WriteLine($"JSON 파일이 성공적으로 삭제되었습니다: {playerFilePath}");
        }
        else
        {
            Console.WriteLine($"JSON 파일이 존재하지 않습니다: {playerFilePath}");
        }
    }

}
