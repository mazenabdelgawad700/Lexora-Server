using Lexora.Base;
using Lexora.DataAccess.Entities;
using Lexora.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Lexora.Repositories;

public class VocabularyEntryRepository : IVocabularyEntryRepository
{
  private readonly DbContext _dbContext;
  private readonly DbSet<VocabularyEntry> _vocabularyEntrySet;
  // TODO:: add logging

  public VocabularyEntryRepository(DbContext dbContext)
  {
    _dbContext = dbContext;
    _vocabularyEntrySet = _dbContext.Set<VocabularyEntry>();
  }

  public async Task<ReturnBase<string>> DeleteVocabularyEntryAsync(DeleteVocabularyEntryDto deleteDto)
  {
    try
    {
      var entity = await _vocabularyEntrySet.FirstOrDefaultAsync(e => e.Id == deleteDto.Id);

      if (entity is null)
        return ReturnBaseHandler.Failed<string>($"Vocabulary with id: ({deleteDto.Id}) is not exist?!, is not it already  deleted?");

      var removeResult = _vocabularyEntrySet.Remove(entity);

      if (removeResult is null)
        return ReturnBaseHandler.Failed<string>("Failed to delete vocabulary");

      var saveChanges = await _dbContext.SaveChangesAsync();

      if (saveChanges <= 0)
        return ReturnBaseHandler.Failed<string>("Failed to delete vocabulary");

      return ReturnBaseHandler.Success("Vocabulary deleted successffully");
    }
    catch (Exception ex)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<string>("Failed to delete vocabulary");
    }
  }

  public async Task<ReturnBase<IQueryable<VocabularyEntry>>> GetVocabularyEntriesPaginatedAsync(GetVocabularyEntriesPaginatedDto getDto)
  {
    try
    {
      var skipAmount = (getDto.Page - 1) * getDto.PageSize;

      var entities = _vocabularyEntrySet.Skip(skipAmount).Take(getDto.PageSize).AsNoTracking();

      if (entities is null)
        return ReturnBaseHandler.Failed<IQueryable<VocabularyEntry>>($"Can not get vocabularies!");

      return ReturnBaseHandler.Success(entities);
    }
    catch (Exception ex)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<IQueryable<VocabularyEntry>>("Failed to get vocabularies");
    }
  }
  public async Task<ReturnBase<VocabularyEntry>> GetVocabularyEntryByIdAsync(GetVocabularyEntryByIdDto getDto)
  {
    try
    {
      var entity = await _vocabularyEntrySet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == getDto.Id);

      if (entity is null)
        return ReturnBaseHandler.Failed<VocabularyEntry>($"Vocabulary with id:({getDto.Id}) is not exist?!, is not it deleted?");

      return ReturnBaseHandler.Success(entity);
    }
    catch (Exception ex)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<VocabularyEntry>("Failed to get vocabulary");
    }
  }
  public async Task<ReturnBase<string>> InsertVocabularyEntryAsync(VocabularyEntry entry)
  {
    try
    {
      var insertResult = await _vocabularyEntrySet.AddAsync(entry);

      if (insertResult is null)
        return ReturnBaseHandler.Failed<string>("Failed to add vocabulary, is your input valid?");

      var saveChanges = await _dbContext.SaveChangesAsync();

      if (saveChanges <= 0)
        return ReturnBaseHandler.Failed<string>("Failed to add vocabulary, is your input valid?");

      return ReturnBaseHandler.Success("Vocabulary added successffully");
    }
    catch (Exception ex)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<string>("Failed to add vocabulary, is your input valid?");
    }
  }
  public async Task<ReturnBase<IQueryable<VocabularyEntry>>> SearchVocabularyEntryByWordAsync(SearchVocabularyEntryDtoByWord searchDto)
  {
    try
    {
      var vocabulariesResult = _vocabularyEntrySet.Where(v => v.Word.Contains(searchDto.SearchQuery));

      if (vocabulariesResult is null)
        return ReturnBaseHandler.Failed<IQueryable<VocabularyEntry>>("Failed to get vocabularies");

      return ReturnBaseHandler.Success(vocabulariesResult);
    }
    catch (Exception ex)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<IQueryable<VocabularyEntry>>("Failed to get vocabularies");
    }
  }
  public async Task<ReturnBase<IQueryable<VocabularyEntry>>> SearchVocabularyEntryByWordAsync(SearchVocabularyEntryDtoByDefinition searchDto)
  {
    try
    {
      var vocabulariesResult = _vocabularyEntrySet.Where(v => v.Definition.Contains(searchDto.SearchQuery));

      if (vocabulariesResult is null)
        return ReturnBaseHandler.Failed<IQueryable<VocabularyEntry>>("Failed to get vocabularies");

      return ReturnBaseHandler.Success(vocabulariesResult);
    }
    catch (Exception ex)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<IQueryable<VocabularyEntry>>("Failed to get vocabularies");
    }
  }
  public async Task<ReturnBase<string>> UpdateVocabularyEntryAsync(VocabularyEntry entry)
  {
    try
    {
      var entity = await _vocabularyEntrySet.FirstOrDefaultAsync(e => e.Id == entry.Id);

      if (entity is null)
        return ReturnBaseHandler.Failed<string>($"Vocabulary: {entry.Word} is not exist?!, is not it deleted?");

      var updateResult = _vocabularyEntrySet.Update(entity);

      if (updateResult is null)
        return ReturnBaseHandler.Failed<string>("Failed to update vocabulary, is your input valid?");

      var saveChanges = await _dbContext.SaveChangesAsync();

      if (saveChanges <= 0)
        return ReturnBaseHandler.Failed<string>("Failed to update vocabulary, is your input valid?");

      return ReturnBaseHandler.Success("Vocabulary updated successffully");
    }
    catch (Exception ex)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<string>("Failed to update vocabulary, is your input valid?");
    }
  }
}
