namespace Pdf_To_Xml
{
	public class ReferCode
    {

        /*
		
		while (i < wordDocs.Count)
			{
				WordDoc wordDoc = wordDocs[i];

				if (wordDoc.Text.Contains(' ') || wordDoc.Text.Contains('.'))
				{
					wordDoc.Text = " " + wordDoc.Text;

					if (state == 0)
					{
						WordDoc word = new WordDoc();
						word.Type = "neutral";
						word.Text = wordDoc.Text;
						XmlList.Add(word);
						state = 1;
					}
					else if (state == 1)
					{
						XmlList[XmlList.Count - 1].Text += wordDoc.Text;
					}
					i += 1;
				}
				else if(!String.IsNullOrWhiteSpace(wordDoc.Text))
				{
					WordDoc word = new WordDoc();

					if (wordDoc.Text.ToLower() == "woody")
						word.Type = "friendly";
					else if (wordDoc.Text.ToLower() == "tony")
						word.Type = "tall";
					else
						word.Type = "unknown";

					word.Text = "";
					XmlList.Add(word);

					i += 1;
					while (i < wordDocs.Count && !wordDocs[i].Text.Trim().Contains("---PAGE BREAK---") && (wordDocs[i].Text.Trim().Contains(' ') || wordDocs[i].Text.Trim().Contains('.')) && !String.IsNullOrWhiteSpace(wordDocs[i].Text))
					{
						wordDocs[i].Text=" "+wordDocs[i].Text;
						XmlList[XmlList.Count - 1].Text += wordDocs[i].Text;
						i += 1;
					}
					state = 0;
				}
				else if(String.IsNullOrWhiteSpace(wordDoc.Text))
				{
					i += 1;
				}
				
				if (wordDocs[i].Text == "---PAGE BREAK---")
				{
					state = 0;
					i += 1;
				}

			}

		 */
        /*
		 
		 SautinSoft.PdfFocus sautinSoft=new SautinSoft.PdfFocus();
			sautinSoft.OpenPdf(pdfRecord.Pdf);
			string text = sautinSoft.ToHtml(1, 20);
			string []textLines = text.Split("\r\n");

			for (int j = 1; j<= (reader.NumberOfPages<20? reader.NumberOfPages:20); j++)
			{
				LocationTextExtractionStrategy lx = new LocationTextExtractionStrategy();
				string pageText = PdfTextExtractor.GetTextFromPage(reader,j,lx);
				string[] pageLines = pageText.Split('\n');

				for(int x=0;x<pageLines.Length;x++)
				{
					pageLines[x] = Discard(pageLines[x]);
					if (!String.IsNullOrWhiteSpace(pageLines[x]) && pageLines.Length>=2 && char.IsDigit(pageLines[x].Trim()[0]) && pageLines[x].Trim()[1]=='.')
					{
						pageLines[x] = "";
					}
				}
				lines.AddRange(pageLines.ToArray());
				lines.Add("---PAGE BREAK---");
			}


			/*
			* var docHtml = new HtmlDocument();
		docHtml.Load("Output.html");

		var nodes = docHtml.DocumentNode.Descendants("div").ToList();
		string s =""; 
			*/



        /*for(int x=0;x < lines1.Count;x++)
            {
            for (int y = 0; y < lines.Count; y++)
                {	
                if (lines1[x].ToLower().Contains("trial"))
                    {
                    string current=lines1[x];
                    string [] sentences1=lines1[x].ToLower().Split(' ');
                    string[] sentences = lines[y].ToLower().Split(' ');

                    if(compare(sentences1,sentences))
                        {
                        lines1[x] = lines[y];
                        break;
            }
            }
            }
        }*/

        /*
		 
		 public List<WordDoc> Advanced(PdfTable pdf)
        {
            List<WordDoc> wordDocs = new List<WordDoc>();
            // 1. Opening the PDF file...
            var pdfName = pdf.Pdf;

            


            try
            {
                using (File file = new File(new org.pdfclown.bytes.Buffer(pdfName)))
                {
                    Document document = file.Document;

                    TextExtractor extractor = new TextExtractor(true,true);

                    foreach (Page page in document.Pages)
                    {
                        try
                        {
                            IList<ITextString> textStrings = extractor.Extract(page)[TextExtractor.DefaultArea];
                            foreach (ITextString textString in textStrings)
                            {
                                try
                                {
                                    if (!String.IsNullOrWhiteSpace(textString.Text))
                                    {
                                        RectangleF textStringBox = textString.Box.Value;
                                        WordDoc doc = new WordDoc();
                                        doc.Alignment = (int)Math.Round(textString.TextChars.Where(z => !char.IsWhiteSpace(z.Value)).FirstOrDefault().Box.X);
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
                                    int x = page.Index;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            string msg = ex.ToString();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string msg = ex.ToString();
            }


            return wordDocs;
        }
		 */

    }
}
