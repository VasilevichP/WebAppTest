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
    public async Task<IActionResult> Create([FromBody] QuestUpdateCreateDTO dto)
    {
        var quest = await questService.CreateAsync(dto);
        return Ok(quest);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] QuestUpdateCreateDTO dto)
    {
        var updated = await questService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await questService.DeleteAsync(id);
        return Ok("Квест был удален");
    }
}