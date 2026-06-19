namespace Lexora.Responses;

public class GetVocabularyEntriesPaginatedResponse
{
  public List<GetVocabularyEntryResponse> VocabularyEntries { get; set; } = [];
  public int TotalCount { get; set; }
}