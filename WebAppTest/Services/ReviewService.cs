using System.Net;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class ReviewService(AppDbContext context) : IReviewService
{
    public async Task<ReviewDTO> CreateReviewAsync(ReviewCreateDTO dto)
    {
        var review = dto.Adapt<Review>();

        var quest = await context.Quests.FindAsync(review.QuestId) ??
                    throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");
        var user = await context.Users.FindAsync(review.UserId) ?? await context.Users.FindAsync(review.UserId) ??
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");

        await context.Reviews.AddAsync(review);
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при написании отзыва");

        await CountRating(quest);

        review.Quest = quest;
        review.User = user;

        return review.Adapt<ReviewDTO>();
    }

    public async Task DeleteReviewAsync(Guid id)
    {
        var review = await context.Reviews.FindAsync(id) ??
                     throw new HttpException(HttpStatusCode.NotFound, "Отзыв не найден");
        context.Reviews.Remove(review);
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при удалении отзыва");
        
        var quest = await context.Quests.FindAsync(review.QuestId) ??
                    throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");
        await CountRating(quest);
    }

    private async Task CountRating(Quest quest)
    {
        var sum = await context.Reviews.Where(r => r.Quest == quest).SumAsync(r => r.Rating);
        var num = await context.Reviews.Where(r => r.Quest == quest).CountAsync();
        quest.Rating = num == 0 ? 0 : Math.Round((double)sum / num, 2);
        await context.SaveChangesAsync();
    }
}