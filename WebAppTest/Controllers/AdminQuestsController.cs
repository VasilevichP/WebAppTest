using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[EnableCors("CORSSpecifications")]
public class AdminQuestsController(IQuestService questService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateQuest([FromBody] QuestUpdateCreateDTO dto)
    {
        var quest = await questService.CreateAsync(dto);
        return Ok(quest);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuest(Guid id, [FromBody] QuestUpdateCreateDTO dto)
    {
        var updated = await questService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuest(Guid id)
    {
        await questService.DeleteAsync(id);
        return Ok();
    }
}