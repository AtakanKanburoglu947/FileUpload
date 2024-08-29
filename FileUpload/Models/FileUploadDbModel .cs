using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FileUpload.Models
{
    public class FileUploadDbModel : PageModel
    {
        [BindProperty]
        public FileUploadDb FileUpload { get; set; }
    }
    public class FileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}
