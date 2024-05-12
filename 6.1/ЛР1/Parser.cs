using System;
using System.Collections.Generic;
using System.Text;

public class Parser
{
    private string input;
    private int position;

    public Parser(string input)
    {
        this.input = input;
        position = 0;
    }

    public List<Tetrad> Parse()
    {
        List<Tetrad> tetrads = new List<Tetrad>();
        while (position < input.Length)
        {
            var op = ParseE();
            tetrads.Add(op);
        }
        return tetrads;
    }

    private Tetrad ParseE()
    {
        var result = ParseT();
        while (Peek() == '+' || Peek() == '-')
        {
            var op = Take();
            var right = ParseT();
            var tempResult = "T" + (result.Op[0] == 'T' ? int.Parse(result.Op.Substring(1)) + 1 : int.Parse(result.Op.Substring(1)));
            result = new Tetrad(op.ToString(), result.Result, right.Result, tempResult);
        }
        return result;
    }

    private Tetrad ParseT()
    {
        var result = ParseO();
        while (Peek() == '*' || Peek() == '/')
        {
            var op = Take();
            var right = ParseO();
            var tempResult = "T" + (result.Op[0] == 'T' ? int.Parse(result.Op.Substring(1)) + 1 : int.Parse(result.Op.Substring(1)));
            result = new Tetrad(op.ToString(), result.Result, right.Result, tempResult);
        }
        return result;
    }

    private Tetrad ParseO()
    {
        if (Peek() == '(')
        {
            Take();
            var result = ParseE();
            if (Peek() != ')')
            {
                throw new Exception("Expected ')'");
            }
            Take(); // Consume ')'
            return result;
        }
        else if (char.IsDigit(Peek()))
        {
            var operand = ParseNumber();
            return new Tetrad(operand.ToString(), null, null, operand.ToString());
        }
        else
        {
            throw new Exception("Invalid input");
        }
    }

    private int ParseNumber()
    {
        StringBuilder sb = new StringBuilder();
        while (char.IsDigit(Peek()))
        {
            sb.Append(Take());
        }
        return int.Parse(sb.ToString());
    }


    private char Peek()
    {
        if (position < input.Length)
            return input[position];
        else
            return '\0';
    }

    private char Take()
    {
        char currentChar = Peek();
        position++;
        return currentChar;
    }
}

public class Tetrad
{
    public string Op { get; }
    public string Arg1 { get; }
    public string Arg2 { get; }
    public string Result { get; }

    public Tetrad(string op, string arg1, string arg2, string result)
    {
        Op = op;
        Arg1 = arg1;
        Arg2 = arg2;
        Result = result;
    }
}