Console.WriteLine($"{A(ReadGrid("./input/input.txt"))}");
Console.WriteLine($"{B(ReadGrid("./input/input.txt"))}");


static List<List<char>> ReadGrid(string in_file)
{
    var file = File.OpenText(in_file);
    string? line = file.ReadLine();
    List<List<char>> grid = new List<List<char>>();
    while (line != null)
    {
        List<char> row = new List<char>();
        foreach(char c in line.ToCharArray())
        {
            row.Add(c);
        }
        grid.Add(row);
        line = file.ReadLine();
    }
    return grid;

}

static bool IsPaperRoll(List<List<char>> grid, int x, int y)
{
    return !(y < 0 || y >= grid.Count || x < 0 || x >= grid[y].Count) && grid[x][y] == '@';
}

// Check if the given x and y are a paper roll which can be moved
// Checking one by one is inefficient but should be OK since it doesn't feel like 
// we can do Dynamic Programming this time.
static bool IsMovablePaperRoll(List<List<char>> grid, int x, int y)
{
    if (!IsPaperRoll(grid, x, y))
    {
        return false;
    }
    int adjacent_paper_rolls = 0;
    for (int adjacent_x = x - 1; adjacent_x < x + 2; adjacent_x++)
    {
        for (int adjacent_y = y - 1; adjacent_y < y + 2; adjacent_y++)
        {
            if (adjacent_x == x && adjacent_y == y)
            {
                continue;
            }
            if (IsPaperRoll(grid, adjacent_x, adjacent_y))
            {
                adjacent_paper_rolls++;
                if (adjacent_paper_rolls >= 4)
                {
                    return false;
                }
            }
        }
    }
    return true;
}

static string A(List<List<char>> grid)
{
    int movable_rolls = 0;
    for (int y = 0; y < grid.Count(); y++)
    {
        for (int x = 0; x < grid[y].Count(); x++)
        {
            if (IsMovablePaperRoll(grid, x, y))
            {
                movable_rolls++;
            }
        }
    }
    return $"{movable_rolls}";
}

static Queue<Tuple<int, int>> CollectMovableRolls(List<List<char>> grid)
{
    Queue<Tuple<int, int>> movables = new Queue<Tuple<int, int>>();
    for (int y = 0; y < grid.Count(); y++)
    {
        for (int x = 0; x < grid[y].Count(); x++)
        {
            if (IsMovablePaperRoll(grid, x, y))
            {
                movables.Enqueue(Tuple.Create(x, y));
            }
        }
    }
    return movables;
}

static string B(List<List<char>> grid)
{
    int total_removed_rolls = 0;
    var to_be_removed = CollectMovableRolls(grid);
    while (to_be_removed.Count > 0)
    {
        Tuple<int, int> removable_roll = to_be_removed.Dequeue();
        grid[removable_roll.Item1][removable_roll.Item2] = '.';
        total_removed_rolls += 1;
        if (to_be_removed.Count == 0)
        {
            to_be_removed = CollectMovableRolls(grid);
        }
    }
    return $"{total_removed_rolls}";
}