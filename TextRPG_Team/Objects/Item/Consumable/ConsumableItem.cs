namespace TextRPG_Team.Objects;

public class ConsumableItem : Item
{
    public int HealValue { get; } // 회복량

    public ConsumableItem(int id, string name, Stats effect, string info, int price, int healValue)
        : base(id, name, effect, info, price)
    {
        HealValue = healValue;
    }

    // 소비 아이템만의 효과 표시
    public override string GetEffectDisplay()
    {
        return $"+{HealValue} 회복";
    }

    // 소비 아이템 사용 메서드
    public void Use(Player player)
    {
        //player.Heal(HealValue);
        Console.WriteLine($"{Name}을(를) 사용했습니다! 체력이 {HealValue}만큼 회복되었습니다.");
        // 이후 인벤토리에서 제거하는 로직이 필요합니다.
    }
}