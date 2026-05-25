using Lexora.Base;
using Lexora.DataAccess.Entities;
using Lexora.DTOs;
namespace Lexora.Repositories;

public interface IVocabularyEntryRepository
{
  public Task<ReturnBase<string>> InsertVocabularyEntryAsync(VocabularyEntry entry);
  public Task<ReturnBase<string>> UpdateVocabularyEntryAsync(VocabularyEntry entry);
  public Task<ReturnBase<string>> DeleteVocabularyEntryAsync(DeleteVocabularyEntryDto deleteDto);
  public Task<ReturnBase<VocabularyEntry>> GetVocabularyEntryByIdAsync(GetVocabularyEntryByIdDto getDto);
  public Task<ReturnBase<IQueryable<VocabularyEntry>>> GetVocabularyEntriesPaginatedAsync(GetVocabularyEntriesPaginatedDto getDto);
  public Task<ReturnBase<IQueryable<VocabularyEntry>>> SearchVocabularyEntryByWordAsync(SearchVocabularyEntryDtoByWord searchDto);
  public Task<ReturnBase<IQueryable<VocabularyEntry>>> SearchVocabularyEntryByDefinitionAsync(SearchVocabularyEntryDtoByDefinition searchDto);
  public Task<ReturnBase<IQueryable<VocabularyEntry>>> SearchVocabularyEntryByExampleAsync(string searchQuery);
}