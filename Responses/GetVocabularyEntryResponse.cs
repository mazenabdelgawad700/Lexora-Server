namespace Lexora.Responses;

public class GetVocabularyEntryResponse
{
  public string Id { get; set; } = null!;
  public string Word { get; set; } = null!;
  public string Definition { get; set; } = null!;
  public string Example { get; set; } = null!;
}