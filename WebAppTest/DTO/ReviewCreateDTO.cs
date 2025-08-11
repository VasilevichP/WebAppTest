using System.ComponentModel.DataAnnotations;
using WebAppTest.Entities;

namespace WebAppTest.DTO;
public class ReviewCreateDTO
{
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = "Требуется id квеста")]
    public Guid QuestId { get; set; }

    [Range(1, 5, ErrorMessage = "Рейтинг должен быть от 1 до 5")]
    public int Rating { get; set; }

    public string? Text { get; set; }
}