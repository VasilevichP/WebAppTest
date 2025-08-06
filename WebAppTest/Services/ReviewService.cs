using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;

namespace WebAppTest.Services;

public class ReviewService(AppDbContext context) : IReviewService
{
    public async Task<ReviewDTO> CreateReviewAsync(ReviewCreateDTO dto)
    {
        var review = ReviewCreateDTO.FromCreateDTO(dto);
        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();

        var quest = (await context.Quests.FindAsync(review.QuestId))!;
        await CountRating(quest);

        review.Quest = quest;
        review.User = (await context.Users.FindAsync(review.UserId))!;


        return ReviewDTO.ToDTO(review);
    }

    public async Task<bool> DeleteReviewAsync(Guid id)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review == null) return false;
        context.Reviews.Remove(review);
        var quest = (await context.Quests.FindAsync(review.QuestId))!;
        await CountRating(quest);
        await context.SaveChangesAsync();
        return true;
    }

    private async Task CountRating(Quest quest)
    {
        var sum = await context.Reviews.Where(r => r.Quest == quest).SumAsync(r => r.Rating);
        var num = await context.Reviews.Where(r => r.Quest == quest).CountAsync();
        if (sum == 0 || num == 0) quest.Rating = (double)sum / num;
    }
}