using SimpleLsSample.Interfaces;
using SimpleLsSample.Models;

namespace SimpleLsSample.DAL.DAO
{
    public class ResultDao : IResultDao
    {
        public virtual List<Result> GetResults(string search)
        {
            return new List<Result>();
        }
    }
}
