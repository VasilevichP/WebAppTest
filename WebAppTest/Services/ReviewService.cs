using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;

namespace WebAppTest.Services;

public class ReviewService(AppDbContext context) : IReviewService
{
    public async Task<ReviewDTO> CreateReviewAsync(ReviewCreateDTO dto)
    {
        var review = FromCreateDTO(dto);
        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();
        
        review.User = (await context.Users.FindAsync(review.UserId))!;
        review.Quest = (await context.Quests.FindAsync(review.QuestId))!;
        
        return ReviewDTO.ToDTO(review);
    }

    public async Task<bool> DeleteReviewAsync(Guid id)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review == null) return false;
        context.Reviews.Remove(review);
        await context.SaveChangesAsync();
        return true;
    }

    // private ReviewDTO ToDTO(Review review)
    // {
    //     return new ReviewDTO()
    //     {
    //         Id = review.Id,
    //         Text = review.Text,
    //         Date = review.Date,
    //         UserName = review.User.Username ?? review.User.Email,
    //         Rating = review.Rating
    //     };
    // }
    //
    private Review FromCreateDTO(ReviewCreateDTO dto)
    {
        return new Review()
        {
            UserId = dto.UserId.Value,
            QuestId = dto.QuestId,
            Text = dto.Text,
            Date = DateTime.Now,
            Rating = dto.Rating
        };
    }
}