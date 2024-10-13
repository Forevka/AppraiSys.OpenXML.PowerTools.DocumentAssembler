namespace AppraiSys.OpenXML.PowerTools.DocumentAssembler.ProcessingPipeline;

public interface IPipelineStep<T>
{
    Task<T> Process(T input);
}