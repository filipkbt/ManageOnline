using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ManageOnline.Infrastructure
{
    public static class FileHandler
    {
        public static byte[] GetBytesFromFile(HttpPostedFileBase file)
        {
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                return memoryStream.ToArray();
            }
        }
    }
}