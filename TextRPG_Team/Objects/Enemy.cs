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

        private Stats _stats; // 기본 스탯

        public Stats GetStats => _stats;

        public float Power => GetStats.Atk; // 적 공경력  
        
        public Enemy(string name, Stats stats, int level)
        {
            Name = name;
            _stats = stats;
            //삭제 부탁(Stat 구조체로 이동) Level = level;  // 삭제 부탁
            Health = GetStats.MaxHp; // 최대 체력 초기화
        }

        // 공격
        public void PerformAttack(ICharacter target)
        {
            // 공격 동작 실행
            var log = $"Lv.{GetStats.Lv} {Name}(이)가 {target.Name}에게 {Power}의 데미지를 입혔습니다.\n"; // 공격 로그 생성
            Utility.AddLog(log, ConsoleColor.Red); // 로그 출력

            target.TakeDamage(Power); // 대상의 TakeDamage 호출
        }

        // 피해 처리 
        public void TakeDamage(float damage)
        {
            float preHp = Health;
            Health = Math.Max(0, Health - damage);
            string hpStr = Health > 0 ? Health.ToString() : "Dead";
            var log = $"Lv.{GetStats.Lv} {Name}\nHP {preHp} -> {hpStr}\n";
            Utility.AddLog(log, ConsoleColor.Blue); // 로그 출력
        }

        // 사망 여부 체크 
        public bool IsDead() => Health <= 0f;

        // 몬스터 정보 출력
        public override string ToString()
        {
            // 레벨, 이름, 체력
            string hpStr = Health > 0 ? Health.ToString() : "Dead";
            return $"Lv.{GetStats.Lv} {Name} HP {hpStr}";
        }
    }
}