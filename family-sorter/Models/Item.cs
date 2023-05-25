using System.Data;

namespace family_sorter.Models;

public record Item
{
    public int Row { get; init; }
    public string Id { get; init; }
    public string FamilyName { get; init; }
    public List<Attribut> Attributes { get;init; }
}