using ChatBot.Core.Interfaces;

namespace ChatBot.Services.Helpers
{
    public class ReportFactory
    {
        public IReport CreateReport(string format)
        {
            return format.ToLower() switch
            {
                "txt" => new TxtReport(),
                "csv" => new CsvReport(),
                _ => throw new ArgumentException("Формат не підтримується")
            };
        }
    }
}