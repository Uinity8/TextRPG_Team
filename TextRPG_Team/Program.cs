using TextRPG_Team.Manager;

namespace TextRPG_Team;

using Scenes;

abstract class Program
{
    static void Main()
    {
        LoadManager.LoadItems(); //아이템 데이터 로드
        
        GameState gameState = new GameState(); //인스턴스 생성
        //LoadManager.SaveItemsData(gameState.ItemList);//아이템 데이터 로드
        
        // 첫 번째 씬 설정
        var initialScene = new TitleScene(gameState);

        var sceneManager = new SceneManager(initialScene);
        
        sceneManager.StartGame(); // 게임 시작


    }
}