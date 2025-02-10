namespace TextRPG_Team;

//수정원할시 사전 공지
static class Utility
{
    private static Queue<(string, ConsoleColor)> Logs { get; } = new(); //Log Info를 저장할 Queue입니다.

    /// <summary>
    /// 플레이어의 입력값(정수) 받는 함수
    /// </summary>
    /// <param name="min">선택지 최소값</param>
    /// <param name="max">선택지 최대값</param>
    /// /// <param name="prompt">기본="원하시는 행동을 입력해주세요."</param>
    /// <returns></returns>
    public static int GetInput(int min, int max, string prompt = "원하시는 행동을 입력해주세요.")
    {
        while (true)
        {
            // 현재 위치를 저장
            int cursorTop = Console.CursorTop;

            Console.WriteLine(prompt);
            ColorWrite(">> ", ConsoleColor.Yellow);

            if (int.TryParse(Console.ReadLine(), out int input) && (input >= min) && (input <= max))
                return input;

            // 잘못된 입력이므로 메시지를 수정
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(0, cursorTop + i); // 이전 메시지가 있던 위치로 커서 이동
                Console.Write(new string(' ', Console.WindowWidth)); // 공백으로 채우기
            }

            ColorWrite("잘못된 입력입니다. 다시 입력해주세요", ConsoleColor.Red); //경고 메세지 출력

            Console.SetCursorPosition(0, cursorTop); // 커서를 다시 이동
        }
    }

    /// <summary>
    /// 컬러로 문자열 작성하는 코드(줄바꿈X)
    /// </summary>
    /// <param name="str">출력할 문자열</param>
    /// <param name="color">출력할 색상 ex)ConsoleColor.Red</param>
    public static void ColorWrite(string str, ConsoleColor color)
    {
        Console.ForegroundColor = color; //텍스트 컬러 설정
        Console.Write(str);
        Console.ResetColor();
    }

    /// <summary>
    /// 컬러로 문자열 작성하는 코드(줄바꿈O)
    /// </summary>
    /// <param name="str">출력할 문자열</param>
    /// <param name="color">출력할 색상 ex)ConsoleColor.Red</param>
    public static void ColorWriteLine(string str, ConsoleColor color)
    {
        Console.ForegroundColor = color; //텍스트 컬러 설정
        Console.WriteLine(str);
        Console.ResetColor();
    }

    /// <summary>
    /// 전 씬에서 발생한 모든 로그들을 출력하는 함수
    /// </summary>
    public static void PrintLogs()
    {
        while (Logs.Count > 0)
        {
            var (log, color) = Logs.Dequeue();
            ColorWriteLine(log, color);
        }
    }

    /// <summary>
    /// 로그(Log)에 문자열 추가하는 함수입니다
    /// </summary>
    /// <param name="log">추가할 문자열</param>
    /// <param name="color">색상</param>
    public static void AddLog(string log, ConsoleColor color)
    {
        Logs.Enqueue((log, color));
    }
}