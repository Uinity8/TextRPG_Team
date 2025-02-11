using TextRPG_Team.Manager;

namespace TextRPG_Team;

using Scenes;
class Program
{
    static void Main(string[] args)
    {            
        //LoadManager.DeletePlayerData(); 플레이어 데이터 삭제
        
        GameState gameState = new GameState(); //인스턴스 생성
        
        // 첫 번째 씬 설정
        IScene initialScene = new CharacterCreateScene(gameState);
        
        //데이터 파일이 있으면 MainScene
        if (LoadManager.HasPlayData())
        {
            gameState.Player = LoadManager.LoadPlayerData();
            initialScene = new MainScene(gameState);
        }

        var sceneManager = new SceneManager(initialScene);
        
        sceneManager.StartGame(); // 게임 시작

    }
}