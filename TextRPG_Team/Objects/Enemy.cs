using System;
using System.Collections.Generic;

namespace TextRPG_Team.Objects
{
    public class Enemy : ICharacter
    {
        public string Name { get; private set; }
        //삭제 부탁(Stat 구조체로 이동)public int Level { get; private set; } //삭제 부탁(Stat 구조체로 이동)
        public float Health { get; private set; }
        public Action<ICharacter, float>? AttackAction { get; set; } // 공격 시 동작을 정의하는 액션
        
        private Stats EnemyStats { get; set; } // 적군 기본 스텟 (체력, 공격력)
        public Stats GetStats() => EnemyStats; // 최종 스텟
        public float Power => EnemyStats.Atk;  // 적 공경력  
         
        public Enemy(string name, Stats stats, int level)
        {
            Name = name;
            EnemyStats = stats;
            //삭제 부탁(Stat 구조체로 이동) Level = level;  // 삭제 부탁
            Health = EnemyStats.MaxHp;  // 최대 체력 초기화
        }

        // 공격
        public void Attack(ICharacter target)
        {
            // 공격 액션이 정의되어 있다면 실행
            AttackAction?.Invoke(target, Power);
        }

        // 피해 처리 
        public void TakeDamage(float damage)
        {
            Health = Math.Max(0, Health - damage);  // 피해를 받아서 체력 감소
        }

        // 사망 여부 체크 
        public bool IsDead() => Health <= 0f;

        // 몬스터 정보 출력
        public void ShowInfo()
        {
            // 레벨, 이름, 체력
            Console.WriteLine($"Lv.{GetStats().Lv} {Name} \nHP {Health} \n");
        }
    }
}
