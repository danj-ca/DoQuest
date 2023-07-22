using Microsoft.Data.Sqlite;

static class Database
{
    const string DatabaseFileName = "test.sql";
    const string ConnectionString = $"Data Source={DatabaseFileName}";
    const string DatabaseVersion = "0.0.1";
    const string TasksTableDefinition = """
        CREATE TABLE tasks (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            timestamp TEXT NOT NULL,
            name TEXT NOT NULL,
            score INTEGER NOT NULL
        );
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
            FROM version
        """;
        var versionNumber = command.ExecuteScalar() as string;
        return (!string.IsNullOrWhiteSpace(versionNumber), versionNumber);
    }

    static void InitializeDatabase()
    {
        CreateTasksTable();
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

    static void CreateTasksTable()
    {
        CreateTable(TasksTableDefinition);
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

    static void CreateTable(string createSql)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = createSql;
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
            FROM tasks
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
    /// Inserts a new task, using the call time as the task's timestamp
    /// </summary>
    /// <param name="name">Description of the task</param>
    /// <param name="score">The task's "score" value as a positive integer</param>
    static void InsertTask(string name, int score)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO tasks (timestamp, name, score)
            VALUES ($timestamp, $name, $score);
        """;
        command.Parameters.AddWithValue("$timestamp", DateTime.Now.ToString("O"));
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$score", score);
        command.ExecuteNonQuery();
    }
}