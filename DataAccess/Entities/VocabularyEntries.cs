using System.ComponentModel.DataAnnotations;

namespace Lexora.DataAccess.Entities
{
  public class VocabularyEntry
  {
    [Key]
    public string Id { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Word { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Definition { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Example { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsDeleted { get; set; } = false;
  }
}