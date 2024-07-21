using KH.Helper.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CA.Application.Helpers
{
    //inject it with interface
    public class FileManager
    {
        private readonly IOptions<FileSettings> _fileSettings;
        public FileManager(ILogger<FileManager> logger, IOptions<FileSettings> fileSettings)
        {
            //_logger = logger;
            _fileSettings = fileSettings;
        }

        public FileResponse UploadFile(IFormFile file)
        {
            try
            {
                if (_fileSettings == null || file is null)
                {
                    return new FileResponse() {  IsValidToDownload = false, FilePath = "" };
                }

                //add create folder if not exist ...
                //var folderName = Path.Combine("Resources", "Images");
                //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var pathToSave = _fileSettings.Value.FilePath;

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(_fileSettings.Value.FolderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    string fileExtention = Path.GetExtension(fullPath);

                    return new FileResponse() { 
                        IsValidToDownload = true,
                        FileName = fileName,
                        FilePath = dbPath,
                        FileExtention = fileExtention
                    };
                }
                else
                {
                    return new FileResponse() { IsValidToDownload = false, FilePath = "" };

                }
            }
            catch (Exception ex)
            {
                return new FileResponse() { IsValidToDownload = false, FilePath = "", Message = ex.Message };
            }
        }

        public IEnumerable<FileResponse> UploadMultipleFiles(IFormFileCollection files)
        {
            try
            {
                var filesResponse = new List<FileResponse>();
                //var formCollection = await Request.ReadFormAsync();
                //var files = formCollection.Files;

                //var folderName = Path.Combine("Resources", "Images");
                //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (_fileSettings == null || files is null || files.Count < 1)
                {
                    return filesResponse;
                }

                var pathToSave = _fileSettings.Value.FilePath;

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                if (files.Any(f => f.Length == 0))
                {
                    return filesResponse;
                }

                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    
                    var todayAsString = DateTime.Now.ToString("MM-dd-yyyy-h-mm");
                    var generatedFileName =string.Concat(todayAsString, '-', ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')) ;

                    var fullPath = Path.Combine(pathToSave, generatedFileName);
                    
                    var dbPath = Path.Combine(_fileSettings.Value.FolderName, generatedFileName);
                    
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    string fileExtention = Path.GetExtension(fullPath);

                    var uploadedFile = new FileResponse()
                    {
                        IsValidToDownload = true,
                        FileName = generatedFileName,
                        OrignalName = fileName,
                        FilePath = dbPath,
                        FileExtention = fileExtention
                    };

                    filesResponse.Add(uploadedFile);
                }

                return filesResponse;

            }
            catch (Exception ex)
            {
                return new List<FileResponse>();
            }
        }

        public async Task<FileResponse> Download(string filePath)
        {
            try
            {
                if (!IsValidFile(filePath))
                {
                    return new FileResponse() { IsValidToDownload = false, Message = "empty-path" };
                }

                var physicalPathToGet = _fileSettings.Value.FilePath;
                var fileName = System.IO.Path.GetFileName(filePath);

                var fullPath = Path.Combine(physicalPathToGet, fileName);

                var content = await System.IO.File.ReadAllBytesAsync(fullPath);

                new FileExtensionContentTypeProvider()
                    .TryGetContentType(fileName, out string contentType);

                var result = new FileContentResult(content, contentType);

                return new FileResponse()
                {
                    IsValidToDownload = true,
                    FileContentResult = result,
                    FileName = fileName
                };
            }
            catch (Exception ex)
            {
                return new FileResponse() { IsValidToDownload = false, Message = ex.Message };

            }
        }

        public async Task<FileResponse> DeleteFile(string filePath)
        {
            //we will keep the file but this in case we need to remove
            try
            {
                if (IsValidFile(filePath))
                {
                    throw new Exception("empty-file-path");
                }

                var pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (System.IO.File.Exists(pathToDelete))
                {
                    System.IO.File.Delete(pathToDelete);

                    return new FileResponse() { IsDeleted = true };
                }

                return new FileResponse() { IsDeleted = false };
            }
            catch (Exception ex)
            {
                return new FileResponse() { IsValidToDownload = false, Message = ex.Message };
            }
        }

        private bool IsPhoto(string fileName)
        {
            return fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                || fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidFile(string filePath)
        {
            if (_fileSettings == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(_fileSettings.Value.FilePath))
            {
                return false;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            //if (!File.Exists(filePath))
            //{
            //    return false;
            //}

            return true;
        }
    }
}
