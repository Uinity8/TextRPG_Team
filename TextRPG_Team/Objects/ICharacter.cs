namespace TextRPG_Team.Objects;

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