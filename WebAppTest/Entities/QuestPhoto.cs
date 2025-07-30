namespace WebAppTest.Entities;

public class QuestPhoto
{
    public Guid Id { get; set; }
    public string Url { get; set; }= String.Empty;
    public Guid QuestId { get; set; }
    public Quest Quest { get; set; }
}