namespace TextRPG_Team.Objects;

public class Player : ICharacter
{
    public string Name { get; private set; }
    public float Health { get; private set; }
    public float Power { get; set; }

    public Player(string name, float health, float power)
    {
        Name = name;
        Health = health;
        Power = power;
    }

    // 공격 시 동작을 정의하는 Action
    public Action<ICharacter, float>? AttackAction { get; set; }
    
    public void Attack(ICharacter target)
    {
        // Action이 정의돼 있다면 실행
        AttackAction?.Invoke(this, Power);
    }

    public void TakeDamage(float damage)
    {
        Health = Math.Max(0, Health - damage);
    }

    public bool IsDead() => Health <= 0f;
}