using Terminal.Gui;

Application.Run<ExampleWindow>();

var QuitCommands = new List<string> { "q", "quit", "exit" };

// var command = args[0];
// var commandArgs = args[1..];

// do
// {
//     if (string.Equals(command, "record"))
//     {
//         var taskName = commandArgs[0];
//         var score = int.Parse(commandArgs[1]);
//         var result = Database.TestDatabase(taskName, score);
//         Console.WriteLine(result);
//     }
//     else
//     {
//         Console.WriteLine($"Unrecognized command {command}");
//     }
//     Console.Clear();
//     Console.WriteLine("Command?");
//     var input = Console.ReadLine() ?? string.Empty;
//     var inputArray = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
//     command = inputArray[0];
//     commandArgs = inputArray[1..];
// }
// while (!QuitCommands.Contains(command, StringComparer.CurrentCultureIgnoreCase));

Application.Shutdown();

// Define a top-level window
public class ExampleWindow : Window 
{
    public ExampleWindow()
    {
        Title = "Example-fu";

        var recordLabel = new Label()
        {
            Text = "Record a task:"
        };

        var taskText = new TextField()
        {
            X = Pos.Right(recordLabel) + 1,
            Width = Dim.Percent(75, true)
        };

        var scoreText = new TextField()
        {
            X = Pos.Right(taskText) + 1,
            Width = Dim.Fill(2)
        };

        var addButton = new Button()
        {
            Text = "Add Task",
            X = Pos.Center(),
            Y = Pos.Bottom(taskText)
        };

        addButton.Clicked += () => RecordNewTask();

        var quitButton = new Button()
        {
            Text = "Quit",
            X = Pos.Right(addButton) + 1,
            Y = Pos.Bottom(taskText)
        };

        quitButton.Clicked += () => Application.RequestStop();

        Add(recordLabel, taskText, scoreText, addButton, quitButton);
    }

    private void RecordNewTask()
    {
        // TODO Validate task fields
        throw new NotImplementedException();
    }
}