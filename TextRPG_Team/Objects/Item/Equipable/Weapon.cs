namespace TextRPG_Team.Objects;

public class Weapon : EquipableItem
{
    public Weapon(int id, string name, Stats effect, string info, int price)
        : base(id, name, effect, info, price)
    {
        Icon = " 🗡️   "; // 무기 아이콘
    }
    
    public override string GetEffectDisplay()
    {
        // 공격력 표시
        return $"공격력 +{Effect.Atk}";
    }
}