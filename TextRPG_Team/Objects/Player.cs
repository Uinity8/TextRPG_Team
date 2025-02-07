namespace TextRPG_Team.Objects;

public struct Stats // 체,공,방
{
    public float MaxHp { get; set; }    //최대 체력
    public float Atk {get; set;}    //공격력
    public float Def {get; set;}    //방어력

    public Stats(float maxHp, float atk, float def)
    {
        MaxHp = maxHp;
        Atk = atk;
        Def = def;
    }
    
    // + 연산자 오버로드
    public static Stats operator +(Stats s1,Stats s2)
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
    public string Name { get; private set; }    //이름
    public Action<ICharacter, float>? AttackAction { get; set; } //공격 시 동작을 정의하는 Action


    //Stats
    Stats PlayerStats {get; set; } //기본 스텟
    Stats AddStats { get; set; }  //추가 스텟
    public Stats GetStats() => PlayerStats + AddStats;    //최종 스텟
    public float Health { get; private set; }   //현재 체력

    public float Power => GetStats().Atk; //최종 데미지(추후에 치명타


    public Player(string name, Stats stats)
    {
        Name = name;
        PlayerStats = stats;
    }
    
    public void Attack(ICharacter target)   //공격
    {
        // Action이 정의돼 있다면 실행
        AttackAction?.Invoke(this, Power);
    }

    public void TakeDamage(float damage)    //피해
    {
        Health = Math.Max(0, Health - damage);
    }

    public bool IsDead() => Health <= 0f;   
}