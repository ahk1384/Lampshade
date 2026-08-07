using System.Text.RegularExpressions;
using MiniExcelLibs;

namespace _0_Framework.Application;

public class ExportExcel
{
    public byte[] ExportExcelResult(List<ExcelTable> datas)
    {
        var sheets = new Dictionary<string, object>();

        var usedSheetNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var data in datas)
        {
            string rawName = string.IsNullOrWhiteSpace(data.name) ? "Sheet" : data.name;
            string safeName = Regex.Replace(rawName, @"[\:\?\\\/\*\[\]]", "_");
            if (safeName.Length > 31) safeName = safeName.Substring(0, 31);

            string finalSheetName = safeName;
            int counter = 1;
            while (usedSheetNames.Contains(finalSheetName))
            {
                string suffix = $"_{counter++}";
                int maxLen = 31 - suffix.Length;
                finalSheetName = (safeName.Length > maxLen ? safeName.Substring(0, maxLen) : safeName) + suffix;
            }

            usedSheetNames.Add(finalSheetName);

            var tableRows = new List<Dictionary<string, object>>();

            foreach (var rowModel in data.Rows)
            {
                var rowDict = new Dictionary<string, object>();

                for (int j = 0; j < data.Columns.Count; j++)
                {
                    string colHeader = data.Columns[j];
                    string cellValue = j < rowModel.row.Count ? rowModel.row[j] : string.Empty;

                    if (data.DateFormatColumn - 1 == j && DateTime.TryParse(cellValue, out var parsedDate))
                    {
                        cellValue = parsedDate.ToString("yyyy-MM-dd");
                    }

                    rowDict[colHeader] = cellValue;
                }

                tableRows.Add(rowDict);
            }

            sheets.Add(finalSheetName, tableRows);
        }

        using var stream = new MemoryStream();
        stream.SaveAs(sheets);

        return stream.ToArray();
    }
}

public class ExcelTable
{
    public ExcelTable()
    {
    }

    public ExcelTable(string name, List<Row> rows, List<string> columns, int dateFormatColumn)
    {
        this.name = name;
        Rows = rows;
        Columns = columns;
        DateFormatColumn = dateFormatColumn;
    }

    public string name { get; set; }
    public List<Row> Rows { get; set; } = new();
    public List<string> Columns { get; set; } = new();
    public int DateFormatColumn { get; set; }

    public class Row
    {
        public List<string> row { get; set; } = new();
    }
}