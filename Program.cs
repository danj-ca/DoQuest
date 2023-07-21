var command = args[0];
var commandArgs = args[1..];

if (string.Equals(command, "record"))
{
    var taskName = commandArgs[0];
    var score = int.Parse(commandArgs[1]);
    Console.WriteLine($"You want to record the task \"{taskName}\" worth {score} points!");
}
else 
{
    Console.WriteLine($"Unrecognized command {command}");
}