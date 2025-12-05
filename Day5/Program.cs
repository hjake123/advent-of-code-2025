Console.WriteLine($"{A("./input/input.txt")}");
Console.WriteLine($"{B("./input/input.txt")}");

// Read until the empty line and return all ranges in the file.
static List<Tuple<long, long>> ReadRanges(StreamReader file)
{
    List<Tuple<long, long>> ranges = new List<Tuple<long, long>>();
    string? line = file.ReadLine();
    while(line != null && line != "")
    {
        var range_bounds = line.Split('-');
        long left_bound = long.Parse(range_bounds[0]);
        long right_bound = long.Parse(range_bounds[1]);
        ranges.Add(Tuple.Create(left_bound, right_bound));
        line = file.ReadLine();
    }
    return ranges;
}

static string A(string in_file)
{
    var input = File.OpenText(in_file);
    int fresh_count = 0;

    List<Tuple<long, long>> ranges = ReadRanges(input);

    string? ingredient_id_str = input.ReadLine();
    while(ingredient_id_str != null)
    {
        long ingredient_id = long.Parse(ingredient_id_str);
        foreach (var range in ranges)
        {
            if (ingredient_id >= range.Item1 && ingredient_id <= range.Item2)
            {
                fresh_count++;
                break;
            }
        }

        ingredient_id_str = input.ReadLine();
    }
    return $"{fresh_count}";
}

static bool CheckSeperate(Tuple<long, long> range_a, Tuple<long, long> range_b)
{
    return range_a.Item1 > range_b.Item2 || range_a.Item2 < range_b.Item1;
}

static Tuple<long, long> MergeRanges(Tuple<long, long> range_a, Tuple<long, long> range_b)
{
    return Tuple.Create(Math.Min(range_a.Item1, range_b.Item1), Math.Max(range_a.Item2, range_b.Item2));
}

static List<Tuple<long, long>> MergeOverlappingRanges(List<Tuple<long, long>> input_ranges)
{
    List<Tuple<long, long>> unique_ranges = new List<Tuple<long, long>>();
    foreach (var input_range in input_ranges)
    {
        Tuple<long, long> active_range = Tuple.Create(input_range.Item1, input_range.Item2);
        Tuple<long, long> overlapping_range;
        do
        {
            // Find an overlap if one exists
            overlapping_range = null;
            foreach (var unique_range in unique_ranges)
            {
                if (!CheckSeperate(active_range, unique_range))
                {
                    overlapping_range = unique_range;
                    break;
                }
            }
            // If there is an overlap, merge it into the active range
            // and remove the newly redundant range from the unique list for next cycle
            if (overlapping_range != null)
            {
                active_range = MergeRanges(active_range, overlapping_range);
                unique_ranges.Remove(overlapping_range);
            }
        } 
        while (overlapping_range != null);
        unique_ranges.Add(active_range);
    }
    return unique_ranges;
}

static string B(string in_file)
{
    var input = File.OpenText(in_file);
    long total_fresh_count = 0;

    var unique_ranges = MergeOverlappingRanges(ReadRanges(input));

    foreach (var range in unique_ranges)
    {
        long range_size = range.Item2 - range.Item1 + 1;
        total_fresh_count += range_size;
    }
    
    return $"{total_fresh_count}";
}