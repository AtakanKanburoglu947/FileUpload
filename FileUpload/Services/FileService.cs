using FileUpload.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
namespace FileUpload.Services
{
    public class FileService : IFileService
    {
        private readonly FileUploadContext _context;
        public FileService(FileUploadContext context)
        {
            _context = context;
            
        }
  
        public string GetUploadedFileExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }
            int index = fileName.IndexOf('.');
            if (index >= 0 && index < fileName.Length - 1)
            {
                return fileName.Substring(index);
            }
            return string.Empty;
        }
   
        public void Read(int id)
        {
          AppFile? file =  _context.Files.FirstOrDefault(file=> file.Id == id);
          string extension = GetUploadedFileExtension(file.FileName);
            
            if (file == null) { Console.WriteLine("Dosya bulunamadı"); }
            else
            {
                string result = extension switch
                {
                    ".txt" => ReadTxt(file.Content),
                    ".json" => ReadJson(file.Content),
                    ".xml" => ReadXML(file.Content),
                    _ => "Dosya tipi geçerli değil"
                };

            }

        }
        
        string ReadTxt(byte[] fileContent)
        {
            return Encoding.UTF8.GetString(fileContent);
        }
        string ReadJson(byte[] fileContent)
        {
            try
            {
                string json = Encoding.UTF8.GetString(fileContent);
                JsonDocument document = JsonDocument.Parse(json);
                using (document)
                {
                    string formattedJsonString = JsonSerializer.Serialize(document);
                    return formattedJsonString;
                }
            }
            catch (JsonException exception)
            {

                return exception.ToString();
            }
   
        }
        string ReadXML(byte[] fileContent)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(fileContent);
                using (memoryStream)
                {
                    XDocument xDocument = XDocument.Load(memoryStream);
                    return xDocument.ToString();
                }
            }
            catch (Exception exception)
            {

                return exception.ToString();
            }

        }
        public async Task Upload(FileUploadDbModel model, string[] extensions)
        {
            MemoryStream memoryStream = new MemoryStream();
            string fileName = model.FileUpload.FormFile.FileName;
            using (memoryStream)
            {
                await model.FileUpload.FormFile.CopyToAsync(memoryStream);
                bool validate = ValidateFile(fileName, extensions);
                if (!validate)
                {
                    Console.WriteLine("Yanlış dosya formatı");
                    return;
                }
                if (memoryStream.Length < 2097152)
                {
                    AppFile file = new AppFile()
                    {
                        Content = memoryStream.ToArray(),
                        FileName = fileName
                    };
       
                    _context.Files.Add(file);
                    await _context.SaveChangesAsync();
                    
                }
                else
                {
                    Console.WriteLine("Dosya boyutu çok büyük");
                    return;

                }

            }
        }

         bool ValidateFile(string fileName, string[] permittedExtensions)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
            {
                return false;   
            }
            return true;
        }

     
    }
}
