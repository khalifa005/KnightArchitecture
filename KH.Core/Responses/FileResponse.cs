using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Helper.Responses
{
    public class FileResponse
    {

        public FileContentResult? FileContentResult { get; set; }
        public bool IsValidToDownload { get; set; }
        public bool IsDeleted { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string OrignalName { get; set; }
        public string FileExtention { get; set; }
        public string FilePath { get; set; }

    }
}
