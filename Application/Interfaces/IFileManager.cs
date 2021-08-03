using Microsoft.AspNetCore.Components.Forms;
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
        Task<string> UploadImage(IBrowserFile file);
        void DeleteFile(string FileName);
    }
}
