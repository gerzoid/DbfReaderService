using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using DbfReader.Models;
#pragma warning disable CA1416 // Validate platform compatibility

namespace DbfReader.Services
{

    public class AbstractDbfReader : IDbfReader
    {
        private string _connectionString = "";


        public AbstractDbfReader(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal void SetConnectionString(string? connection_string)
        {
            _connectionString = connection_string ?? throw new ArgumentNullException(nameof(connection_string));
        }

       public async Task<object?> ExecuteCommandScalarAsync(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentException("Command is null or empty", nameof(command));
            }
            Object? result = null;
            using (OleDbConnection? conn2 = new OleDbConnection(_connectionString))
            {
                using (OleDbCommand? cmd = new OleDbCommand(command, conn2))
                {
                    await conn2.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ExecScript";
                    cmd.Parameters.Add("myScript", OleDbType.Char).Value = @"SET DATE GERMAN";
                    await cmd.ExecuteNonQueryAsync();

                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();

                   result = await cmd.ExecuteScalarAsync();
                }
                await conn2.CloseAsync();
            }
            return result;
        }

        public async Task<ResultQuery> ExecuteCommandReaderAsync(string command)
        {
            using (OleDbConnection? conn2 = new OleDbConnection(_connectionString))
            {
                using (OleDbCommand? cmd = new OleDbCommand(command, conn2))
                {
                    await conn2.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ExecScript";
                    cmd.Parameters.Add("myScript", OleDbType.Char).Value = @"SET DATE GERMAN";

                    await cmd.ExecuteNonQueryAsync();
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    ResultQuery results = new ResultQuery();

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                                results.DataTable.Load(reader);

                            await reader.DisposeAsync();
                        }
                    }
                    catch (Exception E)
                    {
                        results.Errror = E.Message;
                        Console.WriteLine("error " + E.Message);
                    }
                    await conn2.CloseAsync();

                    return results;
                }
            }

        }

        public async Task<int> ExecuteCommandNonQueryAsync(string command)
        {
            int result = -1;
            using (OleDbConnection? conn2 = new OleDbConnection(_connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(command, conn2))
                {
                    await conn2.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ExecScript";
                    cmd.Parameters.Add("myScript", OleDbType.Char).Value = @"SET DATE GERMAN";
                    await cmd.ExecuteNonQueryAsync();

                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();

                    result = await cmd.ExecuteNonQueryAsync();

                }
                await conn2.CloseAsync();
            }
            return result;
        }

    }
#pragma warning restore CA1416 // Validate platform compatibility
}
