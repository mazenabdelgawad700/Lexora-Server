namespace Lexora.DTOs;

public class InsertVocabularyEntryDto
{
  public string Word { get; set; } = null!;
  public string Definition { get; set; } = null!;
  public string Example { get; set; } = null!;
}
