using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MEEP.EvaluationSystem
{
    /// <summary>
    /// Writes UI input into worksheet data.
    /// </summary>
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class Worksheet : MonoBehaviour
    {
        public List<TableColumnDefinition> columnDefinitions;

        public int numberOfRows = 6;

        /// <summary>
        /// The data object to write
        /// </summary>
        private WorksheetData dataTarget;

        /// <summary>
        /// The parent object of the worksheet columns
        /// </summary>
        [SerializeField]
        private GameObject uiRoot;

        [SerializeField]
        private GameplayGlobal exitPoint;


        private void Start()
        {
            CreateDataInstance();

            if (exitPoint.GetTarget() != null)
                AddDataToPayload();
            else
                exitPoint.OnGlobalRegistered.AddListener(AddDataToPayload);
        }

        private void AddDataToPayload()
        {
            var payloadComp = exitPoint.GetTarget().AddComponent<SceneLoaderPayload>();

            payloadComp.AddToPayload(dataTarget);

            exitPoint.OnGlobalRegistered.RemoveListener(AddDataToPayload);
        }

        private void CreateDataInstance()
        {
            dataTarget = ScriptableObject.CreateInstance<WorksheetData>();
            dataTarget.SetUp(columnDefinitions.ToArray(), numberOfRows);
        }


        /// <summary>
        /// Update worksheet data to reflect new row state
        /// </summary>
        public void UpdateRowData(TableRow row)
        {
            var values = row.RetrieveValues();
            var index = GetIndexOfRow(row);

            // put values into data object
            dataTarget.SetRowData(index, values);
        }

        private int GetIndexOfRow(TableRow row)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<TableRow>() != null)
                    return i;
            }

            return -1;
        }

    }
}
