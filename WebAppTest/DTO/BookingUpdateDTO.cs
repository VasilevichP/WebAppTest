using System.ComponentModel.DataAnnotations;
using WebAppTest.Entities;
using WebAppTest.Enums;

namespace WebAppTest.DTO;

public class BookingUpdateDTO
{
    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Минимум 1 участник")]
    public int? Participants { get; set; }

    public BookingStatus? Status { get; set; }
}