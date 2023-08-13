using System.Globalization;
using System.Text;
using Microsoft.Data.Sqlite;

// TODO Pull initialization stuff into its own class, 
//      only run it on startup if database doesn't exist
//      (maybe ask user to confirm)
// TODO Need a better way to change the database over time
//      instead of starting over... might need to explore
//      ways of doing proper migrations
static class Database
{
    const string DatabaseFileName = "DoQuest.sql";
    const string ConnectionString = $"Data Source={DatabaseFileName}";
    const string DatabaseVersion = "0.5.0";
    const string StaticLevelTableDefinition = """
        CREATE TABLE level (
            level INTEGER NOT NULL PRIMARY KEY,
            score_required INTEGER NOT NULL
        );
    """;
    // TODO If we're gonna define static contents in these strings,
    //      they should probably get stuck in their own files at some point.
    const string StaticPowerTableDefinition = """
        CREATE TABLE power (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            colour TEXT NOT NULL
        );
        INSERT INTO power (name, colour)
        VALUES ('Creation', 'Cyan'),
               ('Destruction', 'Magenta'),
               ('Light', 'White'),
               ('Shadow', 'Black'),
               ('Air', 'Yellow'),
               ('Earth', 'Green'),
               ('Fire', 'Red'),
               ('Water', 'Blue')
    """;
    const string StaticTreasureTableDefinition = """
        CREATE TABLE treasure (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            description TEXT NOT NULL,
            min_score INTEGER NOT NULL,
            max_score INTEGER NOT NULL,
            value INTEGER NOT NULL,
            rarity TEXT NOT NULL
        );
    """;
    const string StaticTreasureForeignKeyAdditions = """
        ALTER TABLE treasure ADD COLUMN power_id INTEGER REFERENCES power (id);
        ALTER TABLE treasure ADD COLUMN required_class_id INTEGER REFERENCES class (id);
    """;
    const string StaticClassTableDefinition = """
        CREATE TABLE class (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            description TEXT NOT NULL,
            min_level INTEGER,
            parent_class_id INTEGER REFERENCES class (id)
        );
        INSERT INTO class (id, name, description)
        VALUES (1, 'Adventurer', 'You begin your life of adventure as an Adventurer... but who knows what you can become?');
    """;
    const string StaticClassForeignKeyAdditions = """
        ALTER TABLE class ADD COLUMN power_id INTEGER REFERENCES power (id);
        ALTER TABLE class ADD COLUMN required_treasure_id INTEGER REFERENCES treasure (id);
    """;
    const string TaskTableDefinition = """
        CREATE TABLE task (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            timestamp TEXT NOT NULL,
            name TEXT NOT NULL,
            score INTEGER NOT NULL
        );
    """;
    const string CharacterTableDefinition = """
        CREATE TABLE character (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            created_date TEXT NOT NULL,
            name TEXT NOT NULL,
            level INTEGER NOT NULL DEFAULT 1,
            is_current INTEGER NOT NULL DEFAULT TRUE,
            class_id INTEGER NOT NULL DEFAULT 1 REFERENCES class (id)  
        )
    """;
    /// <summary>
    /// Since the purpose of this table is to store the current database version,
    /// create the table and insert the version in one command
    /// </summary>
    const string VersionTableDefinition = """
        CREATE TABLE version (
            version_number TEXT NOT NULL
        );
        INSERT INTO version (version_number)
        VALUES ($version_number);
    """;

    static bool DatabaseFileExists() => File.Exists(DatabaseFileName);
    static (bool, string?) DatabaseVersionSet()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT version_number
            FROM version;
        """;
        var versionNumber = command.ExecuteScalar() as string;
        return (!string.IsNullOrWhiteSpace(versionNumber), versionNumber);
    }
    static bool CurrentCharacterExists()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id
            FROM character
            WHERE is_current = TRUE;
        """;
        var currentCharacterId = command.ExecuteScalar() as Nullable<Int64>;
        return currentCharacterId.HasValue;
    }

    static void InitializeDatabase()
    {
        CreateStaticTables();
        CreateTaskTable();
        CreateCharacterTable();
        CreateVersionTable();
    }

    static void ValidateDatabase()
    {
        var (versionExists, versionNumber) = DatabaseVersionSet();

        if (!versionExists)
        {
            throw new InvalidDataException($"The database file {DatabaseFileName} exists, but isn't properly initialized. It doesn't contain a version number.");
        }

        if (!string.Equals(versionNumber, DatabaseVersion, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidDataException($"The database file {DatabaseFileName} is version {versionNumber}, but the expected version is {DatabaseVersion}");
        }
    }

    static void CreateTaskTable() => ExecuteNonQuery(TaskTableDefinition);

    static void CreateCharacterTable() => ExecuteNonQuery(CharacterTableDefinition);

    static void CreateStaticTables()
    {
        ExecuteNonQuery(StaticLevelTableDefinition);
        FillLevelTable();
        ExecuteNonQuery(StaticPowerTableDefinition);
        ExecuteNonQuery(StaticClassTableDefinition);
        ExecuteNonQuery(StaticTreasureTableDefinition);
        // Since some of the static tables reference each other,
        // we can't add their foreign key columns at creation time!
        ExecuteNonQuery(StaticClassForeignKeyAdditions);
        ExecuteNonQuery(StaticTreasureForeignKeyAdditions);
        // At last, fill the static tables whose contents are non-trivial
        FillClassTable();
        FillTreasureTable();
    }

    /// <summary>
    /// This method contains the algorithm used to determine how
    /// many points are required to gain each level.
    /// If we want to adjust the leveling curve, this is where to do it.
    /// </summary>
    static void FillLevelTable()
    {
        var valuesToInsert = new List<string>() { "(1,0)" };
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO level (level, score_required)");
        sb.AppendLine("VALUES");

        // Current algo

        // An estimate of how many points you'll record on an average day
        var meanPointsPerDay = 68.9;
        // An estimate of how many days it ought to take to gain a level
        var avgDaysPerLevel = 3.65;
        // As the name implies, adjusting this value increases or decreases 
        // the "distance" between subsequent levels
        var tweakableLevellingConstant = 24.25;
        for (int i = 2; i < 101; i++)
        {
            var score_required = (int)Math.Ceiling(meanPointsPerDay * avgDaysPerLevel * (1 + i / tweakableLevellingConstant));
            valuesToInsert.Add($"({i},{score_required})");
        }
        sb.AppendJoin(',', valuesToInsert);

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = sb.ToString();
        command.ExecuteNonQuery();
    }
    // TODO Lay out the classes in some data structure and insert from there; beats hard coding them all in a SQL string!
    static void FillClassTable() { }
    // TODO Procedurally generate treasure? Or use data structures as for Class...
    static void FillTreasureTable() { }
    static void CreateCharacter()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO character (name, created_date)
            VALUES ($name, $created_date);
        """;
        command.Parameters.AddWithValue("$created_date", DateTime.Now.ToString("O"));
        command.Parameters.AddWithValue("$name", GenerateCharacterName());
        command.ExecuteNonQuery();
    }

    private static string GenerateCharacterName()
    {
        // TODO Random fantasy name generator goes here
        return "Bob";
    }

    static void CreateVersionTable()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = VersionTableDefinition;
        command.Parameters.AddWithValue("$version_number", DatabaseVersion);
        command.ExecuteNonQuery();
    }

    static void ExecuteNonQuery(string sql)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }



    public static string TestDatabase(string name, int score)
    {
        if (!DatabaseFileExists())
        {
            InitializeDatabase();
        }

        ValidateDatabase();

        InsertTask(name, score);

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var queryCommand = connection.CreateCommand();
        queryCommand.CommandText = """
            SELECT id, timestamp, name, score
            FROM task
        """;
        using var reader = queryCommand.ExecuteReader();
        var result = string.Empty;
        while (reader.Read())
        {
            result = $"{reader.GetInt32(0)} // {reader.GetString(1)} // {reader.GetString(2)} // {reader.GetInt32(3)}";
        }

        return result;

        // last thing to create is a "Version" table
        // then check if it exists to know that a given
        // database file has already been initialized.

    }

    /// <summary>
    /// Get the currently active character's information from the database
    /// </summary>
    /// <returns>A Character record for the currently active character</returns>
    public static Character GetCurrentCharacter()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var queryCommand = connection.CreateCommand();
        queryCommand.CommandText = """
            SELECT c.id, c.name, c.level, cl.name AS class_name, c.is_current, c.created_date
            FROM character c
            JOIN class cl ON cl.id = c.class_id
            WHERE c.is_current = TRUE;
        """;
        using var reader = queryCommand.ExecuteReader();
        reader.Read();

        var id = reader.GetInt32(0);
        var name = reader.GetString(1);
        var level = reader.GetInt32(2);
        var charClass = reader.GetString(3);
        var isCurrent = reader.GetBoolean(4);
        var createdDate = DateTime.ParseExact(reader.GetString(5), "O", CultureInfo.CurrentCulture);

        // Sanity check: We only expect there to be one is_current row
        var moreRows = reader.Read();
        if (moreRows)
        {
            throw new InvalidDataException("There are multiple current characters in the database!");
        }

        return new Character(name, id, level, charClass, isCurrent, createdDate);
    }

    /// <summary>
    /// Inserts a new task, using the call time as the task's timestamp
    /// </summary>
    /// <param name="name">Description of the task</param>
    /// <param name="score">The task's "score" value as a positive integer</param>
    public static void InsertTask(string name, int score)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO task (timestamp, name, score)
            VALUES ($timestamp, $name, $score);
        """;
        command.Parameters.AddWithValue("$timestamp", DateTime.Now.ToString("O"));
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$score", score);
        command.ExecuteNonQuery();
    }

    public static void Startup()
    {
        // check for database file exists
        // if not exists, create it
        if (!DatabaseFileExists())
        {
            InitializeDatabase();
        }
        // if exists, validate it
        ValidateDatabase();
        // if not valid, raise error to be reported
        // if no current character exists... create one? or report and let user interact?
        if (!CurrentCharacterExists())
        {
            CreateCharacter();
        }
        // if valid database and current character, return success - or, this method should be a no-op from the DB and UI perspective
    }
}