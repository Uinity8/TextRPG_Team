namespace TextRPG_Team.Objects;

public class Item
{
    public int Id { get; set; } // 아이템 ID
    public string Name { get; } // 이름
    public string Info { get; } // 아이템 정보
    public int Price { get; } // 가격
    public int SellPrice { get; } // 판매 가격
    public string Icon { get; set; } // 아이템 아이콘

    protected Item(int id, string name, string info, int price)
    {
        Icon = "     ";
        Id = id;
        Name = name;
        Info = info;
        Price = price;
        SellPrice = (int)(price * 0.85f);
    }

    // 출력 메서드
    public virtual string GetItemDisplay()
    {
        return $"{Name} | {GetEffectDisplay()}";
    }

    public void PrintInfo()
    {
        Utility.AlignLeft("", 5);
        Utility.ColorWriteLine($"└ {Info}", ConsoleColor.DarkGray);
    }

    // 효과 표시 (자식 클래스에서 구현)
    public virtual string GetEffectDisplay()
    {
        return "";
    }
}