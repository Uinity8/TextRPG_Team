using TextRPG_Team.Manager;
using TextRPG_Team.Sound;

namespace TextRPG_Team;

using Scenes;

abstract class Program
{
    static Task Main()
    {
        // 사운드 매니저 초기화
        SoundManager.Initialize();
        

        // 게임 로직 실행
        LoadManager.LoadItems(); // 아이템 데이터 로드
        GameState gameState = new GameState(); // 인스턴스 생성

        // 첫 번째 씬 설정
        var initialScene = new TitleScene(gameState);
        var sceneManager = new SceneManager(initialScene);

        Console.WriteLine("[DEBUG] 게임 로직 실행...");
        sceneManager.StartGame();

        // 게임 종료 시 모든 사운드 리소스 해제
        Console.WriteLine("[DEBUG] 사운드 매니저 종료...");
        SoundManager.Shutdown();
        return Task.CompletedTask;
    }
}