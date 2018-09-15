using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace CZE.Web.Util.Helper
{
    public static partial class MyExtensions
    {
        public static Image Resize(this Image current, int maxWidth, int maxHeight)
        {
            int width, height;
            #region reckon size 
            if (current.Width > current.Height)
            {
                width = Convert.ToInt32(current.Width * maxWidth / (double)current.Height);
                height = maxHeight;
            }
            else
            {
                width = maxWidth;
                height = Convert.ToInt32(current.Height * maxHeight / (double)current.Width);
            }
            #endregion

            #region get resized bitmap 
            var canvas = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = current.Height > maxHeight || current.Width > maxWidth ?
                    InterpolationMode.HighQualityBilinear : InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(current, 0, 0, width, height);
            }

            return canvas;
            #endregion
        }

        public static Image Crop(this Image current, int maxWidth, int maxHeight)
        {
            int restW = 0, restH = 0;
     
                restW = (current.Width - maxWidth)/2;                     
                restH = (current.Height - maxHeight)/2;

            var canvas = new Bitmap(maxWidth, maxHeight);
            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(current, -restW , -restH , current.Width, current.Height);
            }
            return canvas;
        }

        public static byte[] ToByteArray(this Image current)
        {
            using (var stream = new MemoryStream())
            {
                current.Save(stream, current.RawFormat);
                return stream.ToArray();
            }
        }


    }
}
