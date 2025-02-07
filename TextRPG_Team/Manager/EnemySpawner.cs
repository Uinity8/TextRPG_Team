using System;
using System.Collections.Generic;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Manager
{
    public class EnemySpawner
    {
        private List<Enemy> enemy = new List<Enemy>();  // 적 리스트
        private Random random = new Random();

        private EnemySpawner() 
        {
            enemy.Add(new Enemy(2, "슬라임", 20, 5));
            enemy.Add(new Enemy(5, "폭탄병", 30, 15));
        }

        public Enemy GetRandomEnemy()
        {
            int index = random.Next(enemy.Count); // 0 ~ (적군 개수 - 1) 사이의 랜덤 값
            return enemy[index];
        }
    }
}
