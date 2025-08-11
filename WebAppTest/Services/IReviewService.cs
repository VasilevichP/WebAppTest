using WebAppTest.DTO;

namespace WebAppTest.Services;

public interface IReviewService
{
    public Task<ReviewDTO> CreateReviewAsync(ReviewCreateDTO dto);
    public Task DeleteReviewAsync(Guid id);
}