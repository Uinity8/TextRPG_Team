using System;
using System.Collections.Generic;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Manager
{
    public class EnemySpawner
    {
        private List<Enemy> enemies = new List<Enemy>();  // 적 리스트

        // 생성자에서 적을 추가
        public EnemySpawner()
        {
            // 예시:설정에 따라 이름, 공격력 바뀔 수 있듬
            enemies.Add(new Enemy("미니언", new Stats(15, 5, 0), 2));  // Lv 2
            enemies.Add(new Enemy("공허충", new Stats(10, 9, 0), 3));  // Lv 3
            enemies.Add(new Enemy("대포미니언", new Stats(25, 8, 0), 5));  // Lv 5
        }

        
        public List<Enemy> GetEnemies() // 외부에서 적 리스트를 가져올 수 있도록 추가
        {
            return enemies;
        }
        public List<Enemy> GetRandomEnemies(int enemyCount)
        {
            Random random = new Random();
            var randomEnemies = new List<Enemy>();
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = enemies[random.Next(enemies.Count)];
                randomEnemies.Add(new Enemy(enemy.Name, enemy.GetStats(), enemy.GetStats().Lv));
            }
            return randomEnemies;
        }
    }
}
