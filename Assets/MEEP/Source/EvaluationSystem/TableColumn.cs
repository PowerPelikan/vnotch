namespace MEEP.EvaluationSystem
{

    [System.Serializable]
    public class TableColumn
    {
        public TableCell[] cells;

        public TableColumn(params TableColumnDefinition[] columns)
        {
            cells = new TableCell[columns.Length];
        }

        public void FillReferences(params TableCell[] cells)
        {
            this.cells = cells;
        }

    }

}
