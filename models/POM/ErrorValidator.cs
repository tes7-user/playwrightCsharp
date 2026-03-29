using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;


namespace playwrightCSharp.models.POM
{
    public class ErrorValidator
    {
        private readonly IPage _page;
        private readonly ILocator _errorMessageLocator;

        public ErrorValidator(IPage page)
        {
            _page = page;
            _errorMessageLocator = _page.Locator(".validation--errors");

        }
        public async Task ErrorValidate(String  message)
        {
            await Expect(_errorMessageLocator).ToContainTextAsync(message);
        }
    }
}
