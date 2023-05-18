using CsvHelper;
using CsvHelper.Configuration;
using family_sorter.Models;
using System.Globalization;

namespace family_sorter;

public static class ExcelStore
{
    public static List<Item> GetContent(string pathCsv)
    {
        var items = new List<Item>();

        using var reader = new StreamReader(pathCsv);
        using var csv = new CsvReader(reader, GetConfig());

        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            var item = new Item
            {
                Row = csv.Parser.RawRow,
                Name = csv.Parser.Record[0],
                FamilyName = csv.Parser.Record[1],
                Attributes = csv.Parser.Record.Skip(2)
                    .Select((value, index) => new { value, index })
                    .Where(x => !string.IsNullOrEmpty(x.value))
                    .Select(x => csv.HeaderRecord[x.index + 2])
                    .ToList()
            };

            items.Add(item);
        }

        return items;
    }

    private static CsvConfiguration GetConfig()
        => new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        };
}
