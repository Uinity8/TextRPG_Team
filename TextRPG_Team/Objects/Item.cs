using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Team.Objects;

using static ConsoleColor;

public enum ItemType
{
    Weapon, // 무기
    Armor, // 방어구
    Double // 공+방
}

public class Item
{
    public Stats Effect { get; } // 능력치 값
    public string Name { get; } // 이름
    public ItemType Type { get; } // 아이템 종류 (무기 or 방어구)
    public string Info { get; } // 아이템 정보
    public int Price { get; } // 가격
    public int SellPrice { get; } //판매 가격
    public bool itemPurchase { get; set; } // 아이템 구매 여부
    public bool itemEquip { get; set; } // 아이템 장착 여부

    public int Id { get; set; } //아이템 ID
    public string Icon { get; set; }

    public Item(string name, ItemType type, Stats effect, string info, int price, int id)
    {
        Name = name;
        Type = type;
        Effect = effect;
        Info = info;
        Price = price;
        itemPurchase = false;
        itemEquip = false;
        Id = id;

        string[] icon = { " 🗡️", " 🛡️", "🗡️🛡️" };
        Icon = icon[(int)Type];
        SellPrice = (int)(price * 0.85f);
    }

    public string GetItemDisplay() // 장착여부 표시
    {
        string str = itemEquip ? "[E]" : ""; // 장착중이면 : "[E]" / 아니면 : "" 출력
        str += $"{Name} | {GetTypeValue()} | {Info}";
        return str;
    }

    public string GetTypeValue() // 아이템 능력치 표시 ( 공격력 || 방어력 )
    {
        string str = (Type == ItemType.Weapon
            ? $"공격력 +{Effect.Atk}"
            : $"방어력 +{Effect.Def}"); // 타입이 무기면 공격력 / 아니면 방어력 출력
        str = (Type != ItemType.Double ? str : $"공격력 + {Effect.Atk} | 방어력 +{Effect.Def}");
        return str;
    }

    public void PrintNameAndEffect(ConsoleColor color)
    {
        if (itemPurchase)
            color = White;
        Utility.AlignLeft(Name, 16, color);
        Utility.AlignLeft("| " + GetTypeValue(), 34, color);
    }

    public void PrintPriceForSell()
    {
        Utility.AlignRight(SellPrice.ToString(), 7);
        Utility.ColorWriteLine(" G", Yellow);
    }

    public void PrintPriceForBuy(ConsoleColor color)
    {
        if (itemPurchase)
            color = DarkGray;
        
        if (itemPurchase)
            Utility.AlignRight("구매완료\n", 11);
        else
        {
            Utility.AlignRight(Price.ToString(), 7, color);
            Utility.ColorWriteLine(" G", Yellow);
        }
    }

    public void PrintInfo()
    {
        Utility.AlignLeft("", 5);
        Utility.ColorWriteLine("└ " + Info, DarkGray);
    }
}