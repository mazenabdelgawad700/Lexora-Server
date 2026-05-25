using Lexora.Base;
using Lexora.DTOs;
using Lexora.Responses;

namespace Lexora.Services;

public interface IVocabularyEntryService
{
  public Task<ReturnBase<string>> InsertVocabularyEntryAsync(InsertVocabularyEntryDto insertDto);
  public Task<ReturnBase<string>> UpdateVocabularyEntryAsync(UpdateVocabularyEntryDto updateDto);
  public Task<ReturnBase<string>> DeleteVocabularyEntryAsync(DeleteVocabularyEntryDto deleteDto);
  public Task<ReturnBase<GetVocabularyEntryResponse>> GetVocabularyEntryByIdAsync(GetVocabularyEntryByIdDto getDto);
  public Task<ReturnBase<GetVocabularyEntriesPaginatedResponse>> GetVocabularyEntriesPaginatedAsync(GetVocabularyEntriesPaginatedDto getDto);
  public Task<ReturnBase<SearchVocabularyEntryResponse>> SearchVocabularyEntryByWordAsync(SearchVocabularyEntryDtoByWord searchDto);
  public Task<ReturnBase<SearchVocabularyEntryResponse>> SearchVocabularyEntryByDefinitionAsync(SearchVocabularyEntryDtoByDefinition searchDto);
  public Task<ReturnBase<SearchVocabularyEntryResponse>> SearchVocabularyEntryByExampleAsync(string searchQuery);
}