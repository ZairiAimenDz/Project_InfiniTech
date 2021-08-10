using Application.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Repositories
{
    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileManager(IWebHostEnvironment hostEnvironment)
        {
            this.webHostEnvironment = hostEnvironment;
        }

        public void DeleteFile(string FileName)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                string FiletoDelete = Path.Combine(webHostEnvironment.WebRootPath, "images", FileName);
                if (File.Exists(FiletoDelete))
                {
                    File.Delete(FiletoDelete);
                }
            }
        }

        public Task<string> UploadImage(IFormFile FormFile)
        {
            string uniqueFileName = null;

            if (FormFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + FormFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    FormFile.CopyTo(fileStream);
                }
            }
            return Task.FromResult(uniqueFileName);
        }

        public async Task<string> UploadImage(IBrowserFile bfile)
        {
            string uniqueFileName = null;
            var file = bfile.OpenReadStream(maxAllowedSize: 4096000);

            if (file is not null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + bfile.Name;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
