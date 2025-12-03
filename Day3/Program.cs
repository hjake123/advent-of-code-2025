Console.WriteLine($"A: {Run("./input/input.txt", 2)}");
Console.WriteLine($"B: {Run("./input/input.txt", 12)}");

static string Run(string in_file, int cells_to_activate)
{
    var file_handle = File.OpenText(in_file);
    if (file_handle == null)
    {
        throw new FileNotFoundException($"{in_file} does not exist or is inaccessible");
    }

    long joltage = 0;

    while (!file_handle.EndOfStream)
    {
        string? line = file_handle.ReadLine();
        if(line == null)
        {
            throw new InvalidDataException($"No line before end of stream??");
        }
        joltage += NCellJoltage(line, cells_to_activate);
    }
    return $"{joltage}";
}

static long NCellJoltage(string bank, int cell_count)
{
    long bank_joltage = 0;
    int last_digit_index = 0;
    for (int n = 0; n < cell_count; n++)
    {
        bank_joltage *= 10;
        int nth_digit = 0;
        int nth_digit_index = 0;
        for (int i = last_digit_index; i < bank.Length - (cell_count - n - 1); i++) // Reserve enough characters from scanning for future digits to take them
        {
            int digit = int.Parse($"{bank[i]}");
            if (digit > nth_digit)
            {
                nth_digit = digit;
                nth_digit_index = i;
            }
        }
        bank_joltage += nth_digit;
        last_digit_index = nth_digit_index + 1;
    }
    return bank_joltage;
}