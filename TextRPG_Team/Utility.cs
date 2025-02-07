namespace TextRPG_Team;

//수정원할시 사전 공지
static class Utility
{
    /// <summary>
    /// 플레이어의 입력값(정수) 받는 함수
    /// </summary>
    /// <param name="min">선택지 최소값</param>
    /// <param name="max">선택지 최대값</param>
    /// <returns></returns>
    public static int GetInput(int min, int max)
    {
        while (true)
        {
            // 현재 위치를 저장
            int cursorTop = Console.CursorTop;

            Console.WriteLine("원하시는 행동을 입력해주세요.");
            ColorWrite(">> ", ConsoleColor.Yellow);

            if (int.TryParse(Console.ReadLine(), out int input) && (input >= min) && (input <= max))
                return input;

            // 잘못된 입력이므로 메시지를 수정
            Console.SetCursorPosition(0, cursorTop); // 이전 메시지가 있던 위치로 커서 이동
            Console.Write(new string(' ', Console.WindowWidth)); // 공백으로 채우기("원하시는 행동을 입력해주세요.")
            Console.Write(new string(' ', Console.WindowWidth)); // 공백으로 채우기(">>")

            ColorWriteLine("잘못된 입력입니다. 다시 입력해주세요", ConsoleColor.Red);   //경고 메세지 출력
            
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
    /// <param name="logs">_gameState.Logs</param>
    /// <param name="color">컬러값 ex)Console.Color.Red</param>
    public static void PrintLogs(Queue<(string, ConsoleColor)> logs)
    {
        while (logs.Count > 0)
        {
            var (log, color) = logs.Dequeue();
            ColorWriteLine(log, color);
        }
    }

}