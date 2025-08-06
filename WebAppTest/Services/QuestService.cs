using System.Net;
using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Enums;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class QuestService(AppDbContext context) : IQuestService
{
    public async Task<List<QuestBriefDTO>> GetAsync()
    {
        var quests = await context.Quests.ToListAsync();
        return quests.Select(QuestBriefDTO.ToDTO).ToList();
    }

    public async Task<List<QuestBriefDTO>> GetFilteredAsync(FilterQuestDTO filter)
    {
        var query = context.Quests.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(q => q.Title.ToLower().Contains(filter.Title.ToLower()));

        if (filter.MinPrice.HasValue)
            query = query.Where(q => q.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(q => q.Price <= filter.MaxPrice.Value);

        if (filter.MinDuration.HasValue)
            query = query.Where(q => q.DurationMinutes >= filter.MinDuration.Value);

        if (filter.MaxDuration.HasValue)
            query = query.Where(q => q.DurationMinutes <= filter.MaxDuration.Value);

        if (filter.FearLevel.HasValue)
            query = query.Where(q => q.FearLevel == filter.FearLevel.Value);

        if (filter.HasActors.HasValue)
            query = query.Where(q => q.HasActors == filter.HasActors.Value);

        if (filter.DifficultyLevel.HasValue)
            query = query.Where(q => q.DifficultyLevel == filter.DifficultyLevel.Value);
        
        query = filter.SortOptions switch
        {
            QuestSortOptions.TITLE_ASC => query.OrderBy(q => q.Title),
            QuestSortOptions.TITLE_DESC => query.OrderByDescending(q => q.Title),
            QuestSortOptions.DURATION_ASC => query.OrderBy(q => q.DurationMinutes),
            QuestSortOptions.DURATION_DESC => query.OrderByDescending(q => q.DurationMinutes),
            QuestSortOptions.PRICE_ASC => query.OrderBy(q => q.Price),
            QuestSortOptions.PRICE_DESC => query.OrderByDescending(q => q.Price),
            QuestSortOptions.RATING_ASC => query.OrderBy(q => q.Rating),
            QuestSortOptions.RATING_DESC => query.OrderByDescending(q => q.Rating),
            _ => query
        };
        
        var quests = await query.ToListAsync();
        return quests.Select(QuestBriefDTO.ToDTO).ToList();
    }

    public async Task<QuestDetailsDTO> GetByIdAsync(Guid id)
    {
        var quest = await context.Quests
            .Include(q => q.Photos)
            .Include(q => q.Reviews).ThenInclude(r =>r.User)
            .FirstOrDefaultAsync(q => q.Id == id);
        if (quest == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");
        return QuestDetailsDTO.ToDTO(quest);
    }

    public async Task<QuestDetailsDTO> CreateAsync(QuestUpdateCreateDTO dto)
    {
        if (dto.PhotoUrls.Count > 6)
            throw new HttpException(HttpStatusCode.BadRequest, "Можно загрузить максимум 6 фото");
        var quest = QuestUpdateCreateDTO.FromCreateDTO(dto);

        context.Quests.Add(quest);
        await context.SaveChangesAsync();
        var questDto = QuestDetailsDTO.ToDTO(quest);

        return questDto;
    }

    public async Task<QuestDetailsDTO> UpdateAsync(Guid id, QuestUpdateCreateDTO dto)
    {
        if (dto.PhotoUrls.Count > 6)
            throw new HttpException(HttpStatusCode.BadRequest, "Можно загрузить максимум 6 фото");
        var quest = await context.Quests
            .Include(q => q.Photos)
            .Include(q => q.Reviews).ThenInclude(r => r.User)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quest == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");
        QuestUpdateCreateDTO.FromUpdateDTO(quest, dto);
        await context.SaveChangesAsync();
        
        return QuestDetailsDTO.ToDTO(quest);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var quest = await context.Quests.FindAsync(id);
        if (quest == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");

        context.Quests.Remove(quest);
        await context.SaveChangesAsync();
        return true;
    }
}