using FileUpload.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUpload
{
    public class FileUploadContext : DbContext
    {
        public FileUploadContext(DbContextOptions<FileUploadContext> options) : base(options)
        {

        }
        public DbSet<AppFile> Files { get; set; }
    }
}
