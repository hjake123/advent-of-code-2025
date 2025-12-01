// See https://aka.ms/new-console-template for more information

// Remember to set the input files to copy when building!
Console.WriteLine($"A: {A("./input/input.txt")}");
Console.WriteLine($"B: {B("./input/input.txt")}");

static string A(string in_file)
{
    int dial = 50;
    int zero_count = 0;

    var input = File.OpenText(in_file);

    while (!input.EndOfStream)
    {
        string? command = input.ReadLine();
        if (command == null)
        {
            break;
        }
        if (command[0] == 'L')
        {
            int diff = int.Parse(command.Substring(1));
            dial -= diff;
            while (dial < 0)
            {
                dial += 100;
            }
        }
        else if (command[0] == 'R')
        {
            dial += int.Parse(command.Substring(1));
        }
        dial %= 100;
        if (dial == 0)
        {
            zero_count++;
        }
    }
    return $"{zero_count}";
}

static string B(string in_file)
{
    int dial = 50;
    int zero_count = 0;

    var input = File.OpenText(in_file);

    while (!input.EndOfStream)
    {
        string? command = input.ReadLine();
        if (command == null)
        {
            break;
        }
        if (command[0] == 'L')
        {
            int diff = int.Parse(command.Substring(1));
            bool at_zero = dial == 0;
            dial -= diff;
            while(dial < 0)
            {
                dial += 100;
                if(!at_zero)
                {
                    zero_count++;
                }
                at_zero = false;
            }
        }
        else if (command[0] == 'R')
        {
            dial += int.Parse(command.Substring(1));
            while(dial >= 100)
            {
                dial -= 100;
                if(dial != 0) 
                {
                    zero_count++;
                }
            }
        }
        if (dial == 0)
        {
            zero_count++;
        }
    }
    return $"{zero_count}";
}
