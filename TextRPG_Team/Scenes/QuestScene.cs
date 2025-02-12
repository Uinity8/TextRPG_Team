using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Team.Scenes;
using TextRPG_Team;
using TextRPG_Team.Objects;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel;

public class QuestScene : IScene
{
    public enum State
    {
        Default,
        Accep,
        Compensation

    }
    
    State _state;
    private readonly GameState _gameState;
    private string _strTitle = "";
    private int Select;

    public QuestScene(GameState gameState, State state = State.Default, int select = 0) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state; 
        Select = select;

        switch (_state)
        {
            case State.Default:
                _strTitle = "[튜토리얼 퀘스트]\n";
                break;
            case State.Accep:
                _strTitle = "[ 퀘스트를 수행하시겠습니까? Y / N ]\n";
                break;
            case State.Compensation:
                _strTitle = "[ 퀘스트를 완료하셨습니다. ]\n";
                break;
        }
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        ShowScreen();
    }

    private void ShowScreen() //화면 상태 전환
    {
        Console.WriteLine(new string('=', Utility.Width));
        Utility.AlignCenter("Quest!!\n", ConsoleColor.DarkCyan);
        Utility.AlignCenter(_strTitle);
        Console.WriteLine(new string('=', Utility.Width) + "\n");

        switch (_state)
        {
            case State.Default:
                DefaultScreen();
                break;
            case State.Accep:
                AccepScreen();
                break;
            case State.Compensation:
                CompensationScreen();
                break;
        };
    }

    void DefaultScreen() // 퀘스트씬 기본 화면
    {
        foreach (var quest in _gameState.QuestList)
        {
            Console.WriteLine($"- Q{_gameState.QuestList.FindIndex(i => i.Id == quest.Id)+1}. {quest.Name}\n");
        }
        Console.WriteLine(new string('=', Utility.Width) + "\n");
        Console.WriteLine(" 0. 나가기");
        Utility.PrintLogs();
        Console.WriteLine(new string('=', Utility.Width) + "\n");

    }
    void AccepScreen() // 퀘스트 수락 스크린
    {
        PrintScene();
        Console.Write(" 1. 수락");
        Console.WriteLine(" 2. 거절");
        Utility.PrintLogs();
        Console.WriteLine(new string('=', Utility.Width) + "\n");
    }
    void CompensationScreen() // 퀘스트 완료 스크린
    {
        PrintScene();
        Console.Write(" 1. 보상 받기");
        Console.WriteLine(" 2. 돌아가기");
        Utility.PrintLogs();
        Console.WriteLine(new string('=', Utility.Width) + "\n");
    }
    void PrintScene() // 퀘스트 출력
    {
        var quest= _gameState.QuestList;
        Console.WriteLine($"{quest[Select].Name}\n");
        Console.Write($"{quest[Select].Info}");
        if (!quest[Select].Clear) Utility.AlignRight("N / Y\n", Utility.Width-15);
        else Utility.AlignRight("Y / Y\n", Utility.Width-15);
        Utility.AlignCenter("- 보상 -\n");
        Utility.AlignCenter($"{quest[Select].Compensation}G\n");
        Console.WriteLine(new string('=', Utility.Width) + "\n");
    }

    public IScene? GetNextScene()
    {
        return _state switch     //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            State.Default => GetInputForDefault(),
            State.Accep => GetInputForAccep(),
            State.Compensation => GetInputForAccepCompensation(),
            _ => null
        };
    }
    private IScene? GetInputForDefault() // 기본화면 입력
    {
        int input = Utility.GetInput(0, _gameState.QuestList.Count, " 원하시는 행동을 입력해주세요.");
        switch (input)
        {
            case 0:
                return new MainScene(_gameState);
            default:
                if (_gameState.QuestList[input - 1].Acquisition) 
                {
                    Utility.AddLog($"\n{_gameState.QuestList[input - 1].Name}은 이미 클리어한 퀘스트입니다\n",ConsoleColor.Red);
                    return new QuestScene(_gameState);
                }
                if(_gameState.QuestList[input - 1].Accep)ClearCheck(input);
                if (!_gameState.QuestList[input - 1].Clear ) return new QuestScene(_gameState, State.Accep,input-1);
                else return new QuestScene(_gameState, State.Compensation,input-1);
        }
    }

    private IScene? GetInputForAccep() // 수락화면 입력
    {
        int input = Utility.GetInput(1, 2, " 원하시는 행동을 입력해주세요.");
        switch (input)
        {
            case 1:
                AccepCheck();
                return new QuestScene(_gameState,State.Accep, Select);
            default:
                return new QuestScene(_gameState,State.Default, Select);
        }
    }

    private IScene? GetInputForAccepCompensation() // 보상화면 입력
    {
        int input = Utility.GetInput(1, 2, " 원하시는 행동을 입력해주세요.");
        switch (input)
        {
            case 1:
                CompensationCheck();
                return new QuestScene(_gameState, State.Compensation, Select);
            default:
                return new QuestScene(_gameState, State.Default,Select);
        }
    }

    public void AccepCheck() // 수락 확인
    {
        if (_gameState.QuestList[Select].Accep)
        {
            Utility.AddLog("\n이미 수락한 퀘스트입니다.\n", ConsoleColor.Red);
            return;
        }
        Utility.AddLog("\n퀘스트를 수락하셨습니다.\n", ConsoleColor.Blue);
        _gameState.QuestList[Select].Accep = !_gameState.QuestList[Select].Accep;
    }

    public void ClearCheck(int input) // 클리어 확인
    {
        switch (input)
        {
            case 1:
                break;    
            case 2:
                for(int i = 0; i < _gameState.Player.Inventory.Count; i++)
                {
                    if (_gameState.Player.Inventory[i].itemEquip)
                        _gameState.QuestList[Select+1].Clear = true;
                }
                break;
            case 3: 
                break;
            default: 
                break;
        }
    }

    public void CompensationCheck() // 보상 확인
    {
        if (_gameState.QuestList[Select].Acquisition)
        {
            Utility.AddLog("\n이미 수령한 보상입니다.\n", ConsoleColor.Red);
            return;
        }
        Utility.AddLog($"{_gameState.QuestList[Select].Name} 퀘스트 클리어 ! .\n", ConsoleColor.Blue);
        Utility.AddLog("보상을 획득했습니다.\n", ConsoleColor.Blue);
        Utility.AddLog($"{_gameState.QuestList[Select].Compensation}G \n", ConsoleColor.Yellow);
        _gameState.Player.Gold += _gameState.QuestList[Select].Compensation;
        _gameState.QuestList[Select].Acquisition = !_gameState.QuestList[Select].Acquisition;
    }
}
