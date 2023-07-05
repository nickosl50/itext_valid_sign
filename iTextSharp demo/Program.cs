// See https://aka.ms/new-console-template for more information
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using iText.IO.Image;

foreach (string file in Directory.GetFiles("./../../../pdf", "*.pdf"))
{
    AddImageStampToPDF(file, file.ToLower().Replace(".pdf", "_stamp.pdf"));
}

static void AddImageStampToPDF(string sourceFilePath, string targetFilePath)
{
    string imagePath = "./../../../stamp.png";
    float x = 10;
    float y = 200;
    float width = 400;
    float height = 200;

    //for (var i = 1; i <= 9; i++)
    using (PdfReader reader = new PdfReader(sourceFilePath))
    {
        using (PdfWriter writer = new PdfWriter(targetFilePath))//.Replace(".pdf", "_" + i + ".pdf")
        {
            using (PdfDocument pdfDoc = new PdfDocument(reader, writer, new StampingProperties().UseAppendMode()))
            {
                Document doc = new Document(pdfDoc);

                // Load the image
                iText.Kernel.Pdf.Canvas.PdfCanvas canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(pdfDoc.GetFirstPage(), true);
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory.Create(imagePath));

                var page = pdfDoc.GetFirstPage();
                var pageRotation = page.GetRotation();
                var imageRotation = 0;

                switch (pageRotation)
                {
                    case 90:
                        var t1 = x;
                        x = page.GetPageSize().GetWidth() - y;
                        y = t1;
                        imageRotation = 270;
                        break;
                    case 180:
                        x = page.GetPageSize().GetWidth() - x;
                        y = page.GetPageSize().GetHeight() - y;
                        imageRotation = 180;
                        break;
                    case 270:
                        var t2 = x;
                        x = y;
                        y = page.GetPageSize().GetHeight() - t2;
                        imageRotation = 90;
                        break;
                }

                // Set the position and size of the image stamp
                image.SetFixedPosition(x, y);
                image.SetHeight(height);
                image.SetWidth(width);
                image.SetRotationAngle(-(Math.PI / 180) * imageRotation);

                // Add the image to the document
                doc.Add(image);

                doc.Close();
            }
        }
    }
}
