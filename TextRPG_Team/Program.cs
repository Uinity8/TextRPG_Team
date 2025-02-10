namespace TextRPG_Team;
using Scenes;
class Program
{
    static void Main(string[] args)
    {
        GameState gameState = new GameState(); //인스턴스 생성
        //임시코드
        Item item = new Item("아이템", ItemType.Armor, 10, "아이템입니다.", 0);
        gameState.Player.BuyItem(item);
        
        var initialScene = new MainScene(gameState); // 첫 번째 씬 설정

        var sceneManager = new SceneManager(initialScene);
        
        sceneManager.StartGame(); // 게임 시작

    }
}