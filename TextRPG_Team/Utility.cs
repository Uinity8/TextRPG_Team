namespace TextRPG_Team;

//수정원할시 사전 공지
static class Utility
{
    public static int Width = 68;//Console.WindowWidth;
    private static Queue<(string, ConsoleColor)> Logs { get; } = new(); //Log Info를 저장할 Queue입니다.

    /// <summary>
    /// 플레이어의 입력값(정수) 받는 함수
    /// </summary>
    /// <param name="min">선택지 최소값</param>
    /// <param name="max">선택지 최대값</param>
    /// /// <param name="prompt">기본="원하시는 행동을 입력해주세요."</param>
    /// <returns></returns>
    public static int GetInput(int min, int max, string prompt = " 원하시는 행동을 입력해주세요.")
    {
        int cursorTop = Console.GetCursorPosition().Top; // 커서 위치 저장
        Console.Write(prompt); // 줄바꿈 없이 출력
            while (true)
            {
                Console.WriteLine();
                ColorWrite(" >> ", ConsoleColor.Yellow);

                if (int.TryParse(Console.ReadLine(), out int input) && (input >= min) && (input <= max))
                    return input;

                // 이전 두 줄 지우기
                Console.Write("\u001b[1A");  // 위로 이동
                Console.Write(new string(' ', Console.WindowWidth));
                Console.Write("\u001b[1A");  // 위로 이동
                Console.Write(new string(' ', Console.WindowWidth));
                Console.Write("\u001b[1A");  // 위로 이동
                Console.Write("\r");
                // 경고 메시지 출력
                ColorWrite(" 잘못된 입력입니다. 다시 입력해주세요", ConsoleColor.Red);
            }
    }

    /// <summary>
    /// 컬러로 문자열 작성하는 코드(줄바꿈X)
    /// </summary>
    /// <param name="str">출력할 문자열</param>

    /// <param name="color">출력할 색상 ex)ConsoleColor.Red</param>
    public static void ColorWrite(string str, ConsoleColor color =ConsoleColor.White)
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
    public static void ColorWriteLine(string str, ConsoleColor color=ConsoleColor.White)
    {
        Console.ForegroundColor = color; //텍스트 컬러 설정
        Console.WriteLine(str);
        Console.ResetColor();
    }
    
    // 정렬 옵션 열거형
    public enum Alignment { Left, Right, Center }
    
    // 문자열 정렬 함수
    // 좌측 정렬(오른쪽공백)
    public static void AlignLeft(string text, int width,ConsoleColor color = ConsoleColor.White)
    {
        // 한글 문자의 너비를 고려한 사용 가능한 패딩 계산
        int spaces = width - text.Sum(c => (c >= '\uAC00' && c <= '\uD7A3') ? 1 : 0);
        text = spaces > 0 ? text.PadRight(spaces) : text;
        ColorWrite(text, color);
    }

    // 우측 정렬(왼쪽공백)
    public static void AlignRight(string text, int width,ConsoleColor color = ConsoleColor.White)
    {
        // 한글 문자의 너비를 고려한 사용 가능한 패딩 계산
        int spaces = width - text.Sum(c => (c >= '\uAC00' && c <= '\uD7A3') ? 1 : 0);
        text = spaces > 0 ? text.PadLeft(spaces) : text;
        ColorWrite(text, color);
    }

    // 중앙 정렬
    public static void AlignCenter(string text,ConsoleColor color = ConsoleColor.White)
    {
        int spaces = Width - text.Sum(c => (c >= '\uAC00' && c <= '\uD7A3') ? 2 : 1);
        if (spaces > 0)
        {
            int leftPadding = spaces / 2;
            int rightPadding = spaces - leftPadding;
            text = new string(' ', leftPadding) + text;// + new string(' ', rightPadding);
        }
        ColorWrite(text, color);
    }


    /// <summary>
    /// 전 씬에서 발생한 모든 로그들을 출력하는 함수
    /// </summary>
    public static void PrintLogs()
    {
        if (Logs.Count == 0)
        {
            Console.WriteLine();
            return;
        }

        while (Logs.Count > 0)
        {
            var (log, color) = Logs.Dequeue();
            AlignCenter(log, color);
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