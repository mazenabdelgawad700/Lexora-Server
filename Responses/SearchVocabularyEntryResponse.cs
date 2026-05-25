namespace Lexora.Responses;

public class SearchVocabularyEntryResponse
{
  public List<GetVocabularyEntryResponse> Results { get; set; } = new();
  public int TotalCount { get; set; }
}
