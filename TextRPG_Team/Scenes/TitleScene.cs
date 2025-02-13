
using TextRPG_Team.Manager;

namespace TextRPG_Team.Scenes;

public class TitleScene(GameState gameState) : IScene
{
    private bool _hasPlayData;
    //DI 의존성 주입

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        
        _hasPlayData = LoadManager.HasPlayData();
        if (_hasPlayData)
        {
            Console.WriteLine(" 저장 파일이 있습니다 이어 하시겠습니까?");
            Console.WriteLine();
            Console.WriteLine(" 1. 이어하기");
            Console.WriteLine(" 2. 새로하기");
        }
        Console.WriteLine();
    }
    
    public IScene? GetNextScene()
    {
        if (!_hasPlayData)
            return new CharacterCreateScene(gameState);
        
        int input = Utility.GetInput(1, 2);
        switch (input)
        {
            case 1:
                GameState loadState = LoadManager.LoadGameData(); //플레이어 데이터 로드
                return new MainScene(loadState); // 메인 씬으로 돌아감
            case 2:
                return new CharacterCreateScene(gameState); // 캐릭터 생성씬
        }
        return null;
    }
}