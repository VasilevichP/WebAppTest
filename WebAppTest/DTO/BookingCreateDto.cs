using System.ComponentModel.DataAnnotations;

namespace WebAppTest.DTO;

public class BookingCreateDto
{
    [Required(ErrorMessage = "Требуется id квеста")]
    public Guid QuestId { get; set; }
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = "Выберите дату")]
    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "Выберите время")]
    public TimeOnly Time { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Минимум 1 участник")]
    public int Participants { get; set; }
}