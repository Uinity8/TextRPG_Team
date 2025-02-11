namespace TextRPG_Team;

using Scenes;
class Program
{
    static void Main(string[] args)
    {
        GameState gameState = new GameState(); //인스턴스 생성
        
        var initialScene = new CharacterCreateScene(gameState); // 첫 번째 씬 설정

        var sceneManager = new SceneManager(initialScene);
        
        sceneManager.StartGame(); // 게임 시작

    }
}