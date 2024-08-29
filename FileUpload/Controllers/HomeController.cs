using FileUpload.Models;
using FileUpload.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FileUploadContext _context;
        private readonly IFileService _fileService;
        public HomeController(ILogger<HomeController> logger, FileUploadContext context, IFileService fileService)
        {
            _logger = logger;
            _context = context;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public void Read(int id)
        {
             _fileService.Read(id);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task Upload([Bind("FileUpload")] FileUploadDbModel model)
        {
            await _fileService.Upload(model, [".txt",".json",".xml"]);
        }
    
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
