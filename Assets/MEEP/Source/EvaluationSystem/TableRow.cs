using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MEEP.EvaluationSystem
{
    [System.Serializable]
    public class TableRow : MonoBehaviour
    {
        /// <summary>
        /// The worksheet to which this table row belongs
        /// </summary>
        private Worksheet worksheet;

        private List<TMP_InputField> cells;

        private void Start()
        {
            worksheet = GetComponentInParent<Worksheet>();

            cells = new();

            for (int i = 0; i < transform.childCount; i++)
            {
                var inputField = transform.GetChild(i).GetComponentInChildren<TMP_InputField>();

                if (inputField)
                {
                    cells.Add(inputField);
                    inputField.onEndEdit.AddListener(UpdateTable);
                }

            }
        }

        /// <summary>
        /// Notify the worksheet that this row has changed
        /// </summary>
        public void UpdateTable(string newValue)
        {
            worksheet.UpdateRowData(this);
        }


        /// <summary>
        /// Return the current values of this row as strings
        /// </summary>
        public string[] RetrieveValues()
        {
            string[] values = new string[cells.Count];

            for (int i = 0; i < cells.Count; i++)
            {
                values[i] = cells[i].text;
            }

            return values;
        }
    }

}