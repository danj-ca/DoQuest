using Microsoft.Data.Sqlite;

static class Database
{
    const string DatabaseFileName = "test.sql";
    const string TasksTableDefinition = """
        CREATE TABLE tasks (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            timestamp TEXT NOT NULL,
            name TEXT NOT NULL,
            score INTEGER NOT NULL
        );
    """;

    static void CreateTasksTable(string databaseName = ":memory:")
    {
        using var connection = new SqliteConnection($"Data Source={databaseName}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = TasksTableDefinition;
        command.ExecuteNonQuery();
    }



    public static string TestDatabase(string name, int score)
    {
        CreateTasksTable(DatabaseFileName);
        InsertTask(name, score, DatabaseFileName);

        using var connection = new SqliteConnection($"Data Source={DatabaseFileName}");
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
    /// <param name="databaseName">Filename of the target database; if not specified, in-memory database is used</param>
    static void InsertTask(string name, int score, string databaseName = ":memory:")
    {
        using var connection = new SqliteConnection($"Data Source={databaseName}");
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