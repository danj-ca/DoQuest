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
    internal TextField _taskText;
    internal TextField _scoreText;
    public ExampleWindow()
    {
        Title = "Example-fu";

        var recordLabel = new Label()
        {
            Text = "Record a task:"
        };

        _taskText = new TextField()
        {
            X = Pos.Right(recordLabel) + 1,
            Width = 50
        };

        var scoreLabel = new Label()
        {
            Text = "Score:",
            X = Pos.Right(_taskText) + 1
        };

        _scoreText = new TextField()
        {
            X = Pos.Right(scoreLabel) + 1,
            Width = 10
        };

        var addButton = new Button()
        {
            Text = "Add Task",
            X = Pos.Center(),
            Y = Pos.Bottom(_taskText)
        };

        addButton.Clicked += () => RecordNewTask();

        var quitButton = new Button()
        {
            Text = "Quit",
            X = Pos.Right(addButton) + 1,
            Y = Pos.Bottom(_taskText)
        };

        quitButton.Clicked += () => Application.RequestStop();

        Add(recordLabel, _taskText, scoreLabel, _scoreText, addButton, quitButton);
    }

    private void RecordNewTask()
    {
        // Validate task fields
        if (string.IsNullOrWhiteSpace(_taskText.Text.ToString()))
        {
            throw new InvalidDataException($"Missing task name! You entered '{_taskText.Text}'");
        }
        var taskValue = (string)_taskText.Text;

        if (!int.TryParse(_scoreText.Text.ToString(), out int scoreValue))
        {
            throw new InvalidDataException($"Missing or non-numeric score! You entered '{_scoreText.Text}'");
        }

        // TODO Actually entering the task into the database layer goes here
        Database.InsertTask(taskValue, scoreValue);

        _taskText.Text = string.Empty;
        _scoreText.Text = string.Empty;
        _taskText.SetFocus();
    }
}