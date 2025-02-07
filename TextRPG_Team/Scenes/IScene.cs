namespace TextRPG_Team.Scenes;

// 씬 인터페이스: 씬 실행 및 다음 씬 반환
public interface IScene
{
    // 씬을 실행
    void Run();

    // 다음 씬을 반환 (null이면 종료)
    IScene? GetNextScene();
}
