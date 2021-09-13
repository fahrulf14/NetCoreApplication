using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZXing.QrCode;
using System.Drawing;
using System.IO;

namespace NUNA.Controllers.Tags
{
    [HtmlTargetElement("barcode")]
    public class BarCodeTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = context.AllAttributes["content"].Value.ToString();
            var width = int.Parse(context.AllAttributes["width"].Value.ToString());
            var height = int.Parse(context.AllAttributes["height"].Value.ToString());
            var barCodeWriterPixelData = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0
                }
            };
            var pixelData = barCodeWriterPixelData.Write(content);
            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using var memoryStream = new MemoryStream();
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            output.TagName = "img";
            output.Attributes.Clear();
            output.Attributes.Add("width", width);
            output.Attributes.Add("height", height);
            output.Attributes.Add("src", string.Format("data:image/png;base64,{0}", Convert.ToBase64String(memoryStream.ToArray())));
        }
    }
}
