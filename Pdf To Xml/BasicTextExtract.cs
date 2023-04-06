using org.pdfclown.documents.contents.objects;
using org.pdfclown.documents.contents;
using org.pdfclown.documents;
using org.pdfclown.documents.contents.fonts;
using org.pdfclown.files;
using File = org.pdfclown.files.File;
using org.pdfclown.tools;
using Pdf_To_Xml.Models;
using System.Drawing;
using org.pdfclown;
using Font = org.pdfclown.documents.contents.fonts.Font;
using System.Reflection;
using System.Resources;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using iTextSharp.text.pdf.parser;
using org.pdfclown.tokens;

namespace Pdf_To_Xml
{
    public class BasicTextExtraction
    {

        private string getBracketString(string storeText)
        {
            string val = "";
            if (storeText.Contains('(') && !storeText.Contains(')'))
                val = storeText.Substring(storeText.IndexOf('(') + 1);
            else if (storeText.Contains(')') && !storeText.Contains('('))
                val = storeText.Substring(0, storeText.IndexOf(')'));
            else
                val = storeText.Substring(storeText.IndexOf('(') + 1, storeText.IndexOf(')') - (storeText.IndexOf('(') + 1));

            return val;
        }

        public bool checkResxDiscard(string res, string text)
        {
            bool isPresent = false;
            ResourceManager resourceManager = new ResourceManager(res, Assembly.GetExecutingAssembly());
            ResourceSet resourceSet = resourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

            foreach (System.Collections.DictionaryEntry resource in resourceSet)
            {
                if (text.Contains(resource.Value.ToString()))
                    isPresent = true;
            }
            return isPresent;
        }

        private bool checkForNumLetters(string s)
        {
            bool valid = false;
            for(int i = 0;i<s.Length-1;i++)
            {
                if ((char.IsDigit(s[i]) && char.IsLetter(s[i+1])) || (char.IsLetter(s[i]) && char.IsDigit(s[i + 1])))
                {
                    valid = true;
                }
            }
            return valid;
        }

        private string Discard(string text)
        {
            string storeText = text;
            int error = 0;

            while (storeText.Contains('(')|| storeText.Contains(')'))
            {
                try
                {
                    if (!checkForNumLetters(getBracketString(storeText)) && !checkResxDiscard("Pdf_To_Xml.Resources.BracketDiscardExceptions", getBracketString(storeText)))
                    {
                        string val = "";
                        if (storeText.Contains('(') && !storeText.Contains(')'))
                            val = storeText.Substring(storeText.IndexOf('('));
                        else if (storeText.Contains(')') && !storeText.Contains('('))
                            val = storeText.Substring(0, storeText.IndexOf(')')+1);
                        else
                            val = storeText.Substring(storeText.IndexOf('('), storeText.IndexOf(')') - storeText.IndexOf('(') + 1);
                        
                        text = text.Replace(val, "");
                    }
                    var x = 1;

                    if (!storeText.Contains(')'))
                        storeText = "";
                    else
                        storeText = storeText.Substring(storeText.IndexOf(')') + 1);
                }
                catch(Exception ex)
                {
                    string msg = ex.ToString();
                    break;
                }
            }

            ResourceManager resourceManager = new ResourceManager("Pdf_To_Xml.Resources.DiscardString", Assembly.GetExecutingAssembly());
            ResourceSet resourceSet = resourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

            foreach (System.Collections.DictionaryEntry resource in resourceSet)
            {
                if (text.Contains(resource.Value.ToString()))
                    text = text.Replace(resource.Value.ToString(), "");
            }

            return text;
        }

        public void UploadFile(PdfTable pdf)
        {
            var pdfName = pdf.Pdf;

           
            using (File file = new File(new org.pdfclown.bytes.Buffer(pdfName)))
            {
                Document document = file.Document;

                // 2. Text extraction from the document pages.
                foreach (Page page in document.Pages)
                {
                   /*if(page.Index==20)
                    {
                        break;
                    }*/

                    Extract(new ContentScanner(page));
                }
            }
        }


        public List<WordDoc> Advanced(PdfTable pdf)
        {
            List<WordDoc> wordDocs = new List<WordDoc>();
            // 1. Opening the PDF file...
            var pdfName = pdf.Pdf;

            try
            {

                PdfReader pdfReader = new PdfReader(pdf.Pdf);

                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                   
                    try
                        {
                        
                        LocationTextExtractionStrategyEx location = new LocationTextExtractionStrategyEx();
                        var z = 1;
                        string text = PdfTextExtractor.GetTextFromPage(pdfReader, i, location);
                        var e = 1;
    
                        for (int x=0;x<location.TextLocationInfo.Count;x++)
                            {
                                try
                                {
                                    var textString = location.TextLocationInfo[x];
                                    if (!String.IsNullOrWhiteSpace(textString.Text))
                                    {
                                        WordDoc doc = new WordDoc();
                                        int length = textString.Text.IndexOf(textString.Text.Where(y => !char.IsWhiteSpace(y)).FirstOrDefault());
                                        //var align = location.TextCharacterInfos[x].Where(z => !char.IsWhiteSpace(z.Text[0])).FirstOrDefault();

                                       /* if(align!=null)
                                            doc.Alignment = (int)Math.Round(align.TopLeft[0]);
                                        else*/
                                        doc.Alignment = (int)Math.Round(textString.TopLeft[0]);
                                        
                                        doc.Type = "";
                                        doc.Text = Discard(textString.Text).Trim();

                                        try
                                        {
                                            if (!String.IsNullOrWhiteSpace(doc.Text) && doc.Text.Length >= 2 && doc.Text.Contains('.') && doc.Text.Replace(".", "").All(z => char.IsNumber(z)) && char.IsNumber(doc.Text[doc.Text.IndexOf('.') - 1]))
                                            {
                                                doc.Text = "";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            string msg = ex.ToString();
;                                       }


                                        if (!String.IsNullOrWhiteSpace(doc.Text))
                                        {
                                            wordDocs.Add(doc);
                                        }
                                    }
                                }
                                catch(Exception ex)
                                {
                                    string msg = ex.ToString();
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            string msg = ex.ToString();
                        }
                    }
            }
            catch(Exception ex)
            {
                string msg = ex.ToString();
            }


            return wordDocs;
        }
            private void Extract(ContentScanner level)
            {
            if (level == null)
                return;

            while (level.MoveNext())
            {
                ContentObject content = level.Current;
             
                if (content is ShowText)
                {
                    Font font = level.State.Font;
                    Console.WriteLine(font.Decode(((ShowText)content).Text));
                }
                else if (content is Text || content is ContainerObject)
                {
                    Extract(level.ChildLevel);
                }
            }
        }
    }
}
