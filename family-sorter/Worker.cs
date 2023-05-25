using family_sorter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_sorter;

public class Worker
{
    private readonly string _pathCsvFile;
    

    public Worker(string pathCsvFile)
    {
        _pathCsvFile = pathCsvFile;
    }

    public void Execute()
    {
        if (IsValideCsv())
        {
            var content = ExcelStore.GetContent(_pathCsvFile);
            var families = content.Select(x => x.FamilyName).Distinct();

            var dateNow = DateTime.Now;

            foreach (var family in families)
            {
                var nameCsvFamily = $"export_{dateNow.Year}-{dateNow.Month}-{dateNow.Day}_{family.Replace(' ', '_')}";
                var itemsCurrentFamily = content.Select(x => x)
                    .Where(x => x.FamilyName == family)
                    .ToList();

                var attributesCurrentFamily = GetAttributesCurrentFamily(itemsCurrentFamily);

                ExcelStore.WriteCsvFile(nameCsvFamily, family, itemsCurrentFamily, attributesCurrentFamily);
            }
        }
        else
        {
            Console.WriteLine("Ce fichier n'est pas un Excel de type csv");
        }
    }

    private List<string> GetAttributesCurrentFamily(List<Item> itemsCurrentFamily)
    {
        var attributes = new List<string>();

        foreach (var item in itemsCurrentFamily)
        {
            var temp = item.Attributes.Select(x => x.Name);
            attributes.AddRange(temp);
        }

        return attributes.Distinct().ToList();
    }

    private bool IsValideCsv()
    {
        var extension = _pathCsvFile.Substring(_pathCsvFile.Length - 4);
        if (extension is ".csv")
        {
            return true;
        }

        return false;
    }
}
