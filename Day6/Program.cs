Console.WriteLine($"{A("./input/input.txt")}");
Console.WriteLine($"{B("./input/input.txt")}");

static List<Problem> Read(string in_file)
{
    var problems = new List<Problem>();
    var input_file = File.OpenText(in_file);
    while(!input_file.EndOfStream)
    {
        string? line = input_file.ReadLine();
        if(line == null)
        {
            break;
        }
        int current_problem = 0;
        foreach(string part in line.Split(' '))
        {
            if (part.IsWhiteSpace())
            {
                continue;
            }
            long number = 0;
            if (long.TryParse(part, out number))
            { 
                if (problems.Count >= current_problem)
                { // We still need to add a problem for this index.
                    problems.Add(new Problem
                    {
                        Parameters = new List<long>(),
                        Operation = null
                    });

                }
                problems[current_problem].Parameters.Add(number);
            }
            else
            { // It must be the operation
                var problem = problems[current_problem];
                problem.Operation = part[0];
                problems[current_problem] = problem;
            }
            current_problem++;
        }
    }
    return problems;
}

static long A(string in_file)
{
    List<Problem> problems = Read(in_file);
    long total = 0;
    foreach(var problem in problems)
    {
        if (problem.Operation == '+')
        {
            total += problem.Parameters.Aggregate((A, B) => A + B);
        }
        else if (problem.Operation == '*')
        {
            total += problem.Parameters.Aggregate((A, B) => A * B);
        }
    }
    return total;
}

static List<string> SplitByColumn(string input)
{
    List<string> rows = input.Split("\r\n").ToList();
    List<string> columns = new List<string>();
    for (int i = rows[0].Length - 1; i >= 0; i--)
    {
        string column = "";
        foreach (string row in rows)
        {
            column = column + row[i];
        }
        columns.Add(column);
    }
    return columns;
}

// Damn Cephalopods...
static long B(string in_file)
{
    long total = 0;
    string input = File.OpenText(in_file).ReadToEnd();
    var columns = SplitByColumn(input);
    List<long> parameters = new List<long>();
    foreach (string column in columns)
    {
        if (column.IsWhiteSpace())
        {
            // Skip divider columns.
            continue;
        }
        if (column.Contains('+'))
        {
            parameters.Add(long.Parse(column.Substring(0, column.Length - 1)));
            total += parameters.Aggregate((A, B) => A + B);
            parameters.Clear();
        }
        else if (column.Contains('*'))
        {
            parameters.Add(long.Parse(column.Substring(0, column.Length - 1)));
            total += parameters.Aggregate((A, B) => A * B);
            parameters.Clear();
        }
        else
        {
            parameters.Add(long.Parse(column));
        }
    }
    return total;
}

struct Problem
{
    public List<long> Parameters;
    public char? Operation;
}