using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class QuestDetailsDTO : QuestBaseDTO
{
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<string> PhotoUrls { get; set; } = new();
    public List<ReviewDTO> Reviews { get; set; } = new();
    public double Rating { get; set; }
}