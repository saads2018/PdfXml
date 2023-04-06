using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Pdf_To_Xml.Models
{
    public class ExtraUserInfo
    {
        [Key]
        public string UserName { get; set; }
        public string? Name { get; set; }
        public string? About { get; set; }

    }
}

