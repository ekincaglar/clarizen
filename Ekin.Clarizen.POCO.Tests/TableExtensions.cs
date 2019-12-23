using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Ekin.Clarizen.POCO.Tests
{
    public static class TableExtensions
    {
        public static string GetFirstCell(this Table table, string colName)
        {
            return table.Rows[0].GetString(colName);
        }

        public static bool ConvertToBool(this TableRow row,string fieldName)
        {
            string value = null;
            try
            {
                value = row[fieldName];
            }
            catch (Exception)
            {
                // ignored
            }

            return !string.IsNullOrEmpty(value) && Convert.ToBoolean(value);
        }
    }
}