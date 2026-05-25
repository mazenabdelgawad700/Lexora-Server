using Lexora.DTOs;
using Lexora.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lexora.Controllers;

[ApiController]
[Route("api/v1/vocabulary")]
public class VocabularyEntryController : ControllerBase
{
  private readonly IVocabularyEntryService _vocabularyEntryService;

  public VocabularyEntryController(IVocabularyEntryService vocabularyEntryService)
  {
    _vocabularyEntryService = vocabularyEntryService;
  }

  /// <summary>
  /// Create a new vocabulary entry
  /// </summary>
  /// <param name="insertDto">Vocabulary entry data</param>
  /// <returns>Success message or error</returns>
  [HttpPost("create")]
  public async Task<IActionResult> CreateVocabularyEntry([FromBody] InsertVocabularyEntryDto insertDto)
  {
    var result = await _vocabularyEntryService.InsertVocabularyEntryAsync(insertDto);
    if (!result.Succeeded)
      return BadRequest(result);

    return Ok(result);
  }

  /// <summary>
  /// Update an existing vocabulary entry
  /// </summary>
  /// <param name="updateDto">Vocabulary entry data with ID</param>
  /// <returns>Success message or error</returns>
  [HttpPut("update")]
  public async Task<IActionResult> UpdateVocabularyEntry([FromBody] UpdateVocabularyEntryDto updateDto)
  {
    var result = await _vocabularyEntryService.UpdateVocabularyEntryAsync(updateDto);
    if (!result.Succeeded)
      return BadRequest(result);

    return Ok(result);
  }

  /// <summary>
  /// Delete a vocabulary entry (soft delete)
  /// </summary>
  /// <param name="id">Vocabulary entry ID</param>
  /// <returns>Success message or error</returns>
  [HttpDelete("delete/{id}")]
  public async Task<IActionResult> DeleteVocabularyEntry([FromRoute] string id)
  {
    var deleteDto = new DeleteVocabularyEntryDto { Id = id };
    var result = await _vocabularyEntryService.DeleteVocabularyEntryAsync(deleteDto);
    if (!result.Succeeded)
      return BadRequest(result);

    return Ok(result);
  }

  /// <summary>
  /// Get a vocabulary entry by ID
  /// </summary>
  /// <param name="id">Vocabulary entry ID</param>
  /// <returns>Vocabulary entry or error</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetVocabularyEntryById([FromRoute] string id)
  {
    var getDto = new GetVocabularyEntryByIdDto { Id = id };
    var result = await _vocabularyEntryService.GetVocabularyEntryByIdAsync(getDto);
    if (!result.Succeeded)
      return NotFound(result);

    return Ok(result);
  }

  /// <summary>
  /// Get vocabulary entries with pagination
  /// </summary>
  /// <param name="page">Page number (default: 1)</param>
  /// <param name="pageSize">Number of entries per page (default: 10, max: 100)</param>
  /// <returns>Paginated vocabulary entries</returns>
  [HttpGet("list")]
  public async Task<IActionResult> GetVocabularyEntriesPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    var getDto = new GetVocabularyEntriesPaginatedDto { Page = page, PageSize = pageSize };
    var result = await _vocabularyEntryService.GetVocabularyEntriesPaginatedAsync(getDto);
    if (!result.Succeeded)
      return BadRequest(result);

    return Ok(result);
  }

  /// <summary>
  /// Search vocabulary entries by word
  /// </summary>
  /// <param name="searchQuery">Search term</param>
  /// <returns>Matching vocabulary entries</returns>
  [HttpGet("search-by-word")]
  public async Task<IActionResult> SearchByWord([FromQuery] string searchQuery)
  {
    var searchDto = new SearchVocabularyEntryDtoByWord { SearchQuery = searchQuery };
    var result = await _vocabularyEntryService.SearchVocabularyEntryByWordAsync(searchDto);
    if (!result.Succeeded)
      return BadRequest(result);

    return Ok(result);
  }

  /// <summary>
  /// Search vocabulary entries by definition
  /// </summary>
  /// <param name="searchQuery">Search term</param>
  /// <returns>Matching vocabulary entries</returns>
  [HttpGet("search-by-definition")]
  public async Task<IActionResult> SearchByDefinition([FromQuery] string searchQuery)
  {
    var searchDto = new SearchVocabularyEntryDtoByDefinition { SearchQuery = searchQuery };
    var result = await _vocabularyEntryService.SearchVocabularyEntryByDefinitionAsync(searchDto);
    if (!result.Succeeded)
      return BadRequest(result);

    return Ok(result);
  }

  /// <summary>
  /// Search vocabulary entries by example
  /// </summary>
  /// <param name="searchQuery">Search term</param>
  /// <returns>Matching vocabulary entries</returns>
  [HttpGet("search-by-example")]
  public async Task<IActionResult> SearchByExample([FromQuery] string searchQuery)
  {
    var result = await _vocabularyEntryService.SearchVocabularyEntryByExampleAsync(searchQuery);
    if (!result.Succeeded)
      return BadRequest(result);

    return Ok(result);
  }
}
