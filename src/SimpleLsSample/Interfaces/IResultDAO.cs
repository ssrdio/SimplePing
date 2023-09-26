using SimpleLsSample.Models;

namespace SimpleLsSample.Interfaces
{
    public interface IResultDao
    {
        List<Result> GetResults(string search);
    }
}
