using System.Collections.Generic;
using UnityEngine;

namespace MEEP.EvaluationSystem
{
    public class WorksheetData : ScriptableObject
    {
        public TableColumnDefinition[] columnDefs;

        public List<string[]> cellValues;

        public void SetUp(TableColumnDefinition[] columns, int numberOfRows)
        {
            columnDefs = columns;
            cellValues = new();

            for (int i = 0; i < numberOfRows; i++)
            {
                cellValues.Add(new string[columnDefs.Length]);
            }
        }

        public void SetRowData(int rowIndex, string[] data)
        {
            cellValues[rowIndex] = data;
        }


        public string GetCell(int rowIndex, int columnIndex)
        {
            return cellValues[rowIndex][columnIndex];
        }

        public string[] GetRow(int rowIndex)
        {
            return cellValues[rowIndex];
        }

    }

}