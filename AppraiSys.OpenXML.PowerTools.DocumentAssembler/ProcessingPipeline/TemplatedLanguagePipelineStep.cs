namespace AppraiSys.OpenXML.PowerTools.DocumentAssembler.ProcessingPipeline;

public class TemplatedLanguagePipelineStep : IPipelineStep<DocumentPipelineData>
{
    public async Task<DocumentPipelineData> Process(DocumentPipelineData input)
    {
        var wmlDoc = new WmlDocument(input.DocumentId.ToString(), input.DocumentData);
        var populatedTemplateValuesDocument = DocumentAssembler.AssembleDocument(wmlDoc, input.TemplateValues);

        populatedTemplateValuesDocument.Position = 0;

        return new DocumentPipelineData
        {
            DocumentData = populatedTemplateValuesDocument,
            DocumentId = input.DocumentId,
            TemplateValues = input.TemplateValues,
        };
    }
}
