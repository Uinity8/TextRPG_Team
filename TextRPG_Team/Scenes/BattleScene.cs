namespace TextRPG_Team.Scenes;
using System.Collections.Generic;
using System.Linq;
using TextRPG_Team.Objects;

public enum Turn
{
    Player,
    Enemy
}

public class BattleScene : IScene
{
    static Random random = new Random();
    private readonly GameState _gameState;
    private List<Monster> battleMonsters = new List<Monster>();
    private Player player = new Player("Chad", "전사", 1, 100, 10);
    private Turn currentTurn = Turn.Player;

    class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int MaxHp { get; }
        public int Attack { get; }
        public int HP { get; private set; }
        public bool IsDead => HP <= 0;

        public Monster(string name, int level, int hp, int attack)
        {
            Name = name;
            Level = level;
            MaxHp = hp;
            HP = hp;
            Attack = attack;
        }

        public void TakeDamage(int damage)
        {
            HP = Math.Max(0, HP - damage);
        }

        public override string ToString()
        {
            string status = IsDead ? "Dead" : $"HP {HP}";
            return $"Lv.{Level} {Name} {status}";
        }
    }

    public class Player
    {
        public string Name { get; }
        public string Class { get; }
        public int Level { get; }
        public int Hp { get; set; }
        public int MaxHp { get; }
        public int Attack { get; }

        public Player(string name, string playerClass, int level, int hp, int attack)
        {
            Name = name;
            Class = playerClass;
            Level = level;
            Hp = hp;
            MaxHp = hp;
            Attack = attack;
        }

        public void PlayerInfo()
        {
            Console.WriteLine("\n[내정보]");
            Console.WriteLine($"Lv.{Level} {Name} {Class}");
            Console.WriteLine($"HP {Hp}/{MaxHp}");
            Console.WriteLine($"power {Attack}");
        }
    }

    public BattleScene(GameState gameState)
    {
        _gameState = gameState;
    }

    public void Run()
    {
        Console.Clear();
        List<Monster> monsterPool = new List<Monster>
        {
            new Monster("미니언", 2, 15, 5),
            new Monster("공허충", 3, 10, 9),
            new Monster("대포미니언", 5, 25, 8),
        };

        int monsterCount = random.Next(1, 5);
        battleMonsters.Clear();

        for (int i = 0; i < monsterCount; i++)
        {
            battleMonsters.Add(new Monster(
                monsterPool[random.Next(monsterPool.Count)].Name,
                monsterPool[random.Next(monsterPool.Count)].Level,
                monsterPool[random.Next(monsterPool.Count)].MaxHp,
                monsterPool[random.Next(monsterPool.Count)].Attack
            ));
        }

        DisplayBattleState();
        NextTurn();
    }

    private void NextTurn()
    {
         // 모든 몬스터가 죽었는지 확인
        if (battleMonsters.All(m => m.IsDead))
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

        for (int i = 0; i < battleMonsters.Count; i++)
        {
            var monster = battleMonsters[i];
            string status = monster.IsDead ? "Dead" : $"HP {monster.HP}";

            ConsoleColor textColor = monster.IsDead ? ConsoleColor.DarkGray : ConsoleColor.White;

            Utility.ColorWriteLine($"{i + 1} Lv.{monster.Level} {monster.Name} {status}", textColor);
        }
    }

    private void DisplayBattleResult()
    {
        int deadMonsters = battleMonsters.Count(m => m.IsDead); // 죽은 몬스터 수 계산
        Console.Clear();
        Utility.ColorWriteLine($"Battle! - Result", ConsoleColor.DarkCyan);
        Utility.ColorWriteLine("Victory", ConsoleColor.Green);

        Console.WriteLine($"Lv.{player.Level} {player.Name}");
        Console.WriteLine($"HP {player.MaxHp} -> {player.Hp}");
        Console.WriteLine($"몬스터 {deadMonsters}마리 잡았음");

        Console.WriteLine("\n0. 다음");
        while (Console.ReadLine() != "0") { }
    }

    private void EnemyTurn()
    {
        Utility.ColorWriteLine("\nEnemy Turn!", ConsoleColor.Red);

        int oldHp = player.Hp;

        foreach (var monster in battleMonsters)
        {
            if (monster.IsDead) continue;

            // 적이 플레이어에게 공격
            int damage = monster.Attack;
            player.Hp -= damage;

            // 공격 후 출력
            Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격! [데미지: {damage}]");
        }

        // 플레이어 HP가 갱신된 후 출력
        Console.WriteLine($"Lv.{player.Level} {player.Name} HP {oldHp} -> {player.Hp}");

        // 플레이어가 쓰러졌다면 전투 종료
        if (player.Hp <= 0)
        {
            Utility.ColorWriteLine("\n당신은 쓰러졌습니다...", ConsoleColor.Red);
            return; // 전투 종료
        }

        Console.WriteLine("\n0. 다음");
        while (Console.ReadLine() != "0") { }

        // 전투 상태 출력
        DisplayBattleState();
        NextTurn();
    }

    private void PlayerTurn()
    {
        while (true)
        {
            player.PlayerInfo();
            Console.WriteLine("0. 취소\n");
            Console.Write("대상을 선택해주세요.\n>> ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0) return;

                int index = choice - 1;
                if (index >= 0 && index < battleMonsters.Count)
                {
                    Monster target = battleMonsters[index];
                    if (target.IsDead)
                    {
                        Console.WriteLine("잘못된 입력입니다.\n");
                        continue;
                    }

                    AttackMonster(target);
                    break;
                }
            }
        }
    }

    private void AttackMonster(Monster target)
    {
        Console.Clear();
        int attackPower = ApplyAttackVariation(player.Attack);
        int oldHP = target.HP;
        target.TakeDamage(attackPower);

        Console.WriteLine("\nChad 의 공격!");
        Console.WriteLine($"Lv.{target.Level} {target.Name} 을(를) 맞췄습니다. [데미지 : {attackPower}]");
        Console.Write($"Lv.{target.Level} {target.Name}");
        Console.WriteLine($"HP {oldHP} -> {(target.IsDead ? "Dead" : target.HP.ToString())}\n");

        Console.WriteLine("0. 다음");
        while (Console.ReadLine() != "0") { }

        DisplayBattleState();
        NextTurn();
    }

    private int ApplyAttackVariation(int baseAttack)
    {
        double variation = baseAttack * 0.1;
        int minAttack = (int)Math.Ceiling(baseAttack - variation);
        int maxAttack = (int)Math.Ceiling(baseAttack + variation);
        return random.Next(minAttack, maxAttack + 1);
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch
        {
            1 => this,
            2 => new ExampleScene(_gameState),
            _ => null
        };
    }
}
