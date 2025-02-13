namespace TextRPG_Team.Objects.Items.Equipable;

public class EquipableItem(int id, string name, Stats effect, string info, int price)
    : Item(id, name, info, price)
{
    public bool itemEquip { get; set; } = false; // 아이템 장착 여부
    public Stats Effect { get; } = effect; // 능력치 값

    // 출력 메서드
    public override string GetItemDisplay()
    {
        string str = itemEquip ? "[E]" : ""; // '장착 중' 표시
        str += $"{Name} | {GetEffectDisplay()} ";
        return str;
    }
    
    // 장비 아이템의 효과 표시
    public override string GetEffectDisplay()
    {
        return $"{Effect} 효과"; // 예를 들어 능력치+ 표시
    }

    // 장비 아이템 장착/해제
    public void Equip(Player player)
    {
        itemEquip = true;
        Console.WriteLine($"{Name}을(를) 장착했습니다!");
       player.EquipStats(Effect); // 플레이어 스탯에 장비 능력치 추가
    }

    public void Unequip(Player player)
    {
        itemEquip = false;
        Console.WriteLine($"{Name}을(를) 해제했습니다!");
        player.UnEquipStats(Effect); // 플레이어 스탯에서 장비 능력치 제거
    }
}