using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string x=Console.ReadLine();
        bool z=    CheckMemeberAge(x);
            if (z)
                Console.WriteLine("Hello !");
            else
                Console.WriteLine(" bye!");
        }
        public static bool CheckMemeberAge(string PersonalNo)
        {
            var BirthDate = PersonalNo.Substring(0, 4)+"/"+ PersonalNo.Substring(4, 2) + "/" + PersonalNo.Substring(6, 2);

            var DiffInterval = DateTime.Now.Subtract(DateTime.Parse(BirthDate)).Days;
            var Age = DiffInterval / 365;
            Console.WriteLine(Age);
            if (Age>= 18)
                return true;
            else
                return false;
        }
    }
}
