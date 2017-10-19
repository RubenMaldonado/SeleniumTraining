using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SeleniumTraining.Tools
{
    public class PdfGenerator
    {
        public List<string> ImageFileNames { get; set; }

        public string FileName { get; }

        public PdfGenerator()
        {
            FileName = GetFileName();
        }

        public void GeneratePdf()
        {

            using (var fileStream = new FileStream(FileName, FileMode.Create))
            {
            
                var document = new Document(PageSize.LETTER, 10,10,10,10);

                PdfWriter writer = PdfWriter.GetInstance( document, fileStream );

                document.Open();

                var font = new Font { Size = 6f };

                var phrase = new Phrase( "Hi Im a Test" ) { Font = font };


                document.Add(phrase);


                if ( ImageFileNames == null || ImageFileNames.Count <= 0 ) return;
                var para = new Paragraph("Screenshots: ");

                document.Add(para);

                foreach ( string image in ImageFileNames)
                {
                    Image img = Image.GetInstance( image );
                    img.SpacingAfter = 15f;
                    img.Alignment = Element.ALIGN_CENTER;
                    img.ScaleToFit( 350f,350f );

                    var imageNamePhrase= new Phrase(image, font);

                    document.Add( (imageNamePhrase));
                    document.Add( img );

                }

                document.Close();

                writer.Close();
            }
        }

        private static string GetFileName()
        {
            const string extension = ".pdf";
            string pdfDirectory = $@"{AppDomain.CurrentDomain.BaseDirectory}/PDFReports";
            string uniquePart = $@"{Guid.NewGuid()}";
            string dateTime = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}-" +
                              $"{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";

            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            return $@"{pdfDirectory}/{dateTime}-{uniquePart}{extension}";
        }

    }
}
