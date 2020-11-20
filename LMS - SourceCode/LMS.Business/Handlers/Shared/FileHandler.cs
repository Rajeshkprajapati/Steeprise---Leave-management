using LMS.Business.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Business.Handlers.Shared
{
    public class FileHandler: IFileHandler
    {
        public async Task<MemoryStream> FileToStream(string filePath)
        {
            var mStream = new MemoryStream();
            using (var fstream =new FileStream(filePath, FileMode.Open))
            {
                await fstream.CopyToAsync(mStream);
            }
            mStream.Position = 0;
            return mStream;
        }
    }
}
