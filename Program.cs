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
    internal Label _messageArea;
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

        _messageArea = new Label()
        {
            X = Pos.Center(),
            Y = Pos.AnchorEnd(1)
        };

        Add(recordLabel, _taskText, scoreLabel, _scoreText, addButton, quitButton, _messageArea);
    }

    private void RecordNewTask()
    {
        // Validate task fields
        if (string.IsNullOrWhiteSpace(_taskText.Text.ToString()))
        {
            _messageArea.Text = $"Missing task name! You entered '{_taskText.Text}'";
            ResetForm();
            // TODO Multiple returns, though? Really?
            return;
        }
        var taskValue = (string)_taskText.Text;

        if (string.IsNullOrWhiteSpace(_scoreText.Text.ToString()) ||
            !int.TryParse(_scoreText.Text.ToString(), out int scoreValue))
        {
            _messageArea.Text = $"Missing or non-numeric score! You entered '{_scoreText.Text}'";
            ResetForm();
            return;
        }

        // TODO Add error handling in case adding to the db doesn't work
        Database.InsertTask(taskValue, scoreValue);

        // TODO Maybe select it back out and print the result from the database here
        // TODO How do I change the colours?
        _messageArea.Text = $"Added {taskValue} (score: {scoreValue}) successfully.";
        ResetForm();
    }

    private void ResetForm()
    {
        _taskText.Text = string.Empty;
        _scoreText.Text = string.Empty;
        _taskText.SetFocus();
    }
}