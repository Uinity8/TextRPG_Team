using TextRPG_Team.Manager;

namespace TextRPG_Team;

using Scenes;
class Program
{
    static void Main(string[] args)
    {

        LoadManager.LoadItems();///아이템 데이터 로드
        
        GameState gameState = new GameState(); //인스턴스 생성
        
        // 첫 번째 씬 설정
        var initialScene = new TitleScene(gameState);

        var sceneManager = new SceneManager(initialScene);
        
        sceneManager.StartGame(); // 게임 시작


    }
}