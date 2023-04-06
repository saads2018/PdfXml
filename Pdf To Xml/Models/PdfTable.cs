using System.ComponentModel.DataAnnotations;

namespace Pdf_To_Xml.Models
{
	public class PdfTable
	{
		[Key]
		public int Id { get; set; }
		public string? UserName { get; set; }
		public byte[]? Pdf { get; set; }
        public byte[]? Xml { get; set; }
        public string? FileNames { get; set; }
        public byte[]? AddFile1 { get; set; }
        public byte[]? AddFile2 { get; set; }
        public string? savedTypes { get; set; }


    }
}
