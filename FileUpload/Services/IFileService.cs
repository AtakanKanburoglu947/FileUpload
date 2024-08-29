using FileUpload.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Services
{
    public interface IFileService
    {
       Task Upload(FileUploadDbModel model, string[] extensions);
       void Read(int id);
    }
}
