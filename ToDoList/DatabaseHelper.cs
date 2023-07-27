using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography.X509Certificates;

public static class DatabaseHelper
{
    //public static string DbFilePath() 
    //{
    //    //Obtem o caminho para a pasta Files do projeto. Essa pasta será usada para armazenar o BD
    //    string dbFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

    //    //Criar diretório se não existir
    //    if (!Directory.Exists(dbFolderPath)) {
    //        Directory.CreateDirectory(dbFolderPath);
    //    }

    //    //Combina o diretorio da pasta "Files" com o arquivo "BancoDados.db"
    //    string dbFilePath = Path.Combine(dbFolderPath, "BancoDadosTodo.db");

    //    return dbFilePath;
    //}

    public static string ConnectionString { get; set; } = @"Data Source=Files\\BancoDados.db;Version=3";

    public static void InitializeDatabase()
    {

        if(!File.Exists(@"Files\\BancoDados.db")) 
        {
            SQLiteConnection.CreateFile(@"Files\\BancoDados.db");

            using (var connection = new SQLiteConnection(ConnectionString)) 
            {
                connection.Open();

                //String para criar tables
                string createTodoTable = @"
                       CREATE TABLE IF NOT EXISTS Todo (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL);";


                using (var command = new SQLiteCommand(connection)) 
                {
                    command.CommandText = createTodoTable;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}