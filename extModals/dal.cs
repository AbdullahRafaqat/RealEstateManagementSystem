using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace RealEstate.extModals
{
    public class dal
    {
        public static byte[] ResizeUploadedImage(Stream streamToResize)
        {
            byte[] resizedImage;
            using (Image orginalImage = Image.FromStream(streamToResize))
            {
                ImageFormat orginalImageFormat = orginalImage.RawFormat;
                int orginalImageWidth = orginalImage.Width;
                int orginalImageHeight = orginalImage.Height;
                int resizedImageWidth = 110; // Type here the width you want
                int resizedImageHeight = 75; // Convert.ToInt32(resizedImageWidth * orginalImageHeight / orginalImageWidth);
                using (Bitmap bitmapResized = new Bitmap(orginalImage, resizedImageWidth, resizedImageHeight))
                {
                    using (MemoryStream streamResized = new MemoryStream())
                    {
                        bitmapResized.Save(streamResized, orginalImageFormat);
                        resizedImage = streamResized.ToArray();
                    }
                }
            }

            return resizedImage;
        }
        public static byte[] ResizeUplImage(Stream streamToResize)
        {
            byte[] resizedImage;
            using (Image orginalImage = Image.FromStream(streamToResize))
            {
                ImageFormat orginalImageFormat = orginalImage.RawFormat;
                int orginalImageWidth = orginalImage.Width;
                int orginalImageHeight = orginalImage.Height;
                int resizedImageWidth = 870;
                int resizedImageHeight = 496;
                using (Bitmap bitmapResized = new Bitmap(orginalImage, resizedImageWidth, resizedImageHeight))
                {
                    using (MemoryStream streamResized = new MemoryStream())
                    {
                        bitmapResized.Save(streamResized, orginalImageFormat);
                        resizedImage = streamResized.ToArray();
                    }
                }
            }

            return resizedImage;
        }
    }
}