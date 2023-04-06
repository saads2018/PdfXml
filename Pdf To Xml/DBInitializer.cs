using Microsoft.EntityFrameworkCore;
using Pdf_To_Xml.Data;
using Pdf_To_Xml.Models;

namespace BlazorApp1
{
    public class DBInitializer
    {
        public ApplicationDbContext context1;
        public void Initialize(ApplicationDbContext context)
        {
            context1 = context;
        }

        public void Add(PdfTable pdf)
        {
            context1.PdfTable.Add(pdf);
            context1.SaveChanges();
        }
    }
}
