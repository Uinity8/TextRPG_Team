//// ExampleScene.cs 
//// 씬 예제 파일 입니다. 새로운 씬 만들때 해당 파일 복사해서 이름만 바꾸세요!

//namespace TextRPG_Team.Scenes;

//using Objects;
//using TextRPG_Team.Manager;

//public class TestScene : IScene
//{
//    private readonly GameState _gameState;
//    readonly List<Enemy> _enemies = new List<Enemy>();
    
//    public TestScene(GameState gameState) //DI 의존성 주입
//    {
//        _gameState = gameState;
        
//        EnemySpawner enemySpawner = new EnemySpawner();
//        _enemies.AddRange(enemySpawner.GetEnemies());  // EnemySpawner에서 적들을 _enemies에 추가
        
//        //플레이어,적 어택 액션에 서로의 TakeDamage등록
//        foreach (var enemy in _enemies)
//        {
//            _gameState.GetPlayer().AttackAction = (attacker, damage) =>
//            {
//                enemy.TakeDamage(damage); ;
//                var log = AttackLog(attacker, enemy, damage);   //Log에 AttackLog 추가
//                Utility.AddLog(log, ConsoleColor.Blue); //Log에 AttackLog 추가
//            };
            
//        }
//    }

//    public void Run()
//    {
//        Console.Clear(); //처음 진입시 화면 지우기

//        //예제 로직
//        // 현재 씬에 대한 이름 출력
//        Console.WriteLine("TestScene.\n");
        
//        foreach (var enemy in _enemies)
//        {
//            enemy.ShowInfo();  // 각 적의 정보를 출력
//        }
        
//        // 모든 Log 출력
//        Utility.PrintLogs();

//        _gameState.GetPlayer().Gold -= 100;
//    }

//    public IScene? GetNextScene()
//    {
//        int input = Utility.GetInput(1, 3);

//        switch (input)
//        {
//            case 1:
//                return this;  // 현재 씬 유지
//            case 2:
//                return new MainScene(_gameState);  // 다른 씬으로 전환
//            default:
//                return null;  // 잘못된 입력 시 종료
//        }
//    }

//    //공격 테스트
//    public void AttackTest(int input)
//    {
//        _gameState.GetPlayer().Attack(_enemies[input - 1]); //플레이어가 적을 때림

//    }

//    //Attack Log
//    private string AttackLog(ICharacter attacker, ICharacter target, float damage)
//    {
//        return $"{attacker.Name}가 {target.Name}에게 {damage} 피해를 입혔습니다!";
//    }

//}