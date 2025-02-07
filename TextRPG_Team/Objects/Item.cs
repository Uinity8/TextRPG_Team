using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Team
{
    public enum ItemType
    {
        Weapon, // 무기
        Armor // 방어구
    }

    public class Item
    {
        public string Name { get; } // 이름
        public ItemType Type { get; } // 아이템 종류 (무기 or 방어구)
        public int Value { get; } // 능력치 값
        public string Info { get; } // 아이템 정보
        public int Price { get; } // 가격
        public bool Purchase { get; set; } // 구매 여부
        public bool Equip { get; set; } // 장착 여부

        public Item(string name, ItemType type, int value, string info, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Info = info;
            Price = price;
            Purchase = false;
            Equip = false;
        }
        public string GetItemEffect() // 장착중 표시
        {
            string str = Equip ? "[E]" : ""; // 장착중이면 : "[E]" / 아니면 : "" 출력
            str += $"{Name} | {GetTypeEffect()} | {Info}";
            return str;
        }

        public string GetTypeEffect() // 아이템 능력치 표시
        {
            string str = (Type == ItemType.Weapon ? $"공격력 +{Value}" : $"방어력 +{Value}"); // 타입이 무기면 공격력 / 아니면 방어력 출력
            return str;
        }

        public string GetPriceEffect() // 아이템 가격 표시
        {
            string str = Purchase ? "구매완료" : $"{Price}"; // 구매했으면 "구매완료" / 아니면 가격 출력
            return str;
        }

    }
}

