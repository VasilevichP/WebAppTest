using System.Net;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Enums;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class QuestService(AppDbContext context) : IQuestService
{
    public async Task<List<QuestBaseDTO>> GetAsync()
    {
        var quests = await context.Quests.ToListAsync();
        return quests.Adapt<List<QuestBaseDTO>>();
    }

    public async Task<List<QuestBaseDTO>> GetFilteredAsync(FilterQuestDTO filter)
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
        return quests.Adapt<List<QuestBaseDTO>>();
    }

    public async Task<QuestDetailsDTO> GetByIdAsync(Guid id)
    {
        var quest = await FindQuestAsync(id) ?? 
            throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");
        return quest.Adapt<QuestDetailsDTO>();
    }

    public async Task<QuestDetailsDTO> CreateAsync(QuestUpdateCreateDTO dto)
    {
        if (dto.PhotoUrls.Count > 6)
            throw new HttpException(HttpStatusCode.BadRequest, "Можно загрузить максимум 6 фото");
        var quest = dto.Adapt<Quest>();

        context.Quests.Add(quest);
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при создании квеста");
        
        return quest.Adapt<QuestDetailsDTO>();
    }

    public async Task<QuestDetailsDTO> UpdateAsync(Guid id, QuestUpdateCreateDTO dto)
    {
        if (dto.PhotoUrls.Count > 6)
            throw new HttpException(HttpStatusCode.BadRequest, "Можно загрузить максимум 6 фото");
        var quest = await FindQuestAsync(id) ??
            throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");
        
        var existingUrls = quest.Photos.Select(p => p.Url).ToHashSet();
        var newPhotos = dto.PhotoUrls
            .Where(url => !existingUrls.Contains(url))
            .Select(url => new QuestPhoto { Url = url, QuestId = id })
            .ToList();
        quest.Photos.Clear();
        foreach (var photo in newPhotos)
        {
            quest.Photos.Add(photo);
        }
        
        dto.Adapt(quest);
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка обновлении информации о квесте");
        return quest.Adapt<QuestDetailsDTO>();
    }

    public async Task DeleteAsync(Guid id)
    {
        var quest = await context.Quests.FindAsync(id)??
            throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");

        context.Quests.Remove(quest);
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при удалении квеста");
    }

    private async Task<Quest?> FindQuestAsync(Guid id)
    {
        var quest = await context.Quests
            .Include(q => q.Photos)
            .Include(q => q.Reviews).ThenInclude(r => r.User)
            .FirstOrDefaultAsync(q => q.Id == id);
        return quest;
    }
}