using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using System.Text.RegularExpressions;
using playwrightCSharp.utilities;

namespace playwrightCSharp.models.POM
{
    public class SignInComponent
    {
        private readonly IPage _page;
        private readonly ILocator _emailInputField;
        private readonly ILocator _passwordInputField;
        private readonly ILocator _loginButton;
        public SignInComponent(IPage page)
        {
            _page = page;
            _emailInputField = page.GetByLabel("Email Address");
            _passwordInputField = page.GetByLabel("Password");
            _loginButton = page.GetByRole(AriaRole.Button, new() { Name = "Sign In" });
        }

        public async Task GotoSignInPage()
        {
            await _page.GotoAsync($"{ExtractSecret.BaseUrl}/signin");
            await Expect(_page).ToHaveURLAsync(new Regex("/signin"));
        }

        public async Task EnterLoginDetail(string email, string password)
        {
            await _emailInputField.FillAsync(email);
            await _passwordInputField.FillAsync(password);
            await ClickOnSignInButton();
        }
        public async Task ClickOnSignInButton()
        {
            await _loginButton.ClickAsync();
        }
    }
}
