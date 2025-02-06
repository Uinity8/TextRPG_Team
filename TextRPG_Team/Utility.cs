namespace TextRPG_Team;

static class Utility
{
    //입력키 확인
    public static int GetInput(int min, int max)
    {
        while (true)
        {
            Console.Write("원하시는 행동을 입력해주세요.");

            //int.TryParse는 int로 변환이 가능한지 bool값을 반환, 가능(true)할 경우 out int input으로 숫자도 반환
            if (int.TryParse(Console.ReadLine(), out int input) && (input >= min) && (input <= max))
                return input;

            ColorWriteLine("잘못된 입력입니다. 다시 입력해주세요", ConsoleColor.Red);
        }
    }
    
    //글자 색 변경(줄바꿈 X)
    public static void ColorWrite(string str, ConsoleColor color)
    {
        Console.ForegroundColor = color; //텍스트 컬러 설정
        Console.Write(str);
        Console.ResetColor();
    }
    
    //글자 색 변경(줄바꿈)
    public static void ColorWriteLine(string str, ConsoleColor color)
    {
        Console.ForegroundColor = color; //텍스트 컬러 설정
        Console.WriteLine(str);
        Console.ResetColor();
    }
}