using System.ComponentModel;

namespace TopSwagCode.Blazor.Shared
{

    public class WeatherForecast
    {
        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [DisplayName("Temperature C")]
        public int TemperatureC { get; set; }

        [DisplayName("Summary")]
        public string? Summary { get; set; }
    }

    public record AdminWelcomeMessage(string Message);

    public class UploadResult
    {
        public bool Uploaded { get; set; }
        public string FileName { get; set; }
        public string FileGuid { get; set; }
        public int ErrorCode { get; set; }
        public string FileResultUrl { get; set; }
    }
}