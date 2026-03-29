using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
using playwrightCSharp.models.POM;
using playwrightCSharp.utilities;

namespace playwrightCSharp.tests.UI;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class LoginTest : PageTest
{
    private SignInComponent _signIn;
    private ErrorValidator _errorValidator;

    [SetUp]
    public async Task GotoLoginPage()
    {
        _signIn = new SignInComponent(Page);
        await _signIn.GotoSignInPage();
        _errorValidator = new ErrorValidator(Page);
    }

    [Test]
    public async Task LoginWithValidCredentials()
    {
        await _signIn.EnterLoginDetail(ExtractSecret.TestEmail, ExtractSecret.TestPassword);
        await Expect(Page).ToHaveURLAsync(new Regex("/courses"));
    }

    [Test]
    public async Task NoLoginCredentials()
    {
        await _signIn.ClickOnSignInButton();
        await _errorValidator.ErrorValidate("Please check your email address and password and try again.");
    }

    [Test]
    public async Task RedirectingToCoursesWhenCancelButtonIsClicked()
    {
        await Page.GetByText("Cancel").ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("/courses"));
    }

    [Test]
    public async Task InvalidLoginCredentials()
    {
        await _signIn.EnterLoginDetail("invalidUser@gmail.com", "NotARealPassword");
        await _errorValidator.ErrorValidate("Please check your email address and password and try again.");
    }
}