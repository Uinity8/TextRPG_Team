﻿using System;
using System.Collections.Generic;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Manager
{
    public class EnemySpawner
    {
        public int clearNum = 0;
        private List<Enemy> baseEnemies = new List<Enemy>(); // 기본 적 리스트
        private List<Enemy> spawnedEnemies = new List<Enemy>(); // 랜덤 생성된 적 리스트
        private Random random = new Random(); // Random 객체를 클래스 전체에서 공유

        // 생성자에서 초기화
        public EnemySpawner()
        {
            InitializeBaseEnemies();
        }

        // 기본 적 목록 초기화 메서드
        private void InitializeBaseEnemies()
        {
            baseEnemies.Clear();

            int  i = clearNum / 3;
            baseEnemies.Add(new Enemy("미니언", new Stats(15 + i, 5 + i, 0, 2 + i), 0)); // Lv 2
            baseEnemies.Add(new Enemy("공허충", new Stats(10 + i, 9 + i, 0, 3 + i), 1)); // Lv 3
            baseEnemies.Add(new Enemy("대포미니언", new Stats(25 + i, 8 + i, 0, 5 + i), 2)); // Lv 5
        }

        // 기본 적 리스트 가져오기
        public List<Enemy> GetBaseEnemies()
        {
            return baseEnemies;
        }

        // 현재 생성된 적 리스트 가져오기
        public List<Enemy> GetSpawnedEnemies()
        {
            return spawnedEnemies;
        }

        // 랜덤한 수의 적 생성 (1~4마리) 및 저장
        public void AddRandomEnemies()
        {
            InitializeBaseEnemies();
            
            int minEnemy = 1 + clearNum;
            int maxEnemy = 5 + clearNum;

            spawnedEnemies = new List<Enemy>();
            // 1~4 사이의 랜덤 숫자를 생성
            int randomEnemyCount = random.Next(minEnemy, maxEnemy);
            
            for (int i = 0; i < randomEnemyCount; i++)
            {
                // 기본 적 목록에서 랜덤 적 선택
                var randomEnemy = baseEnemies[random.Next(baseEnemies.Count)];

                // 새 객체로 적을 생성 (복제)
                var newEnemy = new Enemy(randomEnemy.Name, randomEnemy.TotalStats, randomEnemy.Id);

                // 생성된 적 리스트에 추가
                spawnedEnemies.Add(newEnemy);
            }
        }
    }
}