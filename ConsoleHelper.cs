namespace ConsoleHelper
{
    public class Helper
    {
        private static ConsoleColor _defaultColor = ConsoleColor.White;

        public static void Clear() => Console.Clear();
        public static void ResetColor() => Console.ForegroundColor = _defaultColor;

        public static void SetDefaultColor(ConsoleColor color) => _defaultColor = color;

        public static void Write(string text, ConsoleColor? color = null)
        {
            var previousColor = Console.ForegroundColor;
            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.Write(text);

            if (color.HasValue)
            {
                Console.ForegroundColor = previousColor;
            }
        }

        public static void WriteLine(string text, ConsoleColor? color = null)
        {
            Write(text + Environment.NewLine, color);
        }


        public static void WriteError(string Error, string message)
        {
            Write(Error, ConsoleColor.Red);
            WriteLine(" " + message, ConsoleColor.Yellow);
        }

        public static void Logo()
        {
            WriteLine(@"
 __                  ______   ____                  
/\ \                /\__  _\ /\  _`\                
\ \ \____  __  __   \/_/\ \/ \ \ \L\ \              
 \ \ '__`\/\ \/\ \     \ \ \  \ \ ,  /              
  \ \ \L\ \ \ \_\ \     \_\ \__\ \ \\ \             
   \ \_,__/\/`____ \    /\_____\\ \_\ \_\           
    \/___/  `/___/> \   \/_____/ \/_/\/ /        
               /\___/                               
               \/__/                  ImRibka              ", ConsoleColor.Cyan);
        }
    }
}
