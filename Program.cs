using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("1. Частота слов в файле");
        Task1();

        Console.WriteLine();
        Console.WriteLine("2. Вычисление арифметического выражения");
        Task2();

        Console.WriteLine();
        Console.WriteLine("3. Информация о стране");
        Task3();

        Console.ReadLine();
    }

    static void Task1()
    {
        string path = "text.txt";

        if (!File.Exists(path))
        {
            Console.WriteLine("Файл text.txt не найден");
            return;
        }

        string text = File.ReadAllText(path).ToLower();

        char[] separators =
        {
            ' ', '\n', '\r', '\t', '.', ',', ';', ':', '!', '?',
            '(', ')', '[', ']', '{', '}', '"', '\'', '-', '_'
        };

        string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, int> dictionary = new Dictionary<string, int>();

        for (int i = 0; i < words.Length; i++)
        {
            if (dictionary.ContainsKey(words[i]))
                dictionary[words[i]]++;
            else
                dictionary.Add(words[i], 1);
        }

        List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

        foreach (KeyValuePair<string, int> item in dictionary)
        {
            list.Add(item);
        }

        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - 1 - i; j++)
            {
                if (list[j].Value < list[j + 1].Value)
                {
                    KeyValuePair<string, int> temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i].Key + " - " + list[i].Value);
        }
    }

    static void Task2()
    {
        Console.Write("Введите выражение: ");
        string expression = Console.ReadLine();

        try
        {
            List<string> postfix = ToPostfix(expression);
            double result = CalculatePostfix(postfix);
            Console.WriteLine("Результат: " + result);
        }
        catch
        {
            Console.WriteLine("Неверное выражение");
        }
    }

    static List<string> ToPostfix(string expression)
    {
        Dictionary<char, int> priority = new Dictionary<char, int>();
        priority.Add('+', 1);
        priority.Add('-', 1);
        priority.Add('*', 2);
        priority.Add('/', 2);

        List<string> output = new List<string>();
        Stack<char> operators = new Stack<char>();

        int i = 0;

        while (i < expression.Length)
        {
            char ch = expression[i];

            if (ch == ' ')
            {
                i++;
                continue;
            }

            if (char.IsDigit(ch) || ch == '.')
            {
                string number = "";

                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                {
                    number += expression[i];
                    i++;
                }

                output.Add(number);
                continue;
            }

            if (ch == '(')
            {
                operators.Push(ch);
                i++;
                continue;
            }

            if (ch == ')')
            {
                while (operators.Count > 0 && operators.Peek() != '(')
                {
                    output.Add(operators.Pop().ToString());
                }

                if (operators.Count > 0 && operators.Peek() == '(')
                    operators.Pop();

                i++;
                continue;
            }

            if (priority.ContainsKey(ch))
            {
                while (operators.Count > 0 &&
                       operators.Peek() != '(' &&
                       priority.ContainsKey(operators.Peek()) &&
                       priority[operators.Peek()] >= priority[ch])
                {
                    output.Add(operators.Pop().ToString());
                }

                operators.Push(ch);
                i++;
                continue;
            }

            throw new Exception();
        }

        while (operators.Count > 0)
        {
            output.Add(operators.Pop().ToString());
        }

        return output;
    }

    static double CalculatePostfix(List<string> postfix)
    {
        Stack<double> stack = new Stack<double>();

        for (int i = 0; i < postfix.Count; i++)
        {
            string item = postfix[i];

            if (item == "+" || item == "-" || item == "*" || item == "/")
            {
                double b = stack.Pop();
                double a = stack.Pop();

                if (item == "+")
                    stack.Push(a + b);
                else if (item == "-")
                    stack.Push(a - b);
                else if (item == "*")
                    stack.Push(a * b);
                else
                    stack.Push(a / b);
            }
            else
            {
                stack.Push(double.Parse(item));
            }
        }

        return stack.Pop();
    }

    static void Task3()
    {
        Dictionary<string, string> capitals = new Dictionary<string, string>();
        capitals.Add("Россия", "Москва");
        capitals.Add("Франция", "Париж");
        capitals.Add("Германия", "Берлин");
        capitals.Add("Италия", "Рим");
        capitals.Add("Япония", "Токио");

        Dictionary<string, int> population = new Dictionary<string, int>();
        population.Add("Россия", 146000000);
        population.Add("Франция", 68000000);
        population.Add("Германия", 84000000);
        population.Add("Италия", 59000000);
        population.Add("Япония", 124000000);

        Console.Write("Введите название страны: ");
        string country = Console.ReadLine();

        if (capitals.ContainsKey(country) && population.ContainsKey(country))
        {
            Console.WriteLine("Столица: " + capitals[country]);
            Console.WriteLine("Население: " + population[country]);
        }
        else
        {
            Console.WriteLine("Страна не найдена");
        }
    }
}