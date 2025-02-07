namespace TextRPG_Team.Scenes;
using System.Collections.Generic;

public class BattleScene : IScene
{
    static Random random = new Random();
    private readonly GameState _gameState;
    private List<Monster> battleMonsters = new List<Monster>();
    private Player player = new Player("Chad", "전사", 1, 100, 10);

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

        // 🔹 HP를 변경하는 메서드 추가
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

    public BattleScene(GameState gameState)//파라미터 추가해서 true 플레이어턴 false 적턴 or Enum 사용
    {
        _gameState = gameState;
    }

    public void Run()
    {
        Console.Clear();

        Console.WriteLine("배틀 씬 입니다");
        Console.WriteLine("1. 유지");
        Console.WriteLine("2. 배틀");

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
        PlayerTurn();
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

private void EnemyPhase()
{
    Console.Clear();
    Utility.ColorWriteLine("\nEnemy Phase!", ConsoleColor.Red);

    foreach (var monster in battleMonsters)
    {
        if (monster.IsDead) continue;
        
        int damage = monster.Attack;
        player.Hp -= damage;
        
        Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격! [데미지: {damage}]");
    }

    if (player.Hp <= 0)
    {
        Utility.ColorWriteLine("\n당신은 쓰러졌습니다...", ConsoleColor.Red);
        return;
    }

    Console.WriteLine("\n0. 다음");
    while (Console.ReadLine() != "0") { }

    DisplayBattleState();
    PlayerTurn();
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
    int attackPower = ApplyAttackVariation(player.Attack);
    int oldHP = target.HP;
    target.TakeDamage(attackPower);

    Console.WriteLine("\nChad 의 공격!");
    Console.WriteLine($"Lv.{target.Level} {target.Name} 을(를) 맞췄습니다. [데미지 : {attackPower}]");
    Console.WriteLine($"Lv.{target.Level} {target.Name}");
    Console.WriteLine($"HP {oldHP} -> {(target.IsDead ? "Dead" : target.HP.ToString())}\n");

    Console.WriteLine("0. 다음");
    while (Console.ReadLine() != "0") { }

    DisplayBattleState();
    EnemyPhase();
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
