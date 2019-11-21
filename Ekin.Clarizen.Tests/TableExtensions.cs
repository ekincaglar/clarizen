using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Clarizen.Tests
{
    public static class TableExtensions
    {
        public static string GetFirstCell(this Table table, string colName)
        {
            return table.Rows[0].GetString(colName);
        }
    }
}