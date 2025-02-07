// ExampleScene.cs 
// 씬 예제 파일 입니다. 새로운 씬 만들때 해당 파일 복사해서 이름만 바꾸세요!

namespace TextRPG_Team.Scenes;

using Objects;

public class TestScene : IScene
{
    private readonly GameState _gameState;
    private readonly Player _player = new Player("Payer1", 100f, 10f);
    readonly List<Player> _enemies = new List<Player>(); //예제를 위해 플레이어 클래스로 적들 생성
    
    public TestScene(GameState gameState) //DI 의존성 주입
    {
        _gameState = gameState;
        
        //테스트 적들 추가
        _enemies.Add(new Player("빵빵이", 10, 1));
        _enemies.Add(new Player("옥지", 10, 1));
        
        
        //플레이어,적 어택 액션에 서로의 TakeDamage등록
        foreach (var enemy in _enemies)
        {
            _player.AttackAction = (attacker, damage) =>
            {
                enemy.TakeDamage(damage); ;
                var log = AttackLog(attacker, enemy, damage);   //Log에 AttackLog 추가
                _gameState.Logs.Enqueue(log); //Log에 AttackLog 추가
            };
            //enemy.AttackAction = (attacker, damage) => { _player.TakeDamage(damage); };
        }
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기

        //예제 로직
        // 현재 씬에 대한 이름 출력
        Console.WriteLine("TestScene.\n");

        Console.WriteLine("1. 빵빵이");
        Console.WriteLine("2. 옥지");
        Console.WriteLine("");
        
        // 모든 Log 출력
        // 이거 모든 씬에서 써야할것 같은데 Utility에 옮겨도 OK?
        for (int i = 0; i < _gameState.Logs.Count(); i++)
        {
            Utility.ColorWriteLine(_gameState.Logs.Dequeue(), ConsoleColor.Blue);
        }
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);

        switch (input)
        {
            case 1:
            case 2:
                AttackTest(input);
                return this;
                break;
        }

        return null;
    }

    //공격 테스트
    public void AttackTest(int input)
    {
        _player.Attack(_enemies[input - 1]); //플레이어가 적을 때림

        //int rand = new Random().Next(0, 2); // 0~1 사이의 랜덤 값
        // _enemies[rand].Attack(_player); //옥지나 빵빵이가 랜덤으로 플레이어를 때림
    }

    //Attack Log
    private string AttackLog(ICharacter attacker, ICharacter target, float damage)
    {
        return $"{attacker.Name}가 {target.Name}에게 {damage} 피해를 입혔습니다!";
    }

}