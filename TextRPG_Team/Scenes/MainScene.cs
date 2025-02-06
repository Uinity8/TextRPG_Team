namespace TextRPG_Team.Scenes;

public class MainScene : IScene
{
    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        
        //예제 코드
        Console.WriteLine("1.Main Scene");      
        Console.WriteLine("2.Example Scene");           
    }

    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch
        {
            1 => this,
            2 => new ExampleScene(),
            _ => null // 잘못된 입력 시 종료
        };
    }
}

