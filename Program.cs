var command = args[0];
var commandArgs = args[1..];

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