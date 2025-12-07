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

static long CountTimelines(List<string> grid, int x_positon, int row_number, ref Dictionary<Tuple<int, int>, long> memos)
{
    if (row_number == grid.Count - 1)
    {
        return 1;
    }

    var memo_key = Tuple.Create(x_positon, row_number);
    if (memos.ContainsKey(memo_key))
    {
        return memos[memo_key];
    }
    long tally = 0;
    if (grid[row_number][x_positon] == '^')
    {
        if (x_positon > 0)
        {
            tally += CountTimelines(grid, x_positon - 1, row_number + 1, ref memos);
        }
        if (x_positon <= grid[row_number + 1].Length - 1)
        {
            tally += CountTimelines(grid, x_positon + 1, row_number + 1, ref memos);
        }
    } 
    else
    {
        tally += CountTimelines(grid, x_positon, row_number + 1, ref memos);
    }
    memos.Add(memo_key, tally);
    return tally;
}

static long B(string in_file)
{
    var input_file = File.OpenText(in_file);
    List<string> rows = input_file.ReadToEnd().Split("\r\n").ToList();
    var memos = new Dictionary<Tuple<int, int>, long>();
    return CountTimelines(rows, rows[0].IndexOf('S'), 0, ref memos);
}