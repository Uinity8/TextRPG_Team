namespace TextRPG_Team;
using Scenes;
class Program
{
    static void Main(string[] args)
    {
        var initialScene = new MainScene(); // 첫 번째 씬 설정

        var sceneManager = new SceneManager(initialScene);
        sceneManager.StartGame(); // 게임 시작

    }
}