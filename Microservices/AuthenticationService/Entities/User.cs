using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementModelLayer.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;
    
    public string Role { get; set; } = "User";


    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}

public class UserSession
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string JwtTokenId { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    [Required]
    public string IpAddress { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public DateTime? RevokedAt { get; set; }
}

public class UserOperation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    public int QuantityMeasurementId { get; set; }

    [ForeignKey("QuantityMeasurementId")]
    public virtual QuantityMeasurementEntity QuantityMeasurement { get; set; } = null!;

    [Required]
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string OperationType { get; set; } = string.Empty; // CONVERT, ADD, COMPARE, etc.
}
