using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ManageOnline.Infrastructure
{
    public static class ImageManager
    {
        public static Image ConvertToImage(byte[] arrayBinary)
        {
            Image rImage = null;

            using (MemoryStream ms = new MemoryStream(arrayBinary))
            {
                rImage = Image.FromStream(ms);
            }
            return rImage;
        }

    }
}