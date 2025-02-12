namespace TextRPG_Team.Objects;

public class HealthPotion : ConsumableItem
{
    public HealthPotion(int id, string name, string info, int healValue, int price)
        : base(id, name, info, healValue,price)
    {
        Icon = " 🍷  ";
    }
    
    
    // 소비 아이템만의 효과 표시
    public override string GetEffectDisplay()
    {
        return $"체력+{HealValue} 회복";
    }

    // 소비 아이템 사용 메서드
    public override void Use(Player player)
    {
        base.Use(player);
        player.Heal(HealValue);
        string log = $"{Name}을(를) 사용했습니다! {HealValue}만큼 회복되었습니다.(현재 HP: {player.Health})\n";
        Utility.AddLog(log, ConsoleColor.Green);
    }
}