using WebAppTest.DTO;
using WebAppTest.Entities;

namespace WebAppTest.Services;

public interface IQuestService
{
    Task<List<QuestBriefDTO>> GetAsync();
    Task<List<QuestBriefDTO>> GetFilteredAsync(FilterQuestDTO filter);
    Task<QuestDetailsDTO?> GetByIdAsync(Guid id);
    Task<QuestDetailsDTO> CreateAsync(QuestUpdateCreateDTO dto);
    Task<QuestDetailsDTO?> UpdateAsync(Guid id, QuestUpdateCreateDTO dto);
    Task<bool> DeleteAsync(Guid id);
}