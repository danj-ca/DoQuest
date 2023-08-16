using System.Text;
using Terminal.Gui;

Database.Startup();

Application.Run<MainTaskEntryWindow>();

Application.Shutdown();

// Define a top-level window
public class MainTaskEntryWindow : Window 
{
    internal TextField _taskText;
    internal TextField _scoreText;
    internal Label _messageArea;
    internal Character _currentCharacter;
    internal Label _characterLabel;
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
            Y = Pos.AnchorEnd(2)
        };

        // TODO Loading data in the window's ctor seems like not a great idea...
        // ...but I haven't yet figured out how best to do otherwise, given Terminal.Gui's docs!
        _currentCharacter = Database.GetCurrentCharacter();
        // TODO Adding a subview like this with its own subviews doesn't get displayed and I'm not sure why
        var charView = new CharacterView(_currentCharacter)
        {
            X = Pos.At(1),
            Y = Pos.Bottom(addButton)
        };
        _characterLabel = new Label
        {
            Text = $"Level {_currentCharacter.Level} {_currentCharacter.Class} (Total Score: {_currentCharacter.TotalScore})",
            X = Pos.At(1),
            Y = Pos.Bottom(addButton)
        };

        Add(recordLabel, _taskText, scoreLabel, _scoreText, addButton, quitButton, _messageArea, _characterLabel);
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
        Database.InsertTask(taskValue, scoreValue, _currentCharacter.Id);
        
        // TODO Award treasure!

        // TODO Maybe select it back out and print the result from the database here
        var greenColour = Application.Driver.MakeColor(Color.BrightYellow, Color.BrightGreen);
        _messageArea.ColorScheme = new ColorScheme() { Normal = greenColour };
        _messageArea.Text = $"Added {taskValue} (score: {scoreValue}) successfully.";

        // Update current character if necessary, then refresh character from DB
        var (leveledUp, newLevel) = Levelator.CheckForLevelUp(_currentCharacter);

        if (leveledUp)
        {
            var newMessage = new StringBuilder(_messageArea.Text.ToString());
            newMessage.AppendLine();
            newMessage.Append($"🥳 {_currentCharacter.Name} reached Level {newLevel}! ✨");
            _messageArea.Text = newMessage.ToString();
        }

        _currentCharacter = Database.GetCurrentCharacter();
        _characterLabel.Text = $"Level {_currentCharacter.Level} {_currentCharacter.Class} (Total Score: {_currentCharacter.TotalScore})";


        ResetForm();
    }

    private void ResetForm()
    {
        _taskText.Text = string.Empty;
        _scoreText.Text = string.Empty;
        _taskText.SetFocus();
    }
}

public class CharacterView : FrameView
{
    public CharacterView(Character c)
    {
        _character = c;
        Title = _character.Name;
        // TODO it would be neat to have like ASCII art for the character in here someday
        var levelClassLabel = new Label { Text = $"Level {_character.Level} {_character.Class}" };

        Add(levelClassLabel);
    }
    internal readonly Character _character;
}