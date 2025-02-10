using TextRPG_Team.Manager;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes;

public class BattleScene : IScene
{
    public enum State
    {
        Default, // 기본 화면
        PlayerPhase, // 플레이어의 행동 차례
        PlayerResult, // 플레이어의 공격 결과
        EnemyPhase // 적의 차례
    }
    public enum Turn
    {
        Player,
        Enemy
    }

    static Random random = new Random();
    private Player player;
    private List<Enemy> battleEnemies = new List<Enemy>();
    private EnemySpawner enemySpawner = new EnemySpawner();
    private Turn currentTurn = Turn.Player;
    private State _state; // 현재 상태
    private readonly GameState _gameState; // 게임 상태 공유

    public BattleScene(GameState gameState, State state = State.Default)
    {
        _gameState = gameState;
        _state = state;
    }

    public void Run()
    {
        Console.Clear();
        ShowScreen();
    }

    public void PlayerInfo()
    {
        var player = _gameState.GetPlayer();
        Console.WriteLine("\n[내 정보]");
        Console.WriteLine(player.ToString());//Player클래스 ToString() 메서드 호출
    }

    private void ShowScreen()
    {
        Console.Clear();
        var enemyList = enemySpawner.GetEnemies();
        int enemyCount = random.Next(1, 5);  // 생성할 적의 수
        battleEnemies.Clear();

        for (int i = 0; i < enemyCount; i++)
        {
            var enemy = enemyList[random.Next(enemyList.Count)];
            battleEnemies.Add(new Enemy(enemy.Name, enemy.GetStats(), enemy.GetStats().Lv));
        }

        DisplayBattleState();
        NextTurn();
    }

    private void NextTurn()
    {
        if (battleEnemies.All(m => m.IsDead()))
        {
            DisplayBattleResult();
            return;
        }

        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
            PlayerTurn();
        }
        else
        {
            Console.Clear();
            currentTurn = Turn.Player;
            EnemyTurn();
        }
    }

    private void DisplayBattleState()
    {
        Utility.ColorWriteLine("\nBattle!", ConsoleColor.Yellow);

        foreach (var enemy in battleEnemies)
        {
            string status = enemy.IsDead() ? "Dead" : $"HP {enemy.Health}";
            ConsoleColor textColor = enemy.IsDead() ? ConsoleColor.DarkGray : ConsoleColor.White;
            Utility.ColorWriteLine($"{enemy.Name} Lv.{enemy.GetStats().Lv} {status}", textColor);
        }

        PlayerInfo();
    }

    private void DisplayBattleResult()
    {
        int deadEnemies = battleEnemies.Count(m => m.IsDead());
        Console.Clear();
        Utility.ColorWriteLine($"Battle! - Result", ConsoleColor.DarkCyan);
        Utility.ColorWriteLine("Victory", ConsoleColor.Green);

        var player = _gameState.GetPlayer();
        Console.WriteLine($"Lv.{player.GetStats().Lv} {player.Name}");
        Console.WriteLine($"HP {player.GetStats().MaxHp} -> {player.Health}");
        Console.WriteLine($"몬스터 {deadEnemies}마리 잡았음");

        Console.WriteLine("\n0. 다음");
        while (Console.ReadLine() != "0") { }
    }

    private void EnemyTurn()
    {
        Utility.ColorWriteLine("\nEnemy Turn!", ConsoleColor.Red);

        float oldHp = _gameState.GetPlayer().Health;

        foreach (var enemy in battleEnemies)
        {
            if (enemy.IsDead()) continue;
            float damage = enemy.Power;
            _gameState.GetPlayer().TakeDamage(damage);
            Console.WriteLine($"Lv.{enemy.GetStats().Lv} {enemy.Name} 의 공격! [데미지: {damage}]");
        }

        Console.WriteLine($"Lv.{_gameState.GetPlayer().GetStats().Lv} {_gameState.GetPlayer().Name} HP {oldHp} -> {_gameState.GetPlayer().Health}");

        if (_gameState.GetPlayer().IsDead())
        {
            Utility.ColorWriteLine("\n당신은 쓰러졌습니다...", ConsoleColor.Red);
            return;
        }

        Console.WriteLine("\n0. 다음");
        while (Console.ReadLine() != "0") { }

        DisplayBattleState();
        NextTurn();
    }

    private void PlayerTurn()
    {
        while (true)
        {
            Console.WriteLine("0. 취소\n");
            Console.Write("대상을 선택해주세요.\n>> ");
    
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0) return; // 취소 시 돌아가기
    
                int index = choice - 1;
                if (index >= 0 && index < battleEnemies.Count)
                {
                    Enemy target = battleEnemies[index];
                    if (target.IsDead())
                    {
                        Console.WriteLine("이미 죽은 적입니다.\n");
                        continue;
                    }
    
                    AttackEnemy(target); // 적 공격
                    break; // 공격 후 종료
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.\n");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 숫자를 입력하세요.\n");
            }
        }
    }

    private void AttackEnemy(Enemy target)
    {
        Console.Clear();
        var player = _gameState.GetPlayer();  // player 객체를 여기에 할당
        float attackPower = player.Power;     // 공격력
        float oldHP = target.Health;         // 공격 전 적 HP
    
        target.TakeDamage(attackPower); // 적에게 데미지 주기
    
        Console.WriteLine($"\n{player.Name} 의 공격!");
        Console.WriteLine($"Lv.{target.GetStats().Lv} {target.Name} 을(를) 맞췄습니다. [데미지 : {attackPower}]");
        Console.WriteLine($"Lv.{target.GetStats().Lv} {target.Name} HP {oldHP} -> {(target.IsDead() ? "Dead" : target.Health.ToString())}\n");
    
        Console.WriteLine("0. 다음");
        while (Console.ReadLine() != "0") { } // 0을 입력할 때까지 기다리기
    
        DisplayBattleState();  // 배틀 화면 갱신
        NextTurn();  // 턴 전환
    }
public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch
        {
            1 => this,
            2 => new MainScene(_gameState),
            _ => null // 잘못된 입력 시 종료
        };
    }
}
