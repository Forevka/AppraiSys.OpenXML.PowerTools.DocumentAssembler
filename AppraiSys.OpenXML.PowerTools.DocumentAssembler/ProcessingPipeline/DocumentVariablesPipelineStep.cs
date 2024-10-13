using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace AppraiSys.OpenXML.PowerTools.DocumentAssembler.ProcessingPipeline;
public class DocumentVariablesPipelineStep : IPipelineStep<DocumentPipelineData>
{
    public async Task<DocumentPipelineData> Process(DocumentPipelineData input)
    {
        var documentServer = new RichEditDocumentServer();
        documentServer.LoadDocument(input.DocumentData);

        documentServer.CalculateDocumentVariable += async (sender, args) =>
        {
            var doc1 = await GenerateTableWithImages();

            // Set the field value to the RichEditDocumentServer instance:
            args.Value = doc1;
            args.Handled = true;
        };

        documentServer.Document.UpdateAllFields();

        using var populatedDocVariablesDocument = new MemoryStream();
        documentServer.SaveDocument(populatedDocVariablesDocument, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);

        return new DocumentPipelineData
        {
            DocumentData = populatedDocVariablesDocument,
            TemplateValues = input.TemplateValues,
            DocumentId = input.DocumentId,
        };
    }


    public static async Task<Document> GenerateTableWithImages()
    {
        var documentServer = new RichEditDocumentServer();
        var document = documentServer.Document;

        // Create a table
        var table = document.Tables.Create(document.Range.End, 2, 2);

        // Insert images into the first row
        for (var colIndex = 0; colIndex < 2; colIndex++)
        {
            var cell = table[0, colIndex];
            var cellSubDoc = cell.Range.BeginUpdateDocument();

            // Load an image from a file or resource
            await using (var imageStream = DownloadImageStreamAsync().Result)
            {
                // Insert the image 
                document.Images.Insert(table.Rows[colIndex].Cells[1].Range.Start, DocumentImageSource.FromStream(imageStream));
                // Optionally, scale the image
                //image.ScaleX = 50; // Scale to 50% of the original size
                //image.ScaleY = 50;


                document.InsertText(table.Rows[colIndex].Cells[0].Range.Start, $"Caption {colIndex + 1}");
            }

            cell.Range.EndUpdateDocument(cellSubDoc);
        }

        return document;
    }


    public static async Task<Stream> DownloadImageStreamAsync()
    {
        const string url = "https://picsum.photos/100/100";
        using var client = new HttpClient();

        // Download the image data as a byte array
        var imageData = await client.GetByteArrayAsync(url);

        // Create a MemoryStream from the image data
        var imageStream = new MemoryStream(imageData);

        // Set the stream position to the beginning
        imageStream.Position = 0;

        return imageStream;
    }
}
