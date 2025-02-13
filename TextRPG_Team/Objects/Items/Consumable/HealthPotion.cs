namespace TextRPG_Team.Objects.Items.Consumable;

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
        return $"체력+{HealValue}";
    }

    // 소비 아이템 사용 메서드
    public override void Use(Player player)
    {

        if (player.Health >= player.TotalStats.MaxHp)
        {
            Utility.AddLog($"이미 최대 체력 입니다.\n", ConsoleColor.Blue);
            return;
        }
        Count--;
        player.Heal(HealValue);
        string log = $"{Name}을(를) 사용했습니다! {HealValue}만큼 회복되었습니다.(현재 HP: {player.Health})\n";
        Utility.AddLog(log, ConsoleColor.Green);
    }
}