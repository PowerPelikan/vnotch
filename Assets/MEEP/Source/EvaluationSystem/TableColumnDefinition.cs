using UnityEngine.Localization;

namespace MEEP.EvaluationSystem
{
    [System.Serializable]
    public class TableColumnDefinition
    {
        public enum ValueType { Integer, Decimal, String }

        public LocalizedString name;

        public ValueType valueType;
    }
}