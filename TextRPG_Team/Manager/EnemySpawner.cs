using TextRPG_Team.Objects;

namespace TextRPG_Team.Manager
{
    public class EnemySpawner
    {
        public int ClearNum = 0;
        private readonly List<Enemy> _baseEnemies = new List<Enemy>(); // 기본 적 리스트
        private List<Enemy> _spawnedEnemies = new List<Enemy>(); // 랜덤 생성된 적 리스트
        private readonly Random _random = new Random(); // Random 객체를 클래스 전체에서 공유

        // 생성자에서 초기화
        public EnemySpawner()
        {
            InitializeBaseEnemies();
        }

        // 기본 적 목록 초기화 메서드
        private void InitializeBaseEnemies()
        {
            _baseEnemies.Clear();

            int  i = ClearNum / 3;
            _baseEnemies.Add(new Enemy("상사","밖에서 만나니 별로 기쁘지 않다. 아는 척 안 했으면...!", new Stats(15 + i, 5 + i, 0, 2 + i), 0)); // Lv 2
            _baseEnemies.Add(new Enemy("선생님","야자를 튄게 들킨거 같다. 엄청난 기세를 가지고 다가온다.", new Stats(10 + i, 9 + i, 0, 3 + i), 1)); // Lv 3
            _baseEnemies.Add(new Enemy("명절에 만난 친척","조언인 척 하는 잔소리를 자꾸 내뱉는다. 윽! 고통스러워! ", new Stats(25 + i, 8 + i, 0, 5 + i), 2)); // Lv 5
        }

        // 기본 적 리스트 가져오기
        public List<Enemy> GetBaseEnemies()
        {
            return _baseEnemies;
        }

        // 현재 생성된 적 리스트 가져오기
        public List<Enemy> GetSpawnedEnemies()
        {
            return _spawnedEnemies;
        }

        // 랜덤한 수의 적 생성 (1~4마리) 및 저장
        public void AddRandomEnemies()
        {
            InitializeBaseEnemies();
            
            int minEnemy = 1 + ClearNum;
            int maxEnemy = 5 + ClearNum;

            _spawnedEnemies = new List<Enemy>();
            // 1~4 사이의 랜덤 숫자를 생성
            int randomEnemyCount = _random.Next(minEnemy, maxEnemy);
            
            for (int i = 0; i < randomEnemyCount; i++)
            {
                // 기본 적 목록에서 랜덤 적 선택
                var randomEnemy = _baseEnemies[_random.Next(_baseEnemies.Count)];

                // 새 객체로 적을 생성 (복제)
                var newEnemy = new Enemy(randomEnemy.Name, randomEnemy.Info,randomEnemy.TotalStats, randomEnemy.Id);

                // 생성된 적 리스트에 추가
                _spawnedEnemies.Add(newEnemy);
            }
        }
    }
}