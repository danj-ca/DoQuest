using Terminal.Gui;

Application.Run<MainTaskEntryWindow>();

var QuitCommands = new List<string> { "q", "quit", "exit" };

// TODO Load the current character object from the database
// TODO Maybe load other stuff from the database that we'll need later?

Application.Shutdown();

// Define a top-level window
public class MainTaskEntryWindow : Window 
{
    internal TextField _taskText;
    internal TextField _scoreText;
    internal Label _messageArea;
    public MainTaskEntryWindow()
    {
        Title = "DoQuest";

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
            Y = Pos.AnchorEnd(1)
        };

        Add(recordLabel, _taskText, scoreLabel, _scoreText, addButton, quitButton, _messageArea);
    }

    private void RecordNewTask()
    {
        // Validate task fields
        if (string.IsNullOrWhiteSpace(_taskText.Text.ToString()))
        {
            var redColour = Application.Driver.MakeColor(Color.White, Color.BrightRed);
            _messageArea.ColorScheme = new ColorScheme() { Normal = redColour };
            _messageArea.Text = $"Missing task name! You entered '{_taskText.Text}'";
            ResetForm();
            // TODO Multiple returns, though? Really?
            return;
        }
        var taskValue = (string)_taskText.Text;

        if (string.IsNullOrWhiteSpace(_scoreText.Text.ToString()) ||
            !int.TryParse(_scoreText.Text.ToString(), out int scoreValue))
        {
            var redColour = Application.Driver.MakeColor(Color.White, Color.BrightRed);
            _messageArea.ColorScheme = new ColorScheme() { Normal = redColour };
            _messageArea.Text = $"Missing or non-numeric score! You entered '{_scoreText.Text}'";
            ResetForm();
            return;
        }

        // TODO Add error handling in case adding to the db doesn't work
        Database.InsertTask(taskValue, scoreValue);

        // TODO Maybe select it back out and print the result from the database here
        var greenColour = Application.Driver.MakeColor(Color.BrightYellow, Color.BrightGreen);
        _messageArea.ColorScheme = new ColorScheme() { Normal = greenColour };
        _messageArea.Text = $"Added {taskValue} (score: {scoreValue}) successfully.";

        var character = Database.GetCurrentCharacter();
        // TODO Levelator.CheckForLevelUp(newTotalScore);

        ResetForm();
    }

    private void ResetForm()
    {
        _taskText.Text = string.Empty;
        _scoreText.Text = string.Empty;
        _taskText.SetFocus();
    }
}