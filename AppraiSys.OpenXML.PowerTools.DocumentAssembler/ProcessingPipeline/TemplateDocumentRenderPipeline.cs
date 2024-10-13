namespace AppraiSys.OpenXML.PowerTools.DocumentAssembler.ProcessingPipeline;

public class TemplateDocumentRenderPipeline<T>
{
    private readonly List<IPipelineStep<T>> _steps = new List<IPipelineStep<T>>();

    public TemplateDocumentRenderPipeline<T> Register(IPipelineStep<T> step)
    {
        _steps.Add(step);
        return this;
    }

    public async Task<T> Execute(T input)
    {
        var result = input;

        foreach (var step in _steps)
        {
            result = await step.Process(result);
        }

        return result;
    }
}
