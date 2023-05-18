namespace family_sorter.Models;

public record Item
{
    public int Row { get; init; }
    public string Name { get; init; }
    public string FamilyName { get; init; }
    public List<string> Attributes { get; init; }
}