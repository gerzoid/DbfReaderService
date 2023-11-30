using DbfReader.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfReader.Services
{
    public interface IDbfReader
    {
        public Task<object?> ExecuteCommandScalarAsync(string command);
        public Task<ResultQuery> ExecuteCommandReaderAsync(string command);
        public Task<int> ExecuteCommandNonQueryAsync(string command);
    }
}
