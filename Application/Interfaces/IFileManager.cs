using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileManager
    {
        Task<string> UploadImage(IFormFile file);
        void DeleteFile(string FileName);
    }
}
