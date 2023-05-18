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
        }
        else
        {
            Console.WriteLine("Ce fichier n'est pas un Excel de type csv");
        }

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
