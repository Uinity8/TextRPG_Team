namespace TextRPG_Team.Objects


{
    public class HealingPotion
    {
        public string Name { get; } = "힐링 포션"; // 포션 이름
        public int RecoveryAmount { get; } = 30;  // 회복량
        public int Count { get; set; } = 3;       // 기본 보유 개수
        private Player _player;  // 플레이어 참조 체력 업데이트를 위해 추가

        public HealingPotion(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player)); //  Null 체크
        }

        public void UsePotion()
        {
            if (Count <= 0)
            {
                Console.WriteLine("포션이 부족합니다.");
                return;
            }

            if (_player.Health >= _player.GetStats.MaxHp)
            {
                Console.WriteLine("이미 체력이 가득 찼습니다.");
                return;
            }

            float healAmount = Math.Min(RecoveryAmount, _player.GetStats.MaxHp - _player.Health);
            _player.Heal(healAmount);  // Player 클래스의 Heal() 메서드를 호출하여 체력 회복
            Count--;

            Console.WriteLine($"포션을 사용하여 체력을 {healAmount} 회복했습니다. (남은 포션: {Count})");
        }

        public void DisplayPotionInfo()
        {
            Console.WriteLine($"현재 보유 포션: {Count}");
        }
    }


}
