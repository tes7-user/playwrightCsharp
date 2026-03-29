using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using playwrightCSharp.models.POM;
using playwrightCSharp.utilities;
using System.Text.RegularExpressions;

namespace playwrightCSharp.tests.UI;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SignUpForm : PageTest
{
    private SignUpFormComponent _signUpFormComponent;
    private ErrorValidator _errorValidator;

    [SetUp]
    public async Task GotoSignUpPage()
    {
        await Page.GotoAsync($"{ExtractSecret.BaseUrl}/signup");
        _signUpFormComponent = new SignUpFormComponent(Page);
        _errorValidator = new ErrorValidator(Page);
    }

    [Test]
    public async Task TriggerValidationError()
    {
        await _signUpFormComponent.InteractWithField("[type=\"submit\"]", FieldType.Button);

        await _errorValidator.ErrorValidate("Your password should be between 8 and 20 characters");
        await _errorValidator.ErrorValidate("Please provide a first name.");
        await _errorValidator.ErrorValidate("Please provide a last name.");
        await _errorValidator.ErrorValidate("Please provide an email address.");
        await _errorValidator.ErrorValidate("Please enter in a valid email address.");
        await _errorValidator.ErrorValidate("Please provide a password.");
    }
    [Test]
    public async Task SignUpWithANewUser()
    {
        var firstName = "firstNameTest";
        var lastName = "lastNameTest";
        await _signUpFormComponent.InteractWithField("[id=\"firstName\"]", FieldType.TextEntry, firstName);
        await _signUpFormComponent.InteractWithField("[id=\"lastName\"]", FieldType.TextEntry, lastName);
        await _signUpFormComponent.InteractWithField("[id=\"emailAddress\"]", FieldType.TextEntry, $"emailAddressTEst{ReUsableFunctions.RandomString(3)}@Test.com");
        await _signUpFormComponent.InteractWithField("[id=\"password\"]", FieldType.TextEntry, "password2122");
        await _signUpFormComponent.InteractWithField("[type=\"submit\"]", FieldType.Button);
        await Expect(Page).ToHaveURLAsync(new Regex("/courses"));
        await Expect(Page.Locator("ul.header--signedin li").First).ToContainTextAsync($"Welcome, {firstName} {lastName}!");
    }
}

