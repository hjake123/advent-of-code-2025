using System.Collections;

Console.WriteLine($"{A("./input/input.txt")}");
Console.WriteLine($"{B("./input/input.txt")}");

static int A(string in_file)
{
    var input_file = File.OpenText(in_file);
    int split_events = 0;
    string? first_row = input_file.ReadLine();
    if (first_row == null)
    {
        throw new InvalidDataException("No data in file");
    }
    BitArray tachyon_positions = new BitArray(first_row.Length);
    tachyon_positions.Set(first_row.IndexOf('S'), true);
    while (!input_file.EndOfStream)
    {
        string? row = input_file.ReadLine();
        if (row == null)
        {
            break;
        }
        for (int i = 0; i < tachyon_positions.Length; i++)
        {
            if (tachyon_positions[i])
            {
                // The above row has tachyons here. They move down and possibly split to "this" row.
                // We must update the positions array to accomodate that movement.
                if (row[i] == '^')
                {
                    split_events++;
                    tachyon_positions.Set(i, false);
                    if (i > 0)
                    {
                        tachyon_positions.Set(i - 1, true);
                    }
                    if (i <= tachyon_positions.Length)
                    {
                        tachyon_positions.Set(i + 1, true);
                    }
                } 
                // Otherwise we assume they just move down.
            }
        }
    }
    return split_events;
}

// Count upward by recursively checking different paths that could happen upward.
// If you reach the S, return 1. If you leave the grid, return 0.
// Otherwise, return the sum of the possible paths above yourself.
static long CountTimelinesUpward(List<string> grid, int x_positon, int row_number, ref Dictionary<Tuple<int, int>, long> memos)
{
    var memo_key = Tuple.Create(x_positon, row_number);
    if (memos.ContainsKey(memo_key))
    {
        return memos[memo_key];
    }

    if (row_number <= 0)
    {
        memos.Add(memo_key, grid[row_number][x_positon] == 'S' ? 1 : 0);
        return grid[row_number][x_positon] == 'S' ? 1 : 0;
    }
    if (grid[row_number][x_positon] == '^')
    {
        // No valid path ever terminates in a splitter.
        memos.Add(memo_key, 0);
        return 0;
    }
    long tally = 0;
    if (x_positon > 0 && grid[row_number][x_positon - 1] == '^')
    {
        // There is a splitter to our left, so there could be a path to the left of us.
        tally += CountTimelinesUpward(grid, x_positon - 1, row_number - 1, ref memos);
    }
    if (x_positon < grid[row_number].Length-1 && grid[row_number][x_positon + 1] == '^')
    {
        // There is a splitter to our right, so there could be a path to the right of us.
        tally += CountTimelinesUpward(grid, x_positon + 1, row_number - 1, ref memos);
    }
    tally += CountTimelinesUpward(grid, x_positon, row_number - 1, ref memos);
    memos.Add(memo_key, tally);
    return tally;
}

static long B(string in_file)
{
    var input_file = File.OpenText(in_file);
    List<string> rows = input_file.ReadToEnd().Split("\r\n").ToList();
    long timelines = 0;
    var memos = new Dictionary<Tuple<int, int>, long>();
    for (int i = 0; i < rows[rows.Count - 1].Length; i++)
    {
        timelines += CountTimelinesUpward(rows, i, rows.Count - 1, ref memos);
    }
    return timelines;
}