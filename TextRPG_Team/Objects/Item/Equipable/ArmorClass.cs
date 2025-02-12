namespace TextRPG_Team.Objects;

public class Armor : EquipableItem
{
    public Armor(int id, string name, Stats effect, string info, int price)
        : base(id, name, effect, info, price)
    {
        Icon = " 🛡️"; // 방어구 아이콘
    }

    public override string GetEffectDisplay()
    {
        // 방어력 표시
        return $"방어력 +{Effect.Def}";
    }
}