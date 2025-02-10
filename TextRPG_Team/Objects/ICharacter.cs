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
            Def = s1.Def + s2.Def,
            Lv = s1.Lv + s2.Lv
        };
    }
}

public interface ICharacter
{
    string Name { get;  }   //이름
    float Health { get; }   //현재 체력
    float Power { get;  }   //최종 데미지
    
    // 상대방에게 데미지를 주는 Action 선언
    Action<ICharacter, float>? AttackAction { get; set; }
    
    void Attack(ICharacter target); //공격
    void TakeDamage(float damage);  //피해
    bool IsDead();  //사망
}