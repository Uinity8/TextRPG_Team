using System;
using System.Collections.Generic;

namespace TextRPG_Team.Objects;

using static ConsoleColor;

public class Enemy : ICharacter
{
    string[] Icon = { " 🦠", " 🎯", " 🐀" };
    public int Id { get; }
    public string Name { get; private set; }

    //삭제 부탁(Stat 구조체로 이동)public int Level { get; private set; } //삭제 부탁(Stat 구조체로 이동)
    public float Health { get; private set; }
    public Action<ICharacter, float>? AttackAction { get; set; } // 공격 시 동작을 정의하는 액션

    private Stats _stats; // 기본 스탯

    public Stats GetStats => _stats;

    public float Power => GetStats.Atk; // 적 공경력  

    public Enemy(string name, Stats stats, int id)
    {
        Name = name;
        _stats = stats;
        Health = GetStats.MaxHp; // 최대 체력 초기화
        Id = id;
    }

    // 공격
    public void PerformAttack(ICharacter target)
    {
        if (target.IsDodge())
        {
            string log = $"{target.Name}을 공격했지만 아무일도 일어나지 않았습니다.\n"; // 공격 로그 생성
            Utility.AddLog(log, Yellow); // 로그 출력
            return;
        }
        // 공격 동작 실행
        var isCritical = new Random().NextDouble() < 0.15; // 랜덤 확률 적용(15%)
        var totalDamage = isCritical ? (float)Math.Floor(Power * 1.6f) : Power;
     
        if (isCritical)
        {
            string log = $"{target.Name}에게 {Power}의 데미지를 입혔습니다.- 치명타 공격!!\n"; // 공격 로그 생성
            Utility.AddLog(log, Magenta); // 로그 출력
        }
        else
        {
            string log = $"{target.Name}에게 {Power}의 데미지를 입혔습니다.\n"; // 공격 로그 생성
            Utility.AddLog(log, Red); // 로그 출력
        }
        
        target.TakeDamage(totalDamage); // 대상의 TakeDamage 호출
    }

    // 피해 처리 
    public void TakeDamage(float damage)
    {
        float preHp = Health;
        Health = Math.Max(0, Health - damage);
        string hpStr = Health > 0 ? Health.ToString() : "Dead";
        var log = $"{Icon[Id]} Lv.{GetStats.Lv} {Name} HP {preHp} -> {hpStr}\n";
        Utility.AddLog(log, Blue); // 로그 출력
    }

    public bool IsDodge()  //회피 
    {
        var isDodge = new Random().NextDouble() < 0.1; // 랜덤 확률 적용(10%)
        
        return isDodge;
    }
    
    // 사망 여부 체크 
    public bool IsDead() => Health <= 0f;

    // 몬스터 정보 출력
    public void PrintInfo(ConsoleColor color = White)
    {
        Utility.AlignLeft(Icon[Id], 4, color);
        Utility.AlignLeft($" Lv.{GetStats.Lv}", 6, color);
        Utility.AlignLeft(Name, 15, color);
        string hpStr = Health > 0 ? Health.ToString() : "Dead";
        Utility.AlignLeft($"HP : {hpStr}", 2, color);
        Console.WriteLine();
    }
}

