using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pdf_To_Xml.Models;

namespace Pdf_To_Xml.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PdfTable> PdfTable { get; set; }

        public DbSet<ExtraUserInfo> ExtraUserInfo { get; set; }

    }
}