using System.Data;

#nullable disable

namespace Rocketcress.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the DbConnection class.
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Creates a new DbCommand.
        /// </summary>
        /// <param name="connection">The connection to create the command on.</param>
        /// <param name="query">The command text to set to the command.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns a new DbCommand object.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string query, params (string Name, object Value)[] @params)
            => CreateCommand(connection, query, null, @params);

        /// <summary>
        /// Creates a new DbCommand.
        /// </summary>
        /// <param name="connection">The connection to create the command on.</param>
        /// <param name="query">The command text to set to the command.</param>
        /// <returns>Returns a new DbCommand object.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string query)
            => CreateCommand(connection, query, null, null);

        /// <summary>
        /// Creates a new DbCommand.
        /// </summary>
        /// <param name="connection">The connection to create the command on.</param>
        /// <param name="query">The command text to set to the command.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <returns>Returns a new DbCommand object.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string query, int? timeout)
            => CreateCommand(connection, query, timeout, null);

        /// <summary>
        /// Creates a new DbCommand.
        /// </summary>
        /// <param name="connection">The connection to create the command on.</param>
        /// <param name="query">The command text to set to the command.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns a new DbCommand object.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string query, int? timeout, params (string Name, object Value)[] @params)
        {
            OpenIfNecessary(connection);
            var command = connection.CreateCommand();
            command.CommandText = query;
            if (@params != null)
            {
                foreach (var (name, value) in @params)
                    command.AddParameterWithValue(name, value);
            }

            if (timeout.HasValue)
                command.CommandTimeout = timeout.Value;
            return command;
        }

        /// <summary>
        /// Executes a query and returns a scalar value.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns the first column of the first row of the result. If no rows are available, null is returned.</returns>
        public static object ExecuteScalar(this IDbConnection connection, string query, params (string Name, object Value)[] @params)
            => ExecuteScalar(connection, query, null, @params);

        /// <summary>
        /// Executes a query and returns a scalar value.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>Returns the first column of the first row of the result. If no rows are available, null is returned.</returns>
        public static object ExecuteScalar(this IDbConnection connection, string query)
            => ExecuteScalar(connection, query, null, null);

        /// <summary>
        /// Executes a query and returns a scalar value.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <returns>Returns the first column of the first row of the result. If no rows are available, null is returned.</returns>
        public static object ExecuteScalar(this IDbConnection connection, string query, int? timeout)
            => ExecuteScalar(connection, query, timeout, null);

        /// <summary>
        /// Executes a query and returns a scalar value.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns the first column of the first row of the result. If no rows are available, null is returned.</returns>
        public static object ExecuteScalar(this IDbConnection connection, string query, int? timeout, params (string Name, object Value)[] @params)
        {
            using var cmd = CreateCommand(connection, query, timeout, @params);
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Executes a query and returns a DbDataReader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns the data as a DbDataReader.</returns>
        public static IDataReader ExecuteReader(this IDbConnection connection, string query, params (string Name, object Value)[] @params)
            => ExecuteReader(connection, query, null, @params);

        /// <summary>
        /// Executes a query and returns a DbDataReader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>Returns the data as a DbDataReader.</returns>
        public static IDataReader ExecuteReader(this IDbConnection connection, string query)
            => ExecuteReader(connection, query, null, null);

        /// <summary>
        /// Executes a query and returns a DbDataReader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <returns>Returns the data as a DbDataReader.</returns>
        public static IDataReader ExecuteReader(this IDbConnection connection, string query, int? timeout)
            => ExecuteReader(connection, query, timeout, null);

        /// <summary>
        /// Executes a query and returns a DbDataReader.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns the data as a DbDataReader.</returns>
        public static IDataReader ExecuteReader(this IDbConnection connection, string query, int? timeout, params (string Name, object Value)[] @params)
        {
            var cmd = CreateCommand(connection, query, timeout, @params);
            return new DataReaderWrapper(cmd, cmd.ExecuteReader());
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns the number of affected rows by the query..</returns>
        public static int ExecuteNonQuery(this IDbConnection connection, string query, params (string Name, object Value)[] @params)
            => ExecuteNonQuery(connection, query, null, @params);

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>Returns the number of affected rows by the query..</returns>
        public static int ExecuteNonQuery(this IDbConnection connection, string query)
            => ExecuteNonQuery(connection, query, null, null);

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <returns>Returns the number of affected rows by the query..</returns>
        public static int ExecuteNonQuery(this IDbConnection connection, string query, int? timeout)
            => ExecuteNonQuery(connection, query, timeout, null);

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="timeout">The timeout for the command.</param>
        /// <param name="params">The parameters for the command.</param>
        /// <returns>Returns the number of affected rows by the query..</returns>
        public static int ExecuteNonQuery(this IDbConnection connection, string query, int? timeout, params (string Name, object Value)[] @params)
        {
            using var cmd = CreateCommand(connection, query, timeout, @params);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Opens the DbConnection if it is not already open.
        /// </summary>
        /// <param name="connection">The connection to open.</param>
        public static void OpenIfNecessary(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        private class DataReaderWrapper : IDataReader
        {
            private readonly IDbCommand _command;
            private readonly IDataReader _reader;

            public DataReaderWrapper(IDbCommand command, IDataReader reader)
            {
                _command = command;
                _reader = reader;
            }

            public object this[int i] => _reader[i];
            public object this[string name] => _reader[name];
            public int Depth => _reader.Depth;
            public bool IsClosed => _reader.IsClosed;
            public int RecordsAffected => _reader.RecordsAffected;
            public int FieldCount => _reader.FieldCount;

            public void Close() => _reader.Close();
            public bool GetBoolean(int i) => _reader.GetBoolean(i);
            public byte GetByte(int i) => _reader.GetByte(i);
            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => _reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
            public char GetChar(int i) => _reader.GetChar(i);
            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => _reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
            public IDataReader GetData(int i) => _reader.GetData(i);
            public string GetDataTypeName(int i) => _reader.GetDataTypeName(i);
            public DateTime GetDateTime(int i) => _reader.GetDateTime(i);
            public decimal GetDecimal(int i) => _reader.GetDecimal(i);
            public double GetDouble(int i) => _reader.GetDouble(i);
            public Type GetFieldType(int i) => _reader.GetFieldType(i);
            public float GetFloat(int i) => _reader.GetFloat(i);
            public Guid GetGuid(int i) => _reader.GetGuid(i);
            public short GetInt16(int i) => _reader.GetInt16(i);
            public int GetInt32(int i) => _reader.GetInt32(i);
            public long GetInt64(int i) => _reader.GetInt64(i);
            public string GetName(int i) => _reader.GetName(i);
            public int GetOrdinal(string name) => _reader.GetOrdinal(name);
            public DataTable GetSchemaTable() => _reader.GetSchemaTable();
            public string GetString(int i) => _reader.GetString(i);
            public object GetValue(int i) => _reader.GetValue(i);
            public int GetValues(object[] values) => _reader.GetValues(values);
            public bool IsDBNull(int i) => _reader.IsDBNull(i);
            public bool NextResult() => _reader.NextResult();
            public bool Read() => _reader.Read();

            public void Dispose()
            {
                _reader.Dispose();
                _command.Dispose();
            }
        }
    }
}
