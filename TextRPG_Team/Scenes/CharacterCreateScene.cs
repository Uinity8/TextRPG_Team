using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes
{
    public class CharacterCreateScene(
        GameState gameState,
        CharacterCreateScene.State state = CharacterCreateScene.State.Name)
        : IScene
    {
        public enum State
        {
            Name, // 기본 상태
            Job // 장착 관리 상태
        }

        // 현재 상태
        // 게임 상태 공유
        bool _isCancel;

        // 생성자 (DI 의존성 주입)

        public void Run()
        {
            Console.Clear();
            Console.WriteLine(new string('=',Utility.Width));
            Utility.AlignCenter("스파르타 던전에 오신 여러분 환영합니다.\n", ConsoleColor.Cyan);
            Console.WriteLine(new string('=',Utility.Width));
            Console.WriteLine();
            ShowScreen();
        }

        private void NameCreateScreen() // 이름 선택 화면
        {
            _isCancel = false;
            
            Utility.AlignCenter("원하시는 이름을 설정해주세요.\n");
            Utility.PrintLogs();
            string name = "";
            while (string.IsNullOrEmpty(name))
            {
                name = Console.ReadLine()?.Trim() ?? "";
            }
            
            gameState.Player.Name = name;

            if (name.Length > 8 || name.Length < 2 )
            {
                _isCancel = true;
                Utility.AddLog("한/영,숫자 2~8자리로 입력해주세요.\n", ConsoleColor.Red);
                return;
            }
                
                

            Console.WriteLine($"\n입력하신 이름은 {gameState.Player.Name} 입니다.\n");
            Console.WriteLine("1. 저장");
            Console.WriteLine("0. 취소\n");
        }

        private void JobCreateScreen() // 직업 선택 화면
        {
            Utility.AlignCenter($" {gameState.Player.Name}님 원하시는 직업을 설정해주세요.\n");
            Console.WriteLine();
            Console.WriteLine(new string('-',Utility.Width));
            Console.WriteLine(" 1. 계백수 [ HP : 120 / ATK : 8 / DEF : 10 / 시작골드 : 0 G ]");
            Utility.ColorWriteLine("    └ 말 그대롭니다. 백수 그 자체", ConsoleColor.DarkGray);
            Console.WriteLine(new string('-',Utility.Width));
            Console.WriteLine(" 2. 고3딩 [ HP : 100 / ATK : 10 / DEF : 5 / 시작골드 : 1500 G ]");
            Utility.ColorWriteLine("    └ 세상을 다 산듯한 얼굴 그렇습니다. 고3이네요.", ConsoleColor.DarkGray);
            Console.WriteLine(new string('-',Utility.Width));
            Console.WriteLine(" 3. 직딩 [ HP :  80 / ATK : 10 / DEF : 2 / 시작골드 : 10000 G ]");
            Utility.ColorWriteLine("    └ 4년차 직장인이라 그런지 눈에 안광이 없는거 같다.", ConsoleColor.DarkGray);
            Console.WriteLine(new string('-',Utility.Width));

        }

        private void ShowScreen() //화면 상태 전환
        {
            switch (state)
            {
                case State.Name:
                    NameCreateScreen();
                    break;
                case State.Job:
                    JobCreateScreen();
                    break;
            }
        }

        // 현재 상태에 따라 다음 씬 반환
        public IScene? GetNextScene()
        {
            if (_isCancel) return this;
            
            return state switch
            {
                State.Name => GetInputName(),
                State.Job => GetInputJob(),
                _ => null // 잘못된 입력 시 종료
            };
        }

        private IScene? GetInputName() // 이름 선택 입력
        {
            int input = Utility.GetInput(0, 1);
            return input switch
            {
                0 => this,
                1 => new CharacterCreateScene(gameState, State.Job),
                _ => null
            };
        }

        private IScene GetInputJob() // 직업 선택 입력
        {
            int input = Utility.GetInput(1, 3);
            Player player = gameState.Player;
            switch (input)
            {
                // (string name, Stats stats, int gold, string job)
                case 1:
                    player.Stats.MaxHp = 120;
                    player.Stats.Atk = 8;
                    player.Stats.Def = 10;
                    player.Gold = 0;
                    player.Job = "계백수";
                    player.Health = player.Stats.MaxHp;
                    break;
                case 2:
                    player.Stats.MaxHp = 100;
                    player.Stats.Atk = 10;
                    player.Stats.Def = 5;
                    player.Gold = 1500;
                    player.Job = "고3딩";
                    player.Health = player.Stats.MaxHp;
                    break;
                case 3:
                    player.Stats.MaxHp = 80;
                    player.Stats.Atk = 10;
                    player.Stats.Def = 2;
                    player.Gold = 10000;
                    player.Job = "직딩";
                    player.Health = player.Stats.MaxHp;
                    break;
                default: return this;
            }
            
            return new MainScene(gameState);
        }
    }
}