using Microsoft.Data.Sqlite;

static class Database
{
    public static void TestDatabase()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE tasks (
                id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                timestamp TEXT NOT NULL,
                name TEXT NOT NULL,
                score INTEGER NOT NULL
            );

            INSERT INTO tasks (timestamp, name, score)
            VALUES ($timestamp, 'Test task', 10);
        """;
        command.Parameters.AddWithValue("$timestamp", DateTime.Now.ToString("O"));
        command.ExecuteNonQuery();

        var queryCommand = connection.CreateCommand();
        queryCommand.CommandText = """
            SELECT id, timestamp, name, score
            FROM tasks
        """;
        using var reader = queryCommand.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var timestamp = reader.GetString(1);
            var name = reader.GetString(2);
            var score = reader.GetInt32(3);
            Console.WriteLine($"{id} // {timestamp} // {name} // {score}");
        }

        // last thing to create is a "Version" table
        // then check if it exists to know that a given
        // database file has already been initialized.

    }
}