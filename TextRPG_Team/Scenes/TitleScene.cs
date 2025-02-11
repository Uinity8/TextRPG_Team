
using TextRPG_Team.Manager;

namespace TextRPG_Team.Scenes;

public class TitleScene : IScene
{
    private readonly GameState _gameState;
    
    private bool _hasPlayData;
    public TitleScene(GameState gameState) //DI 의존성 주입
    {
        _gameState = gameState;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        
        _hasPlayData = LoadManager.HasPlayData();
        if (_hasPlayData)
        {
            Console.WriteLine("저장 파일이 있습니다 이어 하시겠습니까?");
            Console.WriteLine();
            Console.WriteLine(" 1. 이어하기");
            Console.WriteLine(" 2. 새로하기");
        }
    }
    
    public IScene? GetNextScene()
    {
        if (!_hasPlayData)
            return new CharacterCreateScene(_gameState);
        
        int input = Utility.GetInput(1, 2);
        return input switch     
        {
            1 => new MainScene(_gameState), // 메인 씬으로 돌아감
            2 => new CharacterCreateScene(_gameState), // 캐릭터 생성씬
            _ => null // 잘못된 입력 시 종료
        };
    }
}