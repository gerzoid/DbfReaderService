using System.Data;

namespace DbfReader.Models
{
    public class ResultQuery
    {
        string error;
        public bool StatusOk { get; set; }
        public string Errror { get
            {
                return error;
            } 
            set
            {
                StatusOk = false;
                error = value;
            }
        }
        public DataTable? DataTable { get; set; }
        public ResultQuery() { 
            StatusOk = true;
            DataTable = new DataTable();
        }
    }
}
