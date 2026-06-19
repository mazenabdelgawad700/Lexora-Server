using Lexora.Base;
using Lexora.DataAccess.Entities;
using Lexora.DTOs;
using Lexora.Repositories;
using Lexora.Responses;

namespace Lexora.Services;

public class VocabularyEntryService : IVocabularyEntryService
{
  private readonly IVocabularyEntryRepository _vocabularyEntryRepository;

  public VocabularyEntryService(IVocabularyEntryRepository vocabularyEntryRepository)
  {
    _vocabularyEntryRepository = vocabularyEntryRepository;
  }

  public async Task<ReturnBase<string>> InsertVocabularyEntryAsync(InsertVocabularyEntryDto insertDto)
  {
    // Validate input
    var validationResult = ValidateInsertDto(insertDto);
    if (!validationResult.Succeeded)
      return validationResult;

    try
    {
      var entity = new VocabularyEntry
      {
        Id = Guid.NewGuid().ToString(),
        Word = insertDto.Word!.Trim(),
        Definition = insertDto.Definition!.Trim(),
        Example = insertDto.Example!.Trim(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        IsDeleted = false
      };

      return await _vocabularyEntryRepository.InsertVocabularyEntryAsync(entity);
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<string>("Failed to insert vocabulary entry");
    }
  }

  public async Task<ReturnBase<string>> UpdateVocabularyEntryAsync(UpdateVocabularyEntryDto updateDto)
  {
    // Validate input
    var validationResult = ValidateUpdateDto(updateDto);
    if (!validationResult.Succeeded)
      return validationResult;

    try
    {
      // Get existing entity
      var getDto = new GetVocabularyEntryByIdDto { Id = updateDto.Id };
      var existingResult = await _vocabularyEntryRepository.GetVocabularyEntryByIdAsync(getDto);

      if (!existingResult.Succeeded || existingResult.Data is null)
        return ReturnBaseHandler.Failed<string>($"Vocabulary entry with id '{updateDto.Id}' not found");

      // Update entity
      var entity = new VocabularyEntry
      {
        Id = updateDto.Id,
        Word = updateDto.Word!.Trim(),
        Definition = updateDto.Definition!.Trim(),
        Example = updateDto.Example!.Trim(),
        CreatedAt = existingResult.Data.CreatedAt,
        UpdatedAt = DateTime.UtcNow,
        IsDeleted = false
      };

      return await _vocabularyEntryRepository.UpdateVocabularyEntryAsync(entity);
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<string>("Failed to update vocabulary entry");
    }
  }

  public async Task<ReturnBase<string>> DeleteVocabularyEntryAsync(DeleteVocabularyEntryDto deleteDto)
  {
    // Validate input
    if (string.IsNullOrWhiteSpace(deleteDto.Id))
      return ReturnBaseHandler.Failed<string>("Vocabulary entry ID cannot be empty");

    try
    {
      // Verify entry exists
      var getDto = new GetVocabularyEntryByIdDto { Id = deleteDto.Id.Trim() };
      var existingResult = await _vocabularyEntryRepository.GetVocabularyEntryByIdAsync(getDto);

      if (!existingResult.Succeeded || existingResult.Data is null)
        return ReturnBaseHandler.Failed<string>($"Vocabulary entry with id '{deleteDto.Id}' not found");

      return await _vocabularyEntryRepository.DeleteVocabularyEntryAsync(deleteDto);
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<string>("Failed to delete vocabulary entry");
    }
  }

  public async Task<ReturnBase<GetVocabularyEntryResponse>> GetVocabularyEntryByIdAsync(GetVocabularyEntryByIdDto getDto)
  {
    // Validate input
    if (string.IsNullOrWhiteSpace(getDto.Id))
      return ReturnBaseHandler.Failed<GetVocabularyEntryResponse>("Vocabulary entry ID cannot be empty");

    try
    {
      var result = await _vocabularyEntryRepository.GetVocabularyEntryByIdAsync(getDto);

      if (!result.Succeeded || result.Data is null)
        return ReturnBaseHandler.Failed<GetVocabularyEntryResponse>($"Vocabulary entry with id '{getDto.Id}' not found");

      var response = MapEntityToResponse(result.Data);
      return ReturnBaseHandler.Success(response, "Vocabulary entry retrieved successfully");
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<GetVocabularyEntryResponse>("Failed to retrieve vocabulary entry");
    }
  }

  public async Task<ReturnBase<GetVocabularyEntriesPaginatedResponse>> GetVocabularyEntriesPaginatedAsync(GetVocabularyEntriesPaginatedDto getDto)
  {
    // Validate pagination parameters
    if (getDto.Page < 1)
      return ReturnBaseHandler.Failed<GetVocabularyEntriesPaginatedResponse>("Page number must be greater than 0");

    if (getDto.PageSize < 1 || getDto.PageSize > 100)
      return ReturnBaseHandler.Failed<GetVocabularyEntriesPaginatedResponse>("Page size must be between 1 and 100");

    try
    {
      var result = await _vocabularyEntryRepository.GetVocabularyEntriesPaginatedAsync(getDto);

      if (!result.Succeeded || result.Data.vocabularies is null)
        return ReturnBaseHandler.Failed<GetVocabularyEntriesPaginatedResponse>("Failed to retrieve vocabulary entries");

      var entries = result.Data.vocabularies.ToList();
      var responses = entries.Select(e => MapEntityToResponse(e)).ToList();
      var response = new GetVocabularyEntriesPaginatedResponse
      {
        VocabularyEntries = responses,
        TotalCount = result.Data.totalCount
      };

      return ReturnBaseHandler.Success(response, "Vocabulary entries retrieved successfully");
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<GetVocabularyEntriesPaginatedResponse>("Failed to retrieve vocabulary entries");
    }
  }

  public async Task<ReturnBase<SearchVocabularyEntryResponse>> SearchVocabularyEntryByWordAsync(SearchVocabularyEntryDtoByWord searchDto)
  {
    // Validate input
    if (string.IsNullOrWhiteSpace(searchDto.SearchQuery))
      return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("Search query cannot be empty");

    try
    {
      var result = await _vocabularyEntryRepository.SearchVocabularyEntryByWordAsync(searchDto);

      if (!result.Succeeded || result.Data is null)
        return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("No vocabulary entries found");

      var entries = result.Data.ToList();
      var responses = entries.Select(e => MapEntityToResponse(e)).ToList();

      var response = new SearchVocabularyEntryResponse
      {
        Results = responses,
        TotalCount = responses.Count
      };

      return ReturnBaseHandler.Success(response, $"Found {responses.Count} vocabulary entry(ies) matching '{searchDto.SearchQuery}'");
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("Failed to search vocabulary entries");
    }
  }

  public async Task<ReturnBase<SearchVocabularyEntryResponse>> SearchVocabularyEntryByDefinitionAsync(SearchVocabularyEntryDtoByDefinition searchDto)
  {
    // Validate input
    if (string.IsNullOrWhiteSpace(searchDto.SearchQuery))
      return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("Search query cannot be empty");

    try
    {
      var result = await _vocabularyEntryRepository.SearchVocabularyEntryByDefinitionAsync(searchDto);

      if (!result.Succeeded || result.Data is null)
        return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("No vocabulary entries found");

      var entries = result.Data.ToList();
      var responses = entries.Select(e => MapEntityToResponse(e)).ToList();

      var response = new SearchVocabularyEntryResponse
      {
        Results = responses,
        TotalCount = responses.Count
      };

      return ReturnBaseHandler.Success(response, $"Found {responses.Count} vocabulary entry(ies) matching '{searchDto.SearchQuery}'");
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("Failed to search vocabulary entries");
    }
  }

  public async Task<ReturnBase<SearchVocabularyEntryResponse>> SearchVocabularyEntryByExampleAsync(string searchQuery)
  {
    // Validate input
    if (string.IsNullOrWhiteSpace(searchQuery))
      return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("Search query cannot be empty");

    try
    {
      var result = await _vocabularyEntryRepository.SearchVocabularyEntryByExampleAsync(searchQuery.Trim());

      if (!result.Succeeded || result.Data is null)
        return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("No vocabulary entries found");

      var entries = result.Data.ToList();
      var responses = entries.Select(e => MapEntityToResponse(e)).ToList();

      var response = new SearchVocabularyEntryResponse
      {
        Results = responses,
        TotalCount = responses.Count
      };

      return ReturnBaseHandler.Success(response, $"Found {responses.Count} vocabulary entry(ies) matching '{searchQuery}'");
    }
    catch (Exception)
    {
      // TODO:: add logging
      return ReturnBaseHandler.Failed<SearchVocabularyEntryResponse>("Failed to search vocabulary entries");
    }
  }

  // ===== PRIVATE HELPER METHODS =====

  private ReturnBase<string> ValidateInsertDto(InsertVocabularyEntryDto dto)
  {
    if (dto is null)
      return ReturnBaseHandler.Failed<string>("Input data cannot be null");

    if (string.IsNullOrWhiteSpace(dto.Word))
      return ReturnBaseHandler.Failed<string>("Word cannot be empty or whitespace");

    if (string.IsNullOrWhiteSpace(dto.Definition))
      return ReturnBaseHandler.Failed<string>("Definition cannot be empty or whitespace");

    if (string.IsNullOrWhiteSpace(dto.Example))
      return ReturnBaseHandler.Failed<string>("Example cannot be empty or whitespace");

    if (dto.Word.Length > 200)
      return ReturnBaseHandler.Failed<string>("Word cannot exceed 200 characters");

    if (dto.Definition.Length > 2000)
      return ReturnBaseHandler.Failed<string>("Definition cannot exceed 2000 characters");

    if (dto.Example.Length > 2000)
      return ReturnBaseHandler.Failed<string>("Example cannot exceed 2000 characters");

    return ReturnBaseHandler.Success("Validation passed");
  }

  private ReturnBase<string> ValidateUpdateDto(UpdateVocabularyEntryDto dto)
  {
    if (dto is null)
      return ReturnBaseHandler.Failed<string>("Input data cannot be null");

    if (string.IsNullOrWhiteSpace(dto.Id))
      return ReturnBaseHandler.Failed<string>("Vocabulary entry ID cannot be empty");

    if (string.IsNullOrWhiteSpace(dto.Word))
      return ReturnBaseHandler.Failed<string>("Word cannot be empty or whitespace");

    if (string.IsNullOrWhiteSpace(dto.Definition))
      return ReturnBaseHandler.Failed<string>("Definition cannot be empty or whitespace");

    if (string.IsNullOrWhiteSpace(dto.Example))
      return ReturnBaseHandler.Failed<string>("Example cannot be empty or whitespace");

    if (dto.Word.Length > 200)
      return ReturnBaseHandler.Failed<string>("Word cannot exceed 200 characters");

    if (dto.Definition.Length > 2000)
      return ReturnBaseHandler.Failed<string>("Definition cannot exceed 2000 characters");

    if (dto.Example.Length > 2000)
      return ReturnBaseHandler.Failed<string>("Example cannot exceed 2000 characters");

    return ReturnBaseHandler.Success("Validation passed");
  }

  private static GetVocabularyEntryResponse MapEntityToResponse(VocabularyEntry entity)
  {
    return new GetVocabularyEntryResponse
    {
      Id = entity.Id,
      Word = entity.Word,
      Definition = entity.Definition,
      Example = entity.Example
    };
  }
}