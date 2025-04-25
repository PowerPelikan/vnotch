namespace MEEP.InputPipelines
{
    /// <summary>
    /// Represents an InputState. May be modified by 
    /// several InputProcessors along the chain.
    /// </summary>
    public struct ProcessedInputAction<ValueType> where ValueType : notnull
    {
        public bool WasConsumed { get; private set; }

        private ValueType value;

        public ValueType ProcessedValue
        {
            get
            {
                return WasConsumed ? default(ValueType) : value;
            }
        }

        public ProcessedInputAction(ValueType initialValue)
        {
            this.WasConsumed = false;
            this.value = initialValue;
        }

        public void Consume()
        {
            WasConsumed = true;
        }

    }
}
