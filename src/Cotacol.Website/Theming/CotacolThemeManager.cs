using AeroBlazor.Theming;
using MudBlazor;
using MudBlazor.Utilities;

namespace Cotacol.Website.Theming;

public class CotacolThemeManager : IThemeManager
{
    public string PageTitle { get; set; } = "Rental manager";
    
    
    public MudTheme CurrentTheme => new MudTheme
    {
        PaletteLight = new PaletteLight()
        {
            Primary = new MudColor("#100C08"),
            Secondary = new MudColor("#e35654"), //FD7D7A
            Tertiary = new MudColor("#999999"),
            Info = new MudColor("#FC4C02"),
            Background = new MudColor("F9F1E6"),
            Success = new MudColor("#078727"),
            Warning = new MudColor("#FC4C02")
        },
        Typography = DefaultTypography
    };


    public static Typography DefaultTypography => new Typography()
    {
        H1 = new H1() {FontSize = "1.5rem", FontFamily = new[] {"Merkury", "sans-serif"}, LineHeight = 1.2, TextTransform = "uppercase", FontWeight = 1200, LetterSpacing = "1px",},
        H2 = new H2() {FontSize = "1.25rem", FontFamily = new[] {"Cooper", "serif"}, LineHeight = 1.2, FontWeight = 900, LetterSpacing = "1px",},
        Body1 = new Body1() {FontSize = "0.8rem", FontFamily = new[] {"Lekton", "sans-serif"}, LineHeight = 2, FontWeight = 400},
        Subtitle1 = new Subtitle1() {FontSize = "1.1rem", FontFamily = new[] {"Merkury", "serif"}, LineHeight = 2, FontWeight = 800, LetterSpacing = "1px"},
        Default = new Default() {FontSize = "0.8rem", FontFamily = new[] {"Lekton", "sans-serif"}, LineHeight = 2, FontWeight = 400}
    
    };

}