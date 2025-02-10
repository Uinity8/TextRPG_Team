namespace TextRPG_Team.Objects;

public struct Stats // 체,공,방
{
    public float MaxHp { get; set; } //최대 체력
    public float Atk { get; set; } //공격력
    public float Def { get; set; } //방어력
    public int Lv { get; set; } //레벨

    public Stats(float maxHp, float atk, float def, int lv = 1)
    {
        MaxHp = maxHp;
        Atk = atk;
        Def = def;
        Lv = lv;
    }

    // + 연산자 오버로드
    public static Stats operator +(Stats s1, Stats s2)
    {
        return new Stats
        {
            MaxHp = s1.MaxHp + s2.MaxHp,
            Atk = s1.Atk + s2.Atk,
            Def = s1.Def + s2.Def
        };
    }
}

public class Player : ICharacter
{
    // 필드
    private int _gold;

    // 속성
    public string Name { get; private set; } //이름

    public int Gold //골드 그냥 골드에 값만 대입하시면 자동으로 로그뜹니다.
    {
        get => _gold;
        set
        {
            if (value < _gold) // 골드가 줄어들 때만 메시지 출력
            {
                if (value < 0)
                    TryBuyAction?.Invoke("골드가 부족합니다.", ConsoleColor.Red);
                else
                    TryBuyAction?.Invoke("구매에 성공했습니다.", ConsoleColor.Blue);
            }
            _gold = value; // 항상 골드 값을 설정
        }
    }

    public float Health { get; private set; } //현재 체력

    public float Power => GetStats().Atk; //최종 데미지(추후에 치명타

    // 이벤트
    public Action<ICharacter, float>? AttackAction { get; set; } //공격 시 동작을 정의하는 Action
    public Action<string, ConsoleColor>? TryBuyAction { get; set; } //구매 시도 Action


    // 스탯 관련 변수 및 메서드
    private Stats PlayerStats { get; set; } //기본 스텟
    private Stats AddStats { get; set; } //추가 스텟
    public Stats GetStats() => PlayerStats + AddStats; //최종 스텟


    // 생성자
    public Player(string name, Stats stats, int gold)
    {
        Name = name;
        PlayerStats = stats;
        _gold = gold;
        Health = PlayerStats.MaxHp;
    }

    // 메서드
    public void Attack(ICharacter target) //공격
    {
        // Action이 정의돼 있다면 실행
        AttackAction?.Invoke(this, Power);
    }

    public void TakeDamage(float damage) //피해
    {
        Health = Math.Max(0, Health - damage);
    }

    public bool IsDead() => Health <= 0f;

    public override string ToString()   //플레이어 정보
    {
        return $"Lv.{GetStats().Lv} : {Name} " + 
               $"HP : {Health} / {GetStats().MaxHp}" + (AddStats.MaxHp > 0 ? $"(+{AddStats.MaxHp})" : "\n")+
               $"공격력 : {GetStats().Atk}" + (AddStats.Atk > 0 ? $"(+{AddStats.Atk})" : "\n")+
               $"방어력 : {GetStats().Def}"+ (AddStats.Def > 0 ? $"(+{AddStats.Def})" : "\n")+
               $"Gold : {Gold} G";
    }
}