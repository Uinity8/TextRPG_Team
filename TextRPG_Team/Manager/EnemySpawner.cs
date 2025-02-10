using System;
using System.Collections.Generic;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Manager
{
    public class EnemySpawner
    {
        private List<Enemy> enemies = new List<Enemy>(); // 적 리스트
        private Random random = new Random(); // Random 객체를 클래스 전체에서 공유

        // 생성자에서 적 기본 리스트 초기화
        public EnemySpawner()
        {
            InitializeEnemies();
        }

        // 기본 적 목록 초기화 메서드 (리팩토링)
        private void InitializeEnemies()
        {
            enemies.Add(new Enemy("미니언", new Stats(15, 5, 0), 2)); // Lv 2
            enemies.Add(new Enemy("공허충", new Stats(10, 9, 0), 3)); // Lv 3
            enemies.Add(new Enemy("대포미니언", new Stats(25, 8, 0), 5)); // Lv 5
        }

        // 적 리스트 가져오기
        public List<Enemy> GetEnemies()
        {
            return enemies;
        }

        // 랜덤한 수의 적 추가 (1~4마리)
        public void AddRandomEnemies()
        {
            // 1~4 사이의 랜덤 숫자를 생성
            int randomEnemyCount = random.Next(1, 5);

            for (int i = 0; i < randomEnemyCount; i++)
            {
                // 랜덤 적을 선택
                var randomEnemy = enemies[random.Next(enemies.Count)];

                // 새 객체로 적을 추가 (기존 적이 변하지 않도록 복제)
                enemies.Add(new Enemy(randomEnemy.Name, randomEnemy.GetStats, randomEnemy.GetStats.Lv));
            }
        }

        // 랜덤 적 리스트 반환
        public List<Enemy> GetRandomEnemies(int enemyCount)
        {
            var randomEnemies = new List<Enemy>();

            for (int i = 0; i < enemyCount; i++)
            {
                // 랜덤 적을 선택
                var randomEnemy = enemies[random.Next(enemies.Count)];

                // 복제하여 반환 (참조 공유 방지)
                randomEnemies.Add(new Enemy(randomEnemy.Name, randomEnemy.GetStats, randomEnemy.GetStats.Lv));
            }

            return randomEnemies;
        }
    }
}