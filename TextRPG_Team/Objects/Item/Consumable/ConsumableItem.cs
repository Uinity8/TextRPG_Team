namespace TextRPG_Team.Objects;

public class ConsumableItem : Item
{
    public int HealValue { get; } // 회복량
    public int Count { get; set; } = 1;

    public ConsumableItem(int id, string name, string info, int healValue, int price)
        : base(id, name, info, price)
    {
        HealValue = healValue;
    }

    // 소비 아이템만의 효과 표시
    public override string GetEffectDisplay()
    {
        return $"+{HealValue} 회복";
    }

    public string GetCountDisplay()
    {
        return $"x{Count}";
    }
    // 소비 아이템 사용 메서드
    public virtual void Use(Player player)
    {
        Count--;
    }
}