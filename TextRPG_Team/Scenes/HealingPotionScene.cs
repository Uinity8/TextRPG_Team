using TextRPG_Team.Objects;

namespace TextRPG_Team.Scenes
{
    public class HealingPotionScene : IScene
    {
        private GameState _gameState;
        private Player _player;

        public HealingPotionScene(GameState gameState)
        {
            _gameState = gameState;
            _player = _gameState.Player;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"포션을 사용하면 체력을 30 회복할 수 있습니다. (남은 포션: {_player.Potion.Count})\n");
                Console.WriteLine("1. 사용하기");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    _player.UseHealingPotion();
                }
                else if (input == "0")
                {
                    Console.WriteLine("회복 아이템 메뉴에서 나갑니다.");
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                }

                Console.WriteLine("\n아무 키나 눌러 계속하기...");
                Console.ReadKey();
            }
        }

        public IScene? GetNextScene()
        {
            return null; //new InventoryScene(_gameState);
        }
    }


}

