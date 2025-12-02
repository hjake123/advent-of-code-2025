// Remember to set the input files to copy when building!
using System.Collections;

Console.WriteLine($"A: {A(Read("./input/input.txt"))}");
Console.WriteLine($"B: {B(Read("./input/input.txt"))}");

static List<Tuple<long, long>> Read(string in_file)
{
    var file_handle = File.OpenText(in_file);
    if(file_handle == null)
    {
        throw new FileNotFoundException($"{in_file} does not exist or is inaccessible");
    }
    string? text = file_handle.ReadLine();
    if(text == null)
    {
        throw new InvalidDataException("No data in file!");
    }
    var ranges = new List<Tuple<long, long>>();
    foreach (var range in text.Split(','))
    {
        var range_parts = range.Split('-').ToArray();
        long min = long.Parse(range_parts[0]);
        long max = long.Parse(range_parts[1]);
        ranges.Add(Tuple.Create(min, max));
    }
    return ranges;
}

// Divides the given string into the given number of segments, and checks if they're all equal, if the text can be split evenly into that many segments
static bool CheckRepeated(string text, int segments)
{
    int digit_count = text.Length;
    if (digit_count % segments != 0)
    {
        return false;
    }
    string first = text.Substring(0, digit_count / segments);
    for (int i = 1; i < segments; i++)
    {
        string next = text.Substring(i * (digit_count / segments), digit_count / segments);
        if (!first.Equals(next))
        {
            return false;
        }
    }
    return true;
}

// Check all valid subdivisions for repetition.
// Goal is to cut each string (each number) into N pieces, where N >= 2 and N <= (number of chars). 
// We only need to consider cuts for which (number of chars) % N == 0.
static bool CheckAllRepeated(long value)
{
    string text = value.ToString();
    for (int n = 2; n <= text.Length; n++)
    {
        if (CheckRepeated(text, n))
        {
            // Console.WriteLine($"Matched {text} into {n} segments");
            return true;
        }
    }
    return false;
}

// Yes this is the dumb way to do this. Too bad!
static string A(List<Tuple<long, long>> input)
{
    long result = 0;
    foreach (Tuple<long, long> range in input)
    {
        for (long i = range.Item1; i <= range.Item2; i++)
        {
            if (CheckRepeated(i.ToString(), 2))
            {
                result += i;
            }
        }
    }
    return $"{result}";
}

static string B(List<Tuple<long, long>> input)
{
    long result = 0;
    foreach (Tuple<long, long> range in input)
    {
        for (long i = range.Item1; i <= range.Item2; i++)
        {
            if (CheckAllRepeated(i))
            {
                result += i;
            }
        }
    }
    return $"{result}";
}