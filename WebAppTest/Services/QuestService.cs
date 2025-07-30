using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Enums;

namespace WebAppTest.Services;

public class QuestService(AppDbContext context) : IQuestService
{
    public async Task<List<QuestBriefDTO>> GetAsync()
    {
        var quests = await context.Quests.ToListAsync();

        return quests.Select(ToBriefDTO).ToList();
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

        if (filter.MinFearLevel.HasValue)
            query = query.Where(q => q.FearLevel >= filter.MinFearLevel.Value);

        if (filter.MaxFearLevel.HasValue)
            query = query.Where(q => q.FearLevel <= filter.MaxFearLevel.Value);

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

        return quests.Select(ToBriefDTO).ToList();
    }

    public async Task<QuestDetailsDTO?> GetByIdAsync(Guid id)
    {
        var quest = await context.Quests
            .Include(q => q.Photos)
            .Include(q => q.Reviews)
            .FirstOrDefaultAsync(q => q.Id == id);

        return quest == null ? null : ToDetailsDTO(quest);
    }

    public async Task<QuestDetailsDTO> CreateAsync(QuestUpdateCreateDTO dto)
    {
        var quest = FromCreateDTO(dto);

        context.Quests.Add(quest);
        await context.SaveChangesAsync();
        var questDto = ToDetailsDTO(quest);

        return questDto;
    }

    public async Task<QuestDetailsDTO?> UpdateAsync(Guid id, QuestUpdateCreateDTO dto)
    {
        var quest = await context.Quests
            .Include(q => q.Photos)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quest == null) return null;
        FromUpdateDTO(quest, dto);
        return ToDetailsDTO(quest);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var quest = await context.Quests.FindAsync(id);
        if (quest == null) return false;

        context.Quests.Remove(quest);
        await context.SaveChangesAsync();
        return true;
    }

    private QuestDetailsDTO ToDetailsDTO(Quest quest)
    {
        return new QuestDetailsDTO
        {
            Id = quest.Id,
            Title = quest.Title,
            Description = quest.Description,
            Address = quest.Address,
            DurationMinutes = quest.DurationMinutes,
            MaxParticipants = quest.MaxParticipants,
            Price = quest.Price,
            DifficultyLevel = quest.DifficultyLevel,
            HasActors = quest.HasActors,
            FearLevel = quest.FearLevel,
            PhotoUrls = quest.Photos.Select(p => p.Url).ToList(),
            Rating = quest.Rating
        };
    }

    private QuestBriefDTO ToBriefDTO(Quest quest)
    {
        return new QuestBriefDTO
        {
            Id = quest.Id,
            Title = quest.Title,
            DurationMinutes = quest.DurationMinutes,
            MaxParticipants = quest.MaxParticipants,
            Price = quest.Price,
            DifficultyLevel = quest.DifficultyLevel,
            HasActors = quest.HasActors,
            FearLevel = quest.FearLevel
        };
    }

    private Quest FromCreateDTO(QuestUpdateCreateDTO dto)
    {
        return new Quest
        {
            Title = dto.Title,
            Description = dto.Description,
            Address = dto.Address,
            DurationMinutes = dto.DurationMinutes,
            MaxParticipants = dto.MaxParticipants,
            Price = dto.Price,
            DifficultyLevel = dto.DifficultyLevel,
            HasActors = dto.HasActors,
            FearLevel = dto.FearLevel,
            Photos = dto.PhotoUrls.Select(url => new QuestPhoto { Url = url }).ToList()
        };
    }

    private void FromUpdateDTO(Quest quest, QuestUpdateCreateDTO dto)
    {
        quest.Title = dto.Title;
        quest.Description = dto.Description;
        quest.Address = dto.Address;
        quest.DurationMinutes = dto.DurationMinutes;
        quest.MaxParticipants = dto.MaxParticipants;
        quest.Price = dto.Price;
        quest.DifficultyLevel = dto.DifficultyLevel;
        quest.HasActors = dto.HasActors;
        quest.FearLevel = dto.FearLevel;

        quest.Photos = dto.PhotoUrls
            .Select(url => new QuestPhoto
            {
                Url = url,
                Quest = quest
            })
            .ToList();
    }
}