using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminQuestsController(IQuestService questService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var quests = await questService.GetAsync();
        return (quests.Any()) ? Ok(quests) : Ok("На данный момент квестов нет");
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetAllFiltered([FromBody] FilterQuestDTO filter)
    {
        var quests = await questService.GetFilteredAsync(filter);
        return (quests.Any()) ? Ok(quests) : Ok("Не найдено подходящих квестов");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var quest = await questService.GetByIdAsync(id);
        return quest == null ? NotFound() : Ok(quest);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] QuestUpdateCreateDTO dto)
    {
        var quest = await questService.CreateAsync(dto);
        return Ok(quest);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] QuestUpdateCreateDTO dto)
    {
        var updated = await questService.UpdateAsync(id, dto);
        return (updated is null) ? NotFound("Квест не найден") : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await questService.DeleteAsync(id);
        return deleted ? Ok("Квест был удален") : NotFound("Квест не найден");
    }
}