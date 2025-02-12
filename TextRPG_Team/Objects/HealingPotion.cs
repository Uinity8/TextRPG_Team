namespace TextRPG_Team.Objects;


public class HealingPotion
{
    public const string PotionName = "힐링 포션"; // 포션 이름 상수화
    public const int DefaultRecoveryAmount = 30; // 기본 회복량 상수화

    public string Name { get; } = PotionName;
    public int RecoveryAmount { get; } = DefaultRecoveryAmount;
    public int Count { get; set; } = 3;

    public void UsePotion(Player player)
    {
        if (Count <= 0)
        {
            Console.WriteLine("포션이 부족합니다.");
            return;
        }

        if (player.Health >= player.GetStats.MaxHp)
        {
            Console.WriteLine("이미 체력이 가득 찼습니다.");
            return;
        }

        float healAmount = Math.Min(RecoveryAmount, player.GetStats.MaxHp - player.Health);
        player.Health += healAmount; // 프로퍼티 값을 직접 수정
        Count--;

        Console.WriteLine($"포션을 사용하여 체력을 {healAmount} 회복했습니다. (남은 포션: {Count})");
    }

}

