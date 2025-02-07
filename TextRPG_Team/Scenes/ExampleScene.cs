// ExampleScene.cs 
// 씬 예제 파일 입니다. 새로운 씬 만들때 해당 파일 복사해서 이름만 바꾸세요!
namespace TextRPG_Team.Scenes;

public class ExampleScene : IScene
{
    private readonly GameState _gameState;

    public ExampleScene(GameState gameState) //DI 의존성 주입
    {
        _gameState = gameState;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        
        //예제 로직
        // 현재 씬에 대한 이름 출력
        Console.WriteLine("ExampleScene.");      
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch     //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            1 => this, // 같은 씬 유지
            2 => new MainScene(_gameState), // 메인 씬으로 돌아감
            _ => null // 잘못된 입력 시 종료
        };
    }
}