using WebAppTest.DTO;
using WebAppTest.Entities;

namespace WebAppTest.Services;

public interface IQuestService
{
    Task<List<QuestBaseDTO>> GetAsync();
    Task<List<QuestBaseDTO>> GetFilteredAsync(FilterQuestDTO filter);
    Task<QuestDetailsDTO> GetByIdAsync(Guid id);
    Task<QuestDetailsDTO> CreateAsync(QuestUpdateCreateDTO dto);
    Task<QuestDetailsDTO> UpdateAsync(Guid id, QuestUpdateCreateDTO dto);
    Task DeleteAsync(Guid id);
}