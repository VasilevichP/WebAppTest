using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class ReviewDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public int Rating { get; set; }
    public string Text { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    
    public static ReviewDTO ToDTO(Review review)
    {
        return new ReviewDTO()
        {
            Id = review.Id,
            Text = review.Text,
            Date = review.Date,
            UserName = review.User.Username ?? review.User.Email,
            Rating = review.Rating
        };
    }
}