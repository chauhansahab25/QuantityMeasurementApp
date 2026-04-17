using QuantityMeasurementModelLayer.Entities;

namespace AuthenticationService.DTOs
{
    public class UserActivity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Activity { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UserStatistics
    {
        public int TotalOperations { get; set; }
        public int TotalConversions { get; set; }
        public int TotalComparisons { get; set; }
        public DateTime LastActivity { get; set; }
    }

    public class UserSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
