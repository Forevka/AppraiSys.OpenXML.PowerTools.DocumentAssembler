using System.Xml.Linq;

namespace AppraiSys.OpenXML.PowerTools.DocumentAssembler.ProcessingPipeline;

public class DocumentPipelineData
{
    public Guid DocumentId { get; set; }
    public MemoryStream DocumentData { get; set; }
    public XElement TemplateValues { get; set; }
}
