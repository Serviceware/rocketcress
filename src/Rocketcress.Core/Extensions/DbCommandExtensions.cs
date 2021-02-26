using System;
using System.Data;

namespace Rocketcress.Core.Extensions
{
    /// <summary>
    /// Provides extensions for the <see cref="IDbCommand"/> interface.
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Adds a parameter with a specified value.
        /// </summary>
        /// <param name="command">The command to add the parameter to.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        public static void AddParameterWithValue(this IDbCommand command, string paramName, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = paramName;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }
    }
}
