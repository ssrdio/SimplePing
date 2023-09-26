using Microsoft.EntityFrameworkCore;
using SimpleLsSample.DAL.DAO;
using System.Text.RegularExpressions;

namespace SimpleLsSample.Services
{
    public class LoggingService
    {
        private PingerDBContext _dbContext;
        public string Lang => "EN";
        public LoggingService(PingerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogError(string error)
        {
            try
            {
                _dbContext.Add(new Error { Message = error, DateTime= DateTime.Now });
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        public string LogErrorAndGetFriendlyMessage(string error)
        {
            try
            {
                LogError(error);
                Regex regex = new Regex("'(.*?)'");
                string codeId = "";
                if (regex.IsMatch(error))
                {
                    codeId = regex.Match(error).Value.Replace("'", "");
                }
                string friendlyMessage = String.Join(",", _dbContext.FriendlyMessages.FromSqlRaw($"Select * from FriendlyMessages where Language like '{Lang}' and Id={codeId}").Select(t => t.Message));
                if(string.IsNullOrWhiteSpace(friendlyMessage)) 
                {
                    LogError("Friendly message does not exist");
                    return "Unknown error";
                }
                return friendlyMessage;
            }
            catch (Exception ex)
            {
                return "DB not avaliable";
            }
        }
        
    }
}
