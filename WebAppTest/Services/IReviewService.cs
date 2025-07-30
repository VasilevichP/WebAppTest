using WebAppTest.DTO;

namespace WebAppTest.Services;

public interface IReviewService
{
    public Task<ReviewDTO> CreateReviewAsync(ReviewCreateDTO dto);
    public Task<bool> DeleteReviewAsync(Guid id);
}