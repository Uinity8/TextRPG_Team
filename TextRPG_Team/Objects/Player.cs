namespace TextRPG_Team.Objects;
using static ConsoleColor;

public class Player : ICharacter
{
    // ====== 필드 ======

    // ====== 속성 ======
    /// <summary>캐릭터 이름</summary>
    public string Name { get; set; }

    /// <summary>캐릭터 직업</summary>
    public string Job { get; set; }

    /// <summary>소지 금액</summary>
    public int Gold { get; set; }

    /// <summary>현재 체력</summary>
    public float Health { get;  set; }

    /// <summary>최종 공격력 (스탯 기반, 치명타 미적용)</summary>
    public float Power => GetStats.Atk;

    /// <summary>소유 아이템 목록</summary>
    public List<Item> Inventory { get; }


    // ====== 스탯 ======
    public Stats _stats; // 기본 스탯
    private Stats AddStats { get; set; } // 추가 스탯

    /// <summary>기본 스탯과 추가 스탯을 합친 최종 스탯 반환</summary>
    public Stats GetStats => _stats + AddStats;

    // ====== 생성자 ======
    /// <summary>
    /// 새로운 플레이어 생성
    /// </summary>
    /// <param name="name">플레이어 이름</param>
    /// <param name="stats">초기 스탯</param>
    /// <param name="gold">초기 골드</param>
    /// <param name="job">플레이어 직업</param>
    public Player(string name, Stats stats, int gold, string job)
    {
        Name = name;
        _stats = stats;
        Gold = gold;
        Health = _stats.MaxHp;
        Inventory = new List<Item>();
        Job = job;
    }

    // ====== 메서드 ======
    /// <summary>타겟에게 공격을 수행</summary>
    /// <param name="target">대상 캐릭터</param>
    public void PerformAttack(ICharacter target)
    {
        // 공격 동작 실행
        var log = $"{Name}(이)가 Lv.{target.GetStats.Lv} {target.Name}에게 {Power}의 데미지를 입혔습니다.\n"; // 공격 로그 생성
        Utility.AddLog(log, ConsoleColor.Blue); // 로그 출력

        target.TakeDamage(Power); // 대상의 TakeDamage 호출
    }


    /// <summary>데미지를 받아 체력을 감소</summary>
    /// <param name="damage">받는 데미지 값</param>
    public void TakeDamage(float damage)
    {
        float preHp = Health;
        Health = Math.Max(0, Health - damage);
        string hpStr = Health > 0 ? $"{Health}" : "Dead";
        var log = $"Lv.{GetStats.Lv} {Name} HP {preHp} -> {hpStr}\n";
        Utility.AddLog(log, ConsoleColor.Blue); // 로그 출력
    }

    /// <summary>플레이어가 사망했는지 여부를 반환</summary>
    public bool IsDead() => Health <= 0f;
    

    /// <summary>아이템 구매 처리 메서드</summary>
    /// <param name="item">구매할 아이템</param>
    public bool BuyItem(Item item)
    {
        if (Inventory.FindAll(i => i.Id == item.Id).FirstOrDefault() != null)

        {
            Utility.AddLog("이미 보유한 아이템 입니다.\n", ConsoleColor.Red);
            return false;
        }

        if (Gold < item.Price)
        {
            Utility.AddLog("골드가 부족합니다\n", ConsoleColor.Red);
            return false;
        }

        // 아이템 구매 성공
        Gold -= item.Price;
        Inventory.Add(item);
        Utility.AddLog($"성공적으로 구매하였습니다. -{item.Price} G\n", ConsoleColor.DarkBlue);
        return true;
            
    }

    /// <summary>아이템 장착/해제</summary>
    public void EquipItem(int index)
    {
        var equpItem = Inventory[index];
        // 기존 장착 해제
        foreach (var invItem in Inventory)
        {
            if (invItem.Type == equpItem.Type && invItem.itemEquip)
            {
                invItem.itemEquip = false;
            }
        }
        equpItem.itemEquip = !equpItem.itemEquip;
        CalculateAddStats();
    }

    public bool TrySell(Item item)
    {
        bool canSell = !item.itemPurchase;
        
        int index = Inventory.FindIndex(i => i.Id == item.Id);
        EquipItem(index);
        SellItem(item);
        return canSell;
    }

    /// <summary>아이템 판매 처리 메서드</summary>
    /// <param name="item">판매할 아이템</param>
    public void SellItem(Item item)
    {
        Item? sell = Inventory.Find(i => i.Id == item.Id);
        if (sell == null) return;

        if (sell.itemEquip)
        {
            sell.itemEquip = false;
            CalculateAddStats();
        }

        Gold += (int)(sell.Price * 0.85);
        Inventory.Remove(sell);
        Utility.AddLog($"성공적으로 판매하였습니다.(+{item.Price} G)\n", ConsoleColor.DarkBlue);;
    }

    /// <summary>아이템 장착 효과를 계산해 추가 스탯에 반영</summary>
    private void CalculateAddStats()
    {
        var itemStats = new Stats(0, 0, 0);
        foreach (var item in Inventory.FindAll(i => i.itemEquip))
        {
            itemStats += item.Effect;
        }
        AddStats = itemStats;
    }


    /// <summary>현재 플레이어의 정보를 문자열로 반환</summary>
    public override string ToString()
    {
        return $"Lv.{GetStats.Lv} : {Name} [{Job}]" + "\n" +
               $"HP : {Health} / {GetStats.MaxHp}" + (AddStats.MaxHp > 0 ? $"(+{AddStats.MaxHp})" : "") + "\n" +
               $"공격력 : {GetStats.Atk}" + (AddStats.Atk > 0 ? $"(+{AddStats.Atk})" : "") + "\n" +
               $"방어력 : {GetStats.Def}" + (AddStats.Def > 0 ? $"(+{AddStats.Def})" : "") + "\n" +
               $"Gold : {Gold} G";
    }

    public void PrintInfo()
    {
        int width = 10;
        
        Console.WriteLine();
        Console.Write($" [ Lv.{GetStats.Lv} ] {Name}  ");
        Utility.ColorWriteLine($"( {Job} )", Cyan);
        Console.WriteLine();
        Console.WriteLine(new string('-', Utility.Width));
        Utility.AlignLeft($"\n HP",width);
        Console.Write($": ");
        if (Health == GetStats.MaxHp)
            Utility.ColorWrite($"{Health} / {GetStats.MaxHp}", ConsoleColor.DarkGreen);
        else
            Utility.ColorWrite($"{Health} / {GetStats.MaxHp}", ConsoleColor.DarkRed);
        if (AddStats.MaxHp > 0) Utility.ColorWrite($"(+{AddStats.MaxHp})", ConsoleColor.DarkBlue);
        
        Utility.AlignLeft("\n 공격력", width);
        Console.Write($": {GetStats.Atk}");
        if (AddStats.Atk > 0) Utility.ColorWrite($"(+{AddStats.Atk})", ConsoleColor.DarkBlue);
        
        Utility.AlignLeft("\n 방어력", width);
        Console.Write($": {GetStats.Def}");
        if (AddStats.Def > 0) Utility.ColorWrite($"(+{AddStats.Def})", ConsoleColor.DarkBlue);
        
        Console.WriteLine("\n");
        Console.WriteLine(new string('-', Utility.Width));
        //Utility.AlignLeft(" Gold", width-1);
        Utility.AlignRight($"{Gold}", Utility.Width - 5);
        Utility.ColorWriteLine(" G", Yellow);
        Console.WriteLine(new string('-', Utility.Width));
    }
}