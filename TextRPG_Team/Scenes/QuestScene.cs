using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Team.Scenes;
using TextRPG_Team;

public class QuestScene : IScene
{
    public enum State
    {
        Default,
        Select,
        KillQuest,
        EquipQuest,
        SkillQuest
    }
    private readonly GameState _gameState;
    State _state;
    bool AccepChk = false;

    public QuestScene(GameState gameState, State state = State.Default) //DI 의존성 주입
    {
        _gameState = gameState;
        _state = state;
    }

    public void Run()
    {
        Console.Clear(); //처음 진입시 화면 지우기
        Console.WriteLine("Quest!!");
        ShowScreen();
    }

    private void ShowScreen() //화면 상태 전환
    {
        switch (_state)
        {
            case State.Default:
                DefaultScreen();
                break;
            case State.KillQuest:
                KillQuestScreen(AccepChk);
                break;
            case State.EquipQuest:
                EquipQuestScreen(AccepChk);
                break;
            case State.SkillQuest:
                SkillQuestScreen(AccepChk);
                break;
        };
    }

    void DefaultScreen()
    {
        Console.WriteLine("\n1. 마을을 위협하는 미니언 처치");
        Console.WriteLine("2. 전용 장비를 장착해보자");
        Console.WriteLine("3. 더욱 더 강해지기!\n");

    }
    void KillQuestScreen(bool Accep)
    {
        Console.WriteLine("마을을 위협하는 미니언 처치\n");
        Console.WriteLine("이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?");
        Console.WriteLine("마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!");
        Console.WriteLine("모험가인 자네가 좀 처치해주게!\n");
        Console.WriteLine("- 미니언 5마리 처치 ({killcount}/5)\n");
        Console.WriteLine("- 보상 - ");
        Console.WriteLine("보상");
        Console.WriteLine("500G\n");
        if (Accep)
        {
            Console.WriteLine("퀘스트를 수락하셨습니다.");
        }
        else if (!Accep)
        {
            Console.WriteLine("1. 수락");
            Console.WriteLine("2. 거절\n");
        }
        else
        {
            Console.WriteLine("1. 보상 받기");
            Console.WriteLine("2. 돌아가기\n");
        }


    }
    void EquipQuestScreen(bool Accep)
    {
        Console.WriteLine("장비를 장착해보자\n");
        Console.WriteLine($"{_gameState.Player.Name} 너에게 맞는 장비를 장착해봐!");
        Console.WriteLine("직업 장비를 장착하면 더욱 강해질 수 있어!\n");
        Console.WriteLine("- 전용장비 장착 ({f/t)\n");
        Console.WriteLine("- 보상 - ");
        Console.WriteLine("공격력 + 5");
        Console.WriteLine("500G\n");
        if (Accep)
        {
            Console.WriteLine("퀘스트를 수락하셨습니다.");
        }
        else if (!Accep)
        {
            Console.WriteLine("1. 수락");
            Console.WriteLine("2. 거절\n");
        }
        else
        {
            Console.WriteLine("1. 보상 받기");
            Console.WriteLine("2. 돌아가기\n");
        }
    }

    void SkillQuestScreen(bool Accep)
    {
        {
            Console.WriteLine("스킬을 사용해보자\n");
            Console.WriteLine($"{_gameState.Player.Name} 너에게 직업 스킬을 사용해봐!");
            Console.WriteLine($"직업 스킬은 {_gameState.Player.Job}만 사용 할 수 있는 스킬이야!\n");
            Console.WriteLine("- 전용스킬 사용 ({f/t)\n");
            Console.WriteLine("- 보상 - ");
            Console.WriteLine("공격력 + 5");
            Console.WriteLine("500G\n");
            if (Accep)
            {
                Console.WriteLine("퀘스트를 수락하셨습니다.");
            }
            else if (!Accep)
            {
                Console.WriteLine("1. 수락");
                Console.WriteLine("2. 거절\n");
            }
            else
            {
                Console.WriteLine("1. 보상 받기");
                Console.WriteLine("2. 돌아가기\n");
            }
        }
    }


    public IScene? GetNextScene()
    {
        int input = Utility.GetInput(1, 2);
        return input switch     //  C#의 `switch 표현식` 입니다. 필요하신분 찾아 보세요
        {
            1 => new QuestScene(_gameState, State.KillQuest),
            2 => new QuestScene(_gameState, State.EquipQuest),
            3 => new QuestScene(_gameState, State.SkillQuest),
            0 => new MainScene(_gameState),
            _ => null
        };
    }
}
