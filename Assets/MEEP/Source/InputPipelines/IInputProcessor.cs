namespace MEEP.InputPipelines
{
    public delegate void IInputProcessor<InputValueType>(ref ProcessedInputAction<InputValueType> parameter);
}
