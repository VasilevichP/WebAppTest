using WebAppTest.Enums;

namespace WebAppTest.DTO;

public class FilterQuestDTO
{
    public string? Title { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinDuration { get; set; }
    public int? MaxDuration { get; set; }
    public int? FearLevel { get; set; }
    public bool? HasActors { get; set; }
    public int? DifficultyLevel { get; set; }

    public QuestSortOptions? SortOptions { get; set; }
}