using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Runtime.InteropServices.ComTypes;

namespace pdf_keyword_searcher
{
    public static class Program
    {
        public static void Main()
        {
            string dataDir = "\\\";

            var sb = new StringBuilder();

            using (var document = PdfDocument.Open(dataDir + "2020_Book_LinearProgramming" + ".pdf"))
            {
                Word previous = null;
                foreach (var page in document.GetPages())
                {
                    foreach (var word in page.GetWords())
                    {
                        if (previous != null)
                        {
                            var hasInsertedWhitespace = false;
                            var bothNonEmpty = previous.Letters.Count > 0 && word.Letters.Count > 0;
                            if (bothNonEmpty)
                            {
                                var prevLetter1 = previous.Letters[0];
                                var currentLetter1 = word.Letters[0];

                                var baselineGap = Math.Abs(prevLetter1.StartBaseLine.Y - currentLetter1.StartBaseLine.Y);

                                if (baselineGap > 3)
                                {
                                    hasInsertedWhitespace = true;
                                    sb.AppendLine();
                                }
                            }

                            if (!hasInsertedWhitespace)
                            {
                                sb.Append(" ");
                            }
                        }

                        sb.Append(word.Text);

                        previous = word;
                    }
                }
            }
            // Create a writer and open the file
            TextWriter tw = new StreamWriter(dataDir + "extracted-text.txt");

            // Write a line of text to the file
            tw.Write(sb.ToString());

            // Close the stream
            tw.Close();
        }
    }
}