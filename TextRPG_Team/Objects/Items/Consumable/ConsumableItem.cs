namespace TextRPG_Team.Objects.Items.Consumable;

public class ConsumableItem(int id, string name, string info, int healValue, int price)
    : Item(id, name, info, price)
{
    public int HealValue { get; } = healValue; // 회복량
    public int Count { get; set; } = 1;

    public override string GetItemDisplay()
    {
        return $"{Name} | {GetEffectDisplay()} x{Count}";
    }

    // 소비 아이템만의 효과 표시
    public override string GetEffectDisplay()
    {
        return $"+{HealValue} ";
    }
    
    // 소비 아이템 사용 메서드
    public virtual void Use(Player player)
    {
        Count--;
    }
}