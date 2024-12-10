namespace Cotacol.WebTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
        var baseUrl = TestContext.Parameters["BaseUrl"] ?? throw new ArgumentNullException("BaseUrl");
        await Page.GotoAsync(baseUrl);

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Cotacol.cc"));

        // create a locator
        var getStarted = Page.Locator("text=Get Started");
        await Page.ScreenshotAsync(new() { Path = "screenshot1.png" });
        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the get started link.
        await getStarted.ClickAsync();

        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }
}