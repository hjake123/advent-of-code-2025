// Remember to set the input files to copy when building!
using System.Collections;

Console.WriteLine($"A: {A(Read("./input/test.txt"))}");

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

static string A(List<Tuple<long, long>> input)
{
    return $"{input}";
}