using System;
using System.Collections.Generic;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Manager
{
    public class EnemySpawner
    {
        private List<Enemy> enemies = new List<Enemy>();  // 적 리스트
        private Random random = new Random();

        // 생성자에서 적을 추가
        private EnemySpawner()
        {
            // 예시: 미니언, 공허충, 대포미니언을 생성
            enemies.Add(new Enemy("미니언", new Stats(15, 5, 0), 2));  // Lv 2
            enemies.Add(new Enemy("공허충", new Stats(10, 9, 0), 3));  // Lv 3
            enemies.Add(new Enemy("대포미니언", new Stats(25, 8, 0), 5));  // Lv 5
        }

        // 랜덤으로 적을 반환
        public Enemy GetRandomEnemy()
        {
            int index = random.Next(enemies.Count); // 0 ~ (적군 개수 - 1) 사이의 랜덤 값
            return enemies[index];
        }
    }
}
