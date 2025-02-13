namespace TextRPG_Team.Scenes;

public class SceneManager(IScene startScene)
{
    private IScene? _currentScene = startScene; // 초기 씬 설정

    public void StartGame()
    {
        while (_currentScene != null)
        {
            _currentScene.Run(); // 현재 씬 실행

            // 다음 씬으로 이동
            _currentScene = _currentScene.GetNextScene();
        }

        Console.WriteLine("Game Over.");
    }
}
