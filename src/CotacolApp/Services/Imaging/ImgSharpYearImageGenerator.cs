using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Services.Extensions;
using Microsoft.AspNetCore.Hosting;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace CotacolApp.Services.Imaging
{
    public class ImgSharpYearImageGenerator : IYearImageGenerator
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImgSharpYearImageGenerator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public bool IsExperimental => true;

        public async Task<byte[]> CreateImageAsync(YearReview summary)
        {
            var fonts = new FontCollection();
            //FontFamily boldFamily = fonts.Install(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/cooper_black_regular.ttf"));
            var lightFamily = fonts.Install(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/merkury_bold.otf"));
            var assembly = Assembly.GetAssembly(typeof(GdiYearImageGenerator));
            var resourceName = "CotacolApp.StaticData.year-a.png";
            // var rightAlignFormat = new StringFormat {Alignment = StringAlignment.Far};

            var yearValuesRightBorder = 480f;
            var totalValuesLeftBorder = 520f;
            var startHeightYear = 405f;
            var startHeightTotals = 376f;
            var rowHeight = 82f;
            await using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var image = await Image.LoadAsync(stream);

                if (summary != null)
                {
                    var f = lightFamily.CreateFont(58, FontStyle.Bold);
                    image.Mutate(i => i.DrawText(new DrawingOptions(), "Blabl", f, Color.Black,
                        new PointF(startHeightYear, yearValuesRightBorder)));
                    //     gfx.DrawString($"{summary?.UniqueColsInYear.Number()}", f, Brushes.Black,
                    //         new RectangleF(0, startHeightYear, yearValuesRightBorder, 100f), rightAlignFormat);
                    //     gfx.DrawString($"{summary?.PointsInYear.Number()}", f, Brushes.Black,
                    //         new RectangleF(0, startHeightYear + rowHeight, yearValuesRightBorder, 100f),
                    //         rightAlignFormat);
                    //     gfx.DrawString($"{(summary.DistanceInYear / 1000).Number()}", f, Brushes.Black,
                    //         new RectangleF(0, startHeightYear + rowHeight * 2, yearValuesRightBorder, 100f),
                    //         rightAlignFormat);
                    //     gfx.DrawString($"{summary?.ElevationInYear.Number()} m", f, Brushes.Black,
                    //         new RectangleF(0, startHeightYear + rowHeight * 3, yearValuesRightBorder, 100f),
                    //         rightAlignFormat);
                    //
                    // using (Font f = new Font(lightFont, 20, FontStyle.Bold, GraphicsUnit.Pixel))
                    // {
                    //     gfx.DrawString($"{summary?.TotalCols.Number()} cols overall", f, Brushes.Black,
                    //         new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight,
                    //             yearValuesRightBorder, 100f));
                    //     gfx.DrawString($"{summary?.TotalPoints.Number()} points overall", f, Brushes.Black,
                    //         new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight * 2,
                    //             yearValuesRightBorder, 100f));
                    //     gfx.DrawString($"{(summary.TotalLength / 1000).Number()} km cols overall", f, Brushes.Black,
                    //         new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight * 3,
                    //             yearValuesRightBorder, 100f));
                    //     gfx.DrawString($"{summary?.TotalElevation.Number()} m elevation overall", f, Brushes.Black,
                    //         new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight * 4,
                    //             yearValuesRightBorder, 100f));
                    //     gfx.DrawString(summary?.UserName, f,
                    //         Brushes.Black, new RectangleF(0, 980, 1000, 100f), rightAlignFormat);
                    // }
                    //
                    // using (Font f = new Font(boldFont, 32, FontStyle.Bold, GraphicsUnit.Pixel))
                    // {
                    //     gfx.DrawString(
                    //         $"#{summary?.MostPopularCol.CotacolId} {summary?.MostPopularCol.CotacolName}", f,
                    //         Brushes.Black, new RectangleF(156, 855, 804, 200f));
                    //     gfx.DrawString(
                    //         $"{summary?.MostPopularColCount} times"
                    //         , f, Brushes.White, new RectangleF(462, 810, 804, 200f));
                    // }
                }
                else
                {
                    // using (Font f = new Font(boldFont, 32, FontStyle.Bold, GraphicsUnit.Pixel))
                    // {
                    //     gfx.DrawString(
                    //         "The year review generation went wrong.  Did you conquer Cotacols, this year?", f,
                    //         Brushes.Black,
                    //         new RectangleF(84, 520, yearValuesRightBorder, 350f));
                    // }
                }

                using (var imgStream = new MemoryStream())
                {
                    await image.SaveAsPngAsync(imgStream);
                    return imgStream.ToArray();
                }
            }
        }
    }
}