using CsvHelper;
using CsvHelper.Configuration;
using family_sorter.Models;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace family_sorter;

public static class ExcelStore
{
    private const string PathCsvOutput = @"C:\Dev\Lab\Dot-net\output\";
    private const string ExtensionCsv = ".csv";

    public static List<Item> GetContent(string pathCsv)
    {
        var items = new List<Item>();

        using var reader = new StreamReader(pathCsv);
        using var csv = new CsvReader(reader, GetConfigInputFile());

        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            var item = new Item
            {
                Row = csv.Parser.RawRow,
                Id = csv.Parser.Record[0],
                FamilyName = csv.Parser.Record[1],
                Attributes = csv.Parser.Record.Skip(2)
                    .Select((value, index) => new { value, index })
                    .Where(x => !string.IsNullOrEmpty(x.value))
                    .Select(x => new Attribut { Name = csv.HeaderRecord[x.index + 2], Value = x.value})
                    .ToList()
            };

            items.Add(item);
        }

        return items;
    }

    public static void WriteCsvFile(string nameCsvFile, string familyName, List<Item> items, List<string> attributesCurrentFamily)
    {
        var stringPath = PathCsvOutput + nameCsvFile + ExtensionCsv;
        using (StreamWriter file = new StreamWriter(stringPath, false))
        {
            file.WriteLine("sku;family;"+ String.Join(';', attributesCurrentFamily));
            foreach (var item in items)
            {
                var stringAttributes = $"{item.Id};{item.FamilyName};";
                foreach (var attribute in attributesCurrentFamily)
                {
                    if (item.Attributes.Select(x => x.Name).Contains(attribute))
                    {
                        stringAttributes = stringAttributes + item.Attributes.Single(x => x.Name == attribute).Value;
                    }
                    else
                    {
                        stringAttributes += ';';
                    }
                }
                file.WriteLine(stringAttributes);
            }
        }
    }

    public static void WriteCsvFileOld(string nameCsvFile, string familyName, List<Item> items, List<string> families)
    {
        var records = new List<object>
        {
            new { Name = "NOM", Family = "FAMILLE", Attributs = String.Join(';', families)},
        };
        using var writer = new StreamWriter(PathCsvOutput + nameCsvFile);
        using var csv = new CsvWriter(writer, GetConfigOutputFile());
        {
            csv.WriteRecords(records);
        }
    }

    private static CsvConfiguration GetConfigInputFile()
        => new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        };

    private static CsvConfiguration GetConfigOutputFile()
        => new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false,
        };
}
