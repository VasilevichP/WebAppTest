using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableCors("CORSSpecifications")]
public class GeneralQuestsController (IQuestService questService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllQuests()
    {
        var quests = await questService.GetAsync();
        return Ok(quests);
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetAllFilteredQuests([FromBody] FilterQuestDTO filter)
    {
        var quests = await questService.GetFilteredAsync(filter);
        return Ok(quests);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuest(Guid id)
    {
        var quest = await questService.GetByIdAsync(id);
        return Ok(quest);
    }
}