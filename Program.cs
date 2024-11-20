/*
 * Name: [Logan Brooks]
 * South Hills Username: [lbrooks81]
 */

using System;
using System.Runtime.CompilerServices;

namespace Calculator
{

    public class Program
    {
        public static List<double> memory = new List<double>();
        public static List<double> results = new List<double>();
        public const String FILE_NAME = "History.txt";
        /* public const String MEMORY_FILE_NAME = "Memory.txt"; */
        public static bool finished = false;
        public const int SUB_EXPRESSION_LENGTH = 3;
        public const int LIST_REMOVAL_OFFSET = 2;
        public static void Main()
        {
            CreateFile(FILE_NAME);
            /*CreateFile(MEMORY_FILE_NAME); */
            while (true)
            {
                List<String> inputs = GetInputs();
                if (finished)
                {
                    break;
                }
                if(inputs != null)
                {
                    double result = Calculation(inputs);
                    Console.WriteLine(result);
                    Console.Write(Environment.NewLine);
                    AppendToFile(result);
                }
            }
        }
        public static List<String> GetInputs()
        {
            
            Console.WriteLine("Type \"History\" to see previous equations.");
            Console.WriteLine("Type \"Q\" to stop.");
            Console.Write("Enter an equation seperated by spaces: ");

            String inputs = Console.ReadLine();
            
            /* if(inputs.Contains("mem"))
            {
                if(!inputs.Contains(String.Empty)) //No spaces are contained means that the user only typed in mem(num), indicating they wish to store memory
                { 
                    StoreMemory(int.Parse(inputs[3..]));
                }
                else
                {
                    for(int i = inputs.LastIndexOf("m") + 1; i < inputs.Length; i++)
                    {
                        if (int.TryParse(inputs, out _))
                        {

                        }
                    }
                    int memnum = int.Parse(inputs[(inputs.Substring(inputs.LastIndexOf("m") + 1), )]);
                }
            }
            */
            
            if (inputs.Equals("History", StringComparison.OrdinalIgnoreCase))
            {
                GetHistory();
                return null;
            }
            if (inputs.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                finished = true;
            }
            else if (int.TryParse(inputs[0].ToString(), out _) == false)
            {
                Console.WriteLine("Not a number.");
                Console.Write(Environment.NewLine);
                return null;
            }

            AppendToFile(inputs);
            return (inputs.Split(' ').ToList());
        }
       
        /*public static void StoreMemory(int num)
        {
            String[]temp = File.ReadAllLines(FILE_NAME);
            int ind = temp[^1].LastIndexOf("=");
            memory.Add(double.Parse(temp[^1][ind..]));
        }
        public static double AccessMemory()
        {
                
        } */
        public static void GetHistory()
        {
            Console.Write(Environment.NewLine);

            foreach (String line in File.ReadAllLines(FILE_NAME))
            {
                Console.WriteLine(line);
            }

            Console.Write(Environment.NewLine);

        }
        public static void CreateFile(String FILE_NAME)
        {
            if (File.Exists(FILE_NAME))
            {
                File.Delete(FILE_NAME);
            }
            File.Create(FILE_NAME).Close();
        }
        public static double Calculation(List<String> inputs)
        {
            double result = 0;
            for (int i = 1; i < inputs.Count; i += 2)
            { 
                //Multiplication and Division first
                if (inputs[i][0].Equals('*'))
                {
                    result = double.Parse(inputs[i - 1]) * double.Parse(inputs[i + 1]);
                    //Removes sub-expression (num1 +-*/ num2) from list and replaces with the result
                    inputs.RemoveRange(i - 1, SUB_EXPRESSION_LENGTH);
                    inputs.Insert(i - 1, result.ToString());
                    //i - 2 to account for the list entries removed
                    i -= LIST_REMOVAL_OFFSET;
                }
                else if (inputs[i][0].Equals('/'))
                {
                    result = double.Parse(inputs[i - 1]) / double.Parse(inputs[i + 1]);
                    inputs.RemoveRange(i - 1, SUB_EXPRESSION_LENGTH);
                    inputs.Insert(i - 1, result.ToString());
                    i -= LIST_REMOVAL_OFFSET;
                }
            }
            for (int i = 1; i < inputs.Count; i += 2)
            {
                //Addition and Subtraction
                if (inputs[i][0].Equals('+'))
                {
                    result = double.Parse(inputs[i - 1]) + double.Parse(inputs[i + 1]);
                    inputs.RemoveRange(i - 1, SUB_EXPRESSION_LENGTH);

                    inputs.Insert(i - 1, result.ToString());
                    i -= LIST_REMOVAL_OFFSET;
                }
                else if (inputs[i][0].Equals('-'))
                {
                    result = double.Parse(inputs[i - 1]) - double.Parse(inputs[i + 1]);
                    inputs.RemoveRange(i - 1, SUB_EXPRESSION_LENGTH);
                    inputs.Insert(i - 1, result.ToString());
                    i -= LIST_REMOVAL_OFFSET;
                }
            }
            return result;
        }
        public static void AppendToFile(String expression)
        {
            //Appends expression
            File.AppendAllText(FILE_NAME, expression);
            File.AppendAllText(FILE_NAME, " = ");
        }
        public static void AppendToFile(double result)
        {
            //Appends result
            File.AppendAllText(FILE_NAME, result.ToString());
            File.AppendAllText(FILE_NAME, Environment.NewLine);
        }
    }
}
