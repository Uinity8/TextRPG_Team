using System;
using System.Collections.Generic;

namespace TextRPG_Team.Objects
{
    public class Enemy : ICharacter
    {
        public string Name { get; private set; }    // 이름
        public Action<ICharacter, float>? AttackAction { get; set; } // 공격 시 동작을 정의하는 Action

        // Stats
        private Stats EnemyStats { get; set; }  // 기본 스텟
        private Stats AddStats { get; set; }    // 추가 스텟
        public Stats GetStats() => EnemyStats + AddStats;  // 최종 스텟

        public float Health { get; private set; }  // 현재 체력
        public float Power => GetStats().Atk;      // 최종 데미지

        // 새로운 Level 프로퍼티 추가
        public int Level { get; private set; } // 몬스터 레벨

        // 생성자
        public Enemy(string name, Stats stats, int level)
        {
            Name = name;
            EnemyStats = stats;
            Level = level;  // 레벨 설정
            Health = EnemyStats.MaxHp;  // 최대 체력 초기화
        }

        // 공격 메서드 (ICharacter 인터페이스에서 구현)
        public void Attack(ICharacter target)
        {
            // 공격 액션이 정의되어 있다면 실행
            AttackAction?.Invoke(target, Power);
        }

        // 피해 처리 메서드 (ICharacter 인터페이스에서 구현)
        public void TakeDamage(float damage)
        {
            Health = Math.Max(0, Health - damage);  // 피해를 받아서 체력 감소
        }

        // 사망 여부 체크 메서드 (ICharacter 인터페이스에서 구현)
        public bool IsDead() => Health <= 0f;

        // 몬스터 정보 출력
        public void ShowInfo()
        {
            // 레벨, 이름, 체력 출력 (형식: "Lv.{Level}, {Name}, HP{Health}")
            Console.WriteLine($"Lv.{Level}, {Name}, HP{Health}");
        }
    }
}
