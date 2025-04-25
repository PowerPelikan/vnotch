namespace MEEP.InputPipelines
{
    /// <summary>
    /// Marks a class as able to place inputs into the pipeline
    /// </summary>
    public interface IInputPublisher<InputValueType> where InputValueType : notnull
    {

        public void SendInput(InputValueType newValue);

    }
}
