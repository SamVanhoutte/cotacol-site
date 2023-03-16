using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using Cotacol.Website.Interfaces;
using Cotacol.Website.Models.CotacolApi;
using Cotacol.Website.Services.Extensions;
using Microsoft.AspNetCore.Hosting;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace Cotacol.Website.Services.Imaging
{
    public class ImgSharpYearImageGenerator : IYearImageGenerator
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImgSharpYearImageGenerator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public bool IsExperimental => false;

        public async Task<byte[]> CreateImageAsync(YearReview summary)
        {
            var fonts = new FontCollection();
            var boldFamily =
                fonts.Install(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/cooper_black_regular.ttf"));
            var lightFamily = fonts.Install(Path.Combine(_hostingEnvironment.WebRootPath, "fonts/lekton-regular.ttf"));
            var assembly = Assembly.GetAssembly(typeof(GdiYearImageGenerator));
            var resourceName = "CotacolApp.StaticData.year-a.png";

            var yearValuesRightBorder = 240f;
            var totalValuesLeftBorder = 520f;
            var startHeightYear = 325f;
            var startHeightTotals = 300f;
            var rowHeight = 80f;

            DrawingOptions rightAlignFormat = new DrawingOptions()
            {
                TextOptions = new TextOptions
                {
                    WrapTextWidth = yearValuesRightBorder,
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            };
            await using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var image = await Image.LoadAsync(stream);

                if (summary != null)
                {
                    // Year values
                    var bigFont = boldFamily.CreateFont(58, FontStyle.Bold);
                    image.Mutate(
                        i =>
                        {
                            i.DrawText(rightAlignFormat,
                                $"{summary?.YearCotacolActivityCount.Number()}/{summary?.YearAllActivityCount.Number()}",
                                bigFont, Color.Black,
                                new PointF(yearValuesRightBorder, startHeightYear));
                            i.DrawText(rightAlignFormat, $"{summary?.UniqueColsInYear.Number()}", bigFont, Color.Black,
                                new PointF(yearValuesRightBorder, startHeightYear + rowHeight));
                            i.DrawText(rightAlignFormat, $"{summary?.PointsInYear.Number()}", bigFont, Color.Black,
                                new PointF(yearValuesRightBorder, startHeightYear + rowHeight * 2));
                            i.DrawText(rightAlignFormat, $"{(summary.DistanceInYear / 1000).Number()}", bigFont,
                                Color.Black,
                                new PointF(yearValuesRightBorder, startHeightYear + rowHeight * 3));
                            i.DrawText(rightAlignFormat, $"{summary.ElevationInYear.Number()}", bigFont, Color.Black,
                                new PointF(yearValuesRightBorder, startHeightYear + rowHeight * 4));
                        });
                    // Total values
                    var smallFont = lightFamily.CreateFont(24, FontStyle.Bold);
                    if (summary?.HeaviestActivity != null)
                    {
                        image.Mutate(
                            i => i.DrawText(new DrawingOptions(),
                                $"Toughest activity: {(summary.HeaviestActivity.TotalPoints).Number()} pts - {summary.HeaviestActivity.UniqueCols.Number()} cols",
                                smallFont, Color.Black,
                                new PointF(totalValuesLeftBorder, startHeightTotals + rowHeight))
                        );
                    }

                    image.Mutate(
                        i =>
                        {
                            i.DrawText(new DrawingOptions(), $"{summary.TotalCols.Number(belowZeroIsNull:false)} cols overall", smallFont,
                                Color.Black,
                                new PointF(totalValuesLeftBorder, startHeightTotals + rowHeight * 2));
                            i.DrawText(new DrawingOptions(), $"{summary.TotalPoints.Number(belowZeroIsNull:false)} points overall",
                                smallFont, Color.Black,
                                new PointF(totalValuesLeftBorder, startHeightTotals + rowHeight * 3));
                            i.DrawText(new DrawingOptions(), $"{(summary.TotalLength / 1000).Number(belowZeroIsNull:false)} km cols overall",
                                smallFont, Color.Black,
                                new PointF(totalValuesLeftBorder, startHeightTotals + rowHeight * 4));
                            i.DrawText(new DrawingOptions(), $"{summary.TotalElevation.Number(belowZeroIsNull:false)} m elevation overall",
                                smallFont, Color.Black,
                                new PointF(totalValuesLeftBorder, startHeightTotals + rowHeight * 5));
                            if (!string.IsNullOrEmpty(summary.UserName))
                            {
                                i.DrawText(rightAlignFormat, summary.UserName, smallFont, Color.Black,
                                    new PointF(760, 990));
                            }
                        });
                    // Col values
                    var regularFont = boldFamily.CreateFont(32, FontStyle.Bold);
                    image.Mutate(
                        i =>
                        {
                            if (summary?.MostPopularCol != null)
                            {
                                i.DrawText(new DrawingOptions(),
                                    $"#{summary?.MostPopularCol.CotacolId} {summary?.MostPopularCol.CotacolName}",
                                    regularFont, Color.Black,
                                    new PointF(156, 855));

                                i.DrawText(new DrawingOptions(), $"{summary?.MostPopularColCount} times", regularFont,
                                    Color.White,
                                    new PointF(464, 812));
                            }
                        });
                }
                else
                {
                    var regularFont = boldFamily.CreateFont(32, FontStyle.Bold);
                    image.Mutate(
                        i => i.DrawText(new DrawingOptions {TextOptions = new TextOptions {WrapTextWidth = 350}},
                            "The year review generation went wrong.  Did you conquer Cotacols, this year?", regularFont,
                            Color.Black,
                            new PointF(84, 520))
                    );
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