using DbfReader.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Data;
using System.Web.Http.Cors;
using System.Diagnostics;
using Humanizer;
using System.Web.Http.Results;

namespace DbfReader.Controllers
{
    [ApiController]
    [Route("api/dbf/[action]")]
    public class DbfController : Controller
    {
        AbstractDbfReader _reader;
        IConfiguration _config;

        public DbfController(AbstractDbfReader reader, IConfiguration config)
        {
            _reader = reader;
            _config = config;
        }

        [HttpGet(Name = "GetScalar")]
        public async Task<Object?> GetScalar([FromForm] string query1, [FromForm] string basepath="tfoms")
        {
            _reader.SetConnectionString(_config[$"Bases:{basepath}"]);
            return await _reader.ExecuteCommandScalarAsync(query1);
        }

        [HttpPost]
        public async Task<IActionResult> GetReaderTable([FromForm] string query, [FromForm] string basepath)
        {
            _reader.SetConnectionString(_config[$"Bases:{basepath}"]);
            var result = await _reader.ExecuteCommandReaderAsync(query);
            if (result.StatusOk)
            {                
                Console.WriteLine($"PrivateMemorySize64 = {Process.GetCurrentProcess().PrivateMemorySize64.Bytes().Humanize()} WorkingSet64= {Process.GetCurrentProcess().WorkingSet64.Bytes().Humanize()} Gc = {GC.GetTotalMemory(true).Bytes().Humanize()}");
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(result.DataTable));
            }
            else
                return BadRequest(result.Errror);                      
        }

        [HttpPost]
        public async Task<Object?> Update([FromForm] string query, [FromForm] string basepath = "tfoms")
        {
            _reader.SetConnectionString(_config[$"Bases:{basepath}"]);
            return await _reader.ExecuteCommandNonQueryAsync(query);
        }

    }
}
