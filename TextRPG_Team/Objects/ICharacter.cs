namespace TextRPG_Team.Objects;

public interface ICharacter
{
    string Name { get;  }
    float Health { get; }
    float Power { get; set; }
    
    
    // 상대방에게 데미지를 주는 Action 선언
    Action<ICharacter, float>? AttackAction { get; set; }
    
    void Attack(ICharacter target);
    void TakeDamage(float damage);
    bool IsDead();
}