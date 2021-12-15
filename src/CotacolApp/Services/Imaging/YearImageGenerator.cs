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
            fonts.AddFontFile(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/merkury_bold.otf"));
            // fonts.AddFontFile(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/lekton-regular.ttf"));
            //fonts.AddFontFile(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/cooper_light_bt-webfont.ttf"));
            var boldFont = fonts.Families[0];
            var lightFont = fonts.Families[1];

            var assembly = Assembly.GetAssembly(typeof(YearImageGenerator));
            var resourceName = "CotacolApp.StaticData.year-a.png";
            var rightAlignFormat = new StringFormat {Alignment = StringAlignment.Far};

            var yearValuesRightBorder = 480f;
            var totalValuesLeftBorder = 520f;
            var startHeightYear = 390f;
            var startHeightTotals = 373f;
            var rowHeight = 82f;
            await using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                stream.Position = 0;
                var bitmap = new Bitmap(stream);
                using (var gfx = Graphics.FromImage(bitmap))
                {
                    if (summary != null)
                    {
                        using (Font f = new Font(boldFont, 48, FontStyle.Bold))
                        {
                            gfx.DrawString($"{summary?.UniqueColsInYear.Number()}", f, Brushes.Black,
                                new RectangleF(0, startHeightYear, yearValuesRightBorder, 100f), rightAlignFormat);
                            gfx.DrawString($"{summary?.PointsInYear.Number()}", f, Brushes.Black,
                                new RectangleF(0, startHeightYear + rowHeight, yearValuesRightBorder, 100f),
                                rightAlignFormat);
                            gfx.DrawString($"{(summary.DistanceInYear / 1000).Number()}", f, Brushes.Black,
                                new RectangleF(0, startHeightYear + rowHeight * 2, yearValuesRightBorder, 100f),
                                rightAlignFormat);
                            gfx.DrawString($"{summary?.ElevationInYear.Number()} m", f, Brushes.Black,
                                new RectangleF(0, startHeightYear + rowHeight * 3, yearValuesRightBorder, 100f),
                                rightAlignFormat);
                        }

                        using (Font f = new Font(lightFont, 16, FontStyle.Bold))
                        {
                            gfx.DrawString($"{summary?.TotalCols.Number()} cols overall", f, Brushes.Black,
                                new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight,
                                    yearValuesRightBorder, 100f));
                            gfx.DrawString($"{summary?.TotalPoints.Number()} points overall", f, Brushes.Black,
                                new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight * 2,
                                    yearValuesRightBorder, 100f));
                            gfx.DrawString($"{(summary.TotalLength / 1000).Number()} km cols overall", f, Brushes.Black,
                                new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight * 3,
                                    yearValuesRightBorder, 100f));
                            gfx.DrawString($"{summary?.TotalElevation.Number()} m elevation overall", f, Brushes.Black,
                                new RectangleF(totalValuesLeftBorder, startHeightTotals + rowHeight * 4,
                                    yearValuesRightBorder, 100f));
                            gfx.DrawString(summary?.UserName, f,
                                Brushes.Black, new RectangleF(0, 980, 1000, 100f), rightAlignFormat);
                        }

                        using (Font f = new Font(boldFont, 29, FontStyle.Bold))
                        {
                            gfx.DrawString(
                                $"#{summary?.MostPopularCol.CotacolId} {summary?.MostPopularCol.CotacolName}", f,
                                Brushes.Black, new RectangleF(148, 850, 804, 200f));
                            gfx.DrawString(
                                $"{summary?.MostPopularColCount} times"
                                , f, Brushes.White, new RectangleF(462, 804, 804, 200f));
                        }
                    }
                    else
                    {
                        using (Font f = new Font(boldFont, 32, FontStyle.Bold))
                        {
                            gfx.DrawString("The year review generation went wrong.  Did you conquer Cotacols, this year?", f, Brushes.Black,
                                new RectangleF(84, 520, yearValuesRightBorder, 350f));
                        }
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