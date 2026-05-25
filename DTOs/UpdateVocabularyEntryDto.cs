namespace Lexora.DTOs;

public class UpdateVocabularyEntryDto
{
  public string Id { get; set; } = null!;
  public string Word { get; set; } = null!;
  public string Definition { get; set; } = null!;
  public string Example { get; set; } = null!;
}