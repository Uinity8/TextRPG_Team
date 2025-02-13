namespace TextRPG_Team.Objects.Items.Equipable;

public class CursedItem : EquipableItem
{
    public CursedItem(int id, string name, Stats effect, string info, int price)
        : base(id, name, effect, info, price)
    {
        Icon = " ☠️   "; // 저주 아이템 아이콘
    }

    public override string GetEffectDisplay()
    {
        // 공/방 동시 표시
        return $"공격력 {Effect.Atk:+#;-#;+0} | 방어력 {Effect.Def:+#;-#;+0}";
    }
}