using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Miracle.Settings.Tests.DatabaseTest
{
    class SampleSqlValueProvider: IValueProvider
    {
        private readonly IDbConnection _connection;

        public SampleSqlValueProvider(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool TryGetValue(string key, out string value)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Value] FROM Setting WHERE [Key]=@key";
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@key";
                parameter.Value = key;
                command.Parameters.Add(parameter);

                value = command.ExecuteScalar() as string;
                return value != null;
            }
        }

        public bool TryGetKeys(string prefix, out IEnumerable<string> keys)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Key] FROM Setting WHERE [Key] LIKE @key ORDER BY [Key]";
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@key";
                parameter.Value = prefix + "%";
                command.Parameters.Add(parameter);

                List<string> keysList = new List<string>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        keysList.Add(reader.GetString(0));
                    }
                }
                keys = keysList;
                return keys.Any();
            }
        }
    }
}
