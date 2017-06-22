using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite.Internal;
using System.Data;

namespace SQliteAccessLayer
{
    public class DBManager
    {
        public static SqliteConnection sqliteConnection = null;

        public DBManager(string connectionString)
        {
            ConnectionString = connectionString;
            this.ConnectDatabase(ConnectionString);
        }

        public String ConnectionString { get; set; }

        public bool ConnectDatabase(string connectionString)
        {
            sqliteConnection = new SqliteConnection(connectionString);
            sqliteConnection.Open();
            return true;
        }

        public SqliteDataReader ExecuteReader(string commandText)
        {
            SqliteCommand command = new SqliteCommand();
            command.Connection = sqliteConnection;
            command.CommandText = commandText;
            return command.ExecuteReader();
        }

        public void ExecuteNonQuery(string commandText)
        {
            SqliteCommand command = new SqliteCommand();
            command.Connection = sqliteConnection;
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }

        public object ExecuteScalar(string commandText)
        {
            SqliteCommand command = new SqliteCommand();
            command.Connection = sqliteConnection;
            command.CommandText = commandText;
            return command.ExecuteScalar();
        }

        public bool IsConnectionOpen()
        {
            return ConnectionState.Open == sqliteConnection.State;
        }

        public bool CloseConnection()
        {
            sqliteConnection.Close();
            return true;
        }

        public bool OpenConnection()
        {
            if (sqliteConnection.State==ConnectionState.Closed)
            {
                sqliteConnection.Open();
            }

            return true;
        }
    }
}
