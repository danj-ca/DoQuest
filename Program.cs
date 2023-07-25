var QuitCommands = new List<string> { "q", "quit", "exit" };

var command = args[0];
var commandArgs = args[1..];

do
{
    if (string.Equals(command, "record"))
    {
        var taskName = commandArgs[0];
        var score = int.Parse(commandArgs[1]);
        var result = Database.TestDatabase(taskName, score);
        Console.WriteLine(result);
    }
    else
    {
        Console.WriteLine($"Unrecognized command {command}");
    }
    Console.Clear();
    Console.WriteLine("Command?");
    var input = Console.ReadLine() ?? string.Empty;
    var inputArray = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    command = inputArray[0];
    commandArgs = inputArray[1..];
}
while (!QuitCommands.Contains(command, StringComparer.CurrentCultureIgnoreCase));