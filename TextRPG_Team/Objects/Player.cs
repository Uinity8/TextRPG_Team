namespace TextRPG_Team.Objects;

using static ConsoleColor;

public class Player : ICharacter
{
    // ====== 필드 ======
    int _exp; //현재 경험치
    float _health; //현재 체력

    // ====== 속성 ======
    /// <summary>캐릭터 이름</summary>
    public string Name { get; set; }

    /// <summary>캐릭터 직업</summary>
    public string Job { get; set; }

    /// <summary>소지 금액</summary>
    public int Gold { get; set; }

    /// <summary>현재 체력</summary>
    public float Health
    {
        get => _health;
        set => _health = Math.Min(value, TotalStats.MaxHp);
    }

    /// <summary>최종 공격력 (±10% 범위의 랜덤 값)</summary>
    public float Power
    {
        get
        {
            float baseAtk = TotalStats.Atk; // 기본 공격력

            // ±10% 계산
            float minAtk = baseAtk * 0.9f; // 최저 공격력 (기본 공격력의 90%)
            float maxAtk = baseAtk * 1.1f; // 최고 공격력 (기본 공격력의 110%)

            // 랜덤한 값 생성
            Random random = new Random();
            float randomizedAtk = (float)(minAtk + (maxAtk - minAtk) * random.NextDouble());

            // 최종값 올림 처리 후 반환
            return (float)Math.Ceiling(randomizedAtk);
        }
    }


    /// <summary>소유 아이템 목록</summary>
    public List<Item> Inventory { get; } = new()
    {
        new Armor(3, "책가방", new Stats(0, -10, 30), "매우 크고 무겁다. 등에 매면 거북이처럼 변할 수 있을거 같다. ", 2300),
        new Weapon(1, "노트북", new Stats(0, 20, 0), "보기보다 가벼운게 아마도 L* gr*m 인거 같다. ", 2800),
        new Weapon(2, "수특책", new Stats(0, 15, 0), "앞표지만 너덜거린다. 올해 수능이 며칠 남은거지 ? ", 1700),
        new HealthPotion(6, "체력 포션", "00회사 창립 00주년 이라 적혀있다. 펼쳐보니 이곳 저곳 욕이 적혀있다. ", 100, 100),
    };


    public int Exp
    {
        get => _exp;
        private set
        {
            if (value >= TotalStats.MaxExp)
            {
                value -= TotalStats.MaxExp;
                LevelUp();
            }

            _exp = value;
        }
    }


    // ====== 스탯 ======
    public Stats _stats; // 기본 스탯
    private Stats AddStats { get; set; } // 추가 스탯

    /// <summary>기본 스탯과 추가 스탯을 합친 최종 스탯 반환</summary>
    public Stats TotalStats => _stats + AddStats;

    // ====== 생성자 ======
    /// <summary>
    /// 새로운 플레이어 생성
    /// </summary>
    /// <param name="name">플레이어 이름</param>
    /// <param name="stats">초기 스탯</param>
    /// <param name="gold">초기 골드</param>
    /// <param name="job">플레이어 직업</param>
    public Player(string name, Stats stats, int gold, string job)
    {
        Name = name;
        _stats = stats;
        Gold = gold;
        Health = _stats.MaxHp;
        Job = job;
    }

    // ====== 메서드 ======
    /// <summary>타겟에게 공격을 수행</summary>
    /// <param name="target">대상 캐릭터</param>
    public void PerformAttack(ICharacter target)
    {
        // 공격 동작 실행
        var isCritical = new Random().NextDouble() < 0.15; // 랜덤 확률 적용(15%)
        var totalDamage = isCritical ? (float)Math.Floor(Power * 1.6f) : Power;

        if (isCritical)
        {
            string log = $"Lv.{target.TotalStats.Lv} {target.Name}에게 {totalDamage}의 데미지를 입혔습니다 -치명타 공격!!\n"; // 공격 로그 생성
            Utility.AddLog(log, Yellow); // 로그 출력
        }
        else
        {
            string log = $"Lv.{target.TotalStats.Lv} {target.Name}에게 {totalDamage}의 데미지를 입혔습니다.\n"; // 공격 로그 생성
            Utility.AddLog(log, Blue); // 로그 출력
        }

        target.TakeDamage(totalDamage); // 대상의 TakeDamage 호출

        if (target.IsDead() && target is Enemy enemy)
        {
            Exp += enemy.TotalStats.Lv;
        }
    }

    public bool IsDodge() //회피 
    {
        var isDodge = new Random().NextDouble() < 0.1; // 랜덤 확률 적용(10%)

        return isDodge;
    }


    /// <summary>데미지를 받아 체력을 감소</summary>
    /// <param name="damage">받는 데미지 값</param>
    public void TakeDamage(float damage)
    {
        float preHp = Health;
        Health = Math.Max(0, Health - damage);
        string hpStr = Health > 0 ? $"{Health}" : "Dead";
        var log = $"Lv.{TotalStats.Lv} {Name} HP {preHp} -> {hpStr}\n";

        Utility.AddLog(log, Blue); // 로그 출력
    }

    /// 레벨업 체크 및 처리
    private void LevelUp()
    {
        _stats.Lv++; // 레벨 증가
        _stats.MaxExp = (5 * (_stats.Lv * _stats.Lv - _stats.Lv)) / 2 + 10;
        _stats.MaxHp += 5; // 최대 체력 증가
        _stats.Atk += 0.5f; // 공격력 증가
        _stats.Def += 1; // 방어력 증가
        Health = _stats.MaxHp; // 체력 회복
    }

    // 플레이어가 사망했는지 여부를 반환
    public bool IsDead() => Health <= 0f;


    /// <summary>아이템 구매 처리 메서드</summary>
    /// <param name="item">구매할 아이템</param>
    public bool BuyItem(Item item)
    {
        if (Inventory.FindAll(i => i.Id == item.Id).FirstOrDefault() != null)
        {
            Utility.AddLog("이미 보유한 아이템 입니다.\n", Red);
            return false;
        }

        if (Gold < item.Price)
        {
            Utility.AddLog("골드가 부족합니다\n", Red);
            return false;
        }

        // 아이템 구매 성공
        Gold -= item.Price;
        Inventory.Add(item);
        Utility.AddLog($"성공적으로 구매하였습니다. -{item.Price} G\n", DarkBlue);
        return true;
    }

    //아이템 판매 메서드
    public void SellItem(Item item)
    {
        if (item == null) return;

        //판매 아이템이 장착아이템&장착 중이라면 장착해제
        if (item is EquipableItem equipableItem && equipableItem.itemEquip)
            equipableItem.Unequip(this);

        Gold += (int)(item.SellPrice);
        Inventory.Remove(item);

        //판매 로그 저장
        Utility.AddLog($"성공적으로 판매하였습니다.(+{item.Price} G)\n", DarkBlue);
    }

    /// <summary>현재 플레이어의 정보를 문자열로 반환</summary>
    public override string ToString()
    {
        return $"Lv.{TotalStats.Lv} : {Name} [{Job}]" + "\n" +
               $"HP : {Health} / {TotalStats.MaxHp}" + (TotalStats.MaxHp > 0 ? $"(+{TotalStats.MaxHp})" : "") + "\n" +
               $"공격력 : {TotalStats.Atk}" +
               (TotalStats.Atk > 0 ? $"(+{TotalStats.Atk})" : (TotalStats.Atk != 0 ? $"(-{TotalStats.Atk})" : "")) +
               "\n" +
               $"방어력 : {TotalStats.Def}" +
               (TotalStats.Def > 0 ? $"(+{TotalStats.Def})" : (TotalStats.Def != 0 ? $"(-{TotalStats.Def})" : "")) +
               "\n" +
               $"Gold : {Gold} G";
    }

    public void PrintInfo()
    {
        int width = 10;

        Console.WriteLine();
        Console.Write($" [ Lv.{TotalStats.Lv} ] {Name}  ");
        Utility.ColorWriteLine($"( {Job} )", Cyan);
        Console.WriteLine();
        Console.WriteLine(new string('-', Utility.Width));

        Utility.AlignLeft($"\n HP", width);
        Console.Write($": ");
        if (Health == TotalStats.MaxHp)
            Utility.ColorWrite($"{Health} / {TotalStats.MaxHp}", DarkGreen);
        else
            Utility.ColorWrite($"{Health} / {TotalStats.MaxHp}", DarkRed);
        if (AddStats.MaxHp > 0) Utility.ColorWrite($"(+{AddStats.MaxHp})", DarkBlue);

        Utility.AlignLeft($"\n Exp", width);
        Console.Write($": ");
        if (Exp == TotalStats.MaxExp)
            Utility.ColorWrite($"{Exp} / {TotalStats.MaxExp}", DarkYellow);
        else
            Utility.ColorWrite($"{Exp} / {TotalStats.MaxExp}", Yellow);

        Utility.AlignLeft("\n 공격력", width);
        Console.Write($": {TotalStats.Atk}");
        if (AddStats.Atk > 0) Utility.ColorWrite($"(+{AddStats.Atk})", DarkBlue);
        else if (AddStats.Atk < 0) Utility.ColorWrite($"({AddStats.Atk})", DarkRed);

        Utility.AlignLeft("\n 방어력", width);
        Console.Write($": {TotalStats.Def}");
        if (AddStats.Def > 0) Utility.ColorWrite($"(+{AddStats.Def})", DarkBlue);
        else if (TotalStats.Def < 0) Utility.ColorWrite($"({AddStats.Def})", DarkRed);
        Console.WriteLine("\n");

        Console.WriteLine(new string('-', Utility.Width));
        //Utility.AlignLeft(" Gold", width-1);
        Utility.AlignRight($"{Gold}", Utility.Width - 5);
        Utility.ColorWriteLine(" G", Yellow);
        Console.WriteLine(new string('-', Utility.Width));
    }

    public void EquipStats(Stats effect)
    {
        AddStats += effect;
    }

    public void UnEquipStats(Stats effect)
    {
        AddStats -= effect;
    }


    //아이템 사용
    public void UseItem(Item item)
    {
        if (item is EquipableItem equipableItem) // 장비 아이템인지 확인
        {
            EquipItem(equipableItem);  
        }
       else  if (item is ConsumableItem consumableItem) // 소비 아이템인지 확인
        {
            consumableItem.Use(this);
        }
    }

    /// <summary>아이템 장착/해제</summary>
        public void EquipItem(EquipableItem item)
        {
            // 장비가 이미 장착되어 있으면 해제
            if (item.itemEquip)
                item.Unequip(this);

            //장착중이 아니라면 조건 검사
            else
            {
                // 같은 클래스의 장비를 모두 해제
                foreach (var invItem in Inventory.FindAll(i => i.GetType() == item.GetType()))
                {
                    if (invItem is EquipableItem otherEquipable && otherEquipable.itemEquip) // itemEquip에 안전하게 접근
                    {
                        otherEquipable.Unequip(this); // 모든 같은 클래스의 아이템 해제
                    }
                }


                // 선택된 아이템 장착
                item.Equip(this);
            }
        }

        public void Heal(int healValue)
        {
            Health += healValue;
        }
    }