using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes
{
    public class CharacterCreateScene : IScene
    {
        public enum State
        {
            Name, // 기본 상태
            Job // 장착 관리 상태
        }

        private State _state; // 현재 상태
        private readonly GameState _gameState; // 게임 상태 공유

        // 생성자 (DI 의존성 주입)
        public CharacterCreateScene(GameState gameState, State state = State.Name)
        {
            _gameState = gameState;
            _state = state;
        }

        public void Run()
        {
            Console.Clear();
            Utility.ColorWriteLine("스파르타 던전에 오신 여러분 환영합니다.", ConsoleColor.Yellow);
            ShowScreen();
        }

        private void NamecreateScreen() // 이름 선택 화면
        {
            Console.WriteLine("원하시는 이름을 설정해주세요.\n");
            string name = "";
            while (string.IsNullOrEmpty(name))
            {
                name = Console.ReadLine() ?? "";
            }
            _gameState.Player.Name = name;

            Console.WriteLine($"\n입력하신 이름은 {_gameState.Player.Name} 입니다.\n");
            Console.WriteLine("1. 저장");
            Console.WriteLine("0. 취소\n");
        }
        private void JobcreateScreen() // 직업 선택 화면
        {
            Console.WriteLine($"{_gameState.Player.Name}님 원하시는 직업을 설정해주세요.\n");
            Console.WriteLine("1. 계백수");
            Console.WriteLine("2. 고딩");
            Console.WriteLine("2. 직딩\n");
        }
        private void ShowScreen() //화면 상태 전환
        {
            switch (_state)
            {
                case State.Name:
                    NamecreateScreen();
                    break;
                case State.Job:
                    JobcreateScreen();
                    break;
            }
        }

        // 현재 상태에 따라 다음 씬 반환
        public IScene? GetNextScene()
        {
            return _state switch
            {
                State.Name => GetInputName(),
                State.Job => GetInputJob(),
                _ => null // 잘못된 입력 시 종료
            };
        }
        private IScene? GetInputName() // 이름 선택 입력
        {
            int input = Utility.GetInput(0, 2);
            return input switch
            {
                0 => this,
                1 => new CharacterCreateScene(_gameState, State.Job),
                _ => null
            };
        }
        private IScene? GetInputJob() // 직업 선택 입력
        {
            int input = Utility.GetInput(1, 3);
            Player player = _gameState.Player;
            switch (input)
            {// (string name, Stats stats, int gold, string job)
                case 1:
                    player._stats.MaxHp = 120;
                    player._stats.Atk = 8;
                    player._stats.Def = 10;
                    player.Gold = 0;
                    player.Job = "계백수";
                    player.Health = player._stats.MaxHp;
                    break;
                case 2:
                    player._stats.MaxHp = 100;
                    player._stats.Atk = 10;
                    player._stats.Def = 5;
                    player.Gold = 1500;
                    player.Job = "고딩";
                    player.Health = player._stats.MaxHp;
                    break;
                case 3:
                    player._stats.MaxHp = 80;
                    player._stats.Atk = 10;
                    player._stats.Def = 2;
                    player.Gold = 10000;
                    player.Job = "직딩";
                    player.Health = player._stats.MaxHp;
                    break;
                default: return this;
            };
            return new MainScene(_gameState);
        }
    }
}
