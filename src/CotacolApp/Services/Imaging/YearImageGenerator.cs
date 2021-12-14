using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Services.Extensions;
using CotacolApp.Services.Maps;
using Microsoft.AspNetCore.Hosting;

namespace CotacolApp.Services.Imaging
{
    public class YearImageGenerator : IYearImageGenerator
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public YearImageGenerator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<byte[]> CreateImageAsync(YearReview summary)
        {
            var fonts = new PrivateFontCollection();
            fonts.AddFontFile(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/cooper_black_regular.ttf"));
            var fontFamily = fonts.Families[0];

            var assembly = Assembly.GetAssembly(typeof(YearImageGenerator));
            var resourceName = "CotacolApp.StaticData.year-a.png";
            var rightAlignFormat = new StringFormat {Alignment = StringAlignment.Far};

            var rightBorder = 480f;
            var startHeight = 390f;
            var rowHeight = 82f;
            await using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                stream.Position = 0;
                var bitmap = new Bitmap(stream);
                using (var gfx = Graphics.FromImage(bitmap))
                {
                    using (Font f = new Font(fontFamily, 48, FontStyle.Bold))
                    {
                        gfx.DrawString($"{summary?.UniqueColsInYear.Number()}", f, Brushes.Black,
                            new RectangleF(0, startHeight, rightBorder, 100f), rightAlignFormat);
                        gfx.DrawString($"{summary?.PointsInYear.Number()}", f, Brushes.Black,
                            new RectangleF(0, startHeight + rowHeight, rightBorder, 100f), rightAlignFormat);
                        gfx.DrawString($"{(summary.DistanceInYear / 1000).Number()}", f, Brushes.Black,
                            new RectangleF(0, startHeight + rowHeight * 2, rightBorder, 100f), rightAlignFormat);
                        gfx.DrawString($"{summary?.ElevationInYear.Number()} m", f, Brushes.Black,
                            new RectangleF(0, startHeight + rowHeight * 3, rightBorder, 100f), rightAlignFormat);
                    }

                    using (Font f = new Font(fontFamily, 29, FontStyle.Bold))
                    {
                        gfx.DrawString($"#{summary?.MostPopularCol.CotacolId} {summary?.MostPopularCol.CotacolName}", f,
                            Brushes.Black, new RectangleF(148, 850, 804, 200f));
                        gfx.DrawString(
                                $"{summary?.MostPopularColCount} times"
                            , f, Brushes.White, new RectangleF(460, 801, 804, 200f));
                    }
                }
                using (var imgStream = new MemoryStream())
                {
                    bitmap.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);
                    return imgStream.ToArray();
                }
            }
        }
    }
}