using Microsoft.Playwright.NUnit;
using playwrightCSharp.models.api;
using playwrightCSharp.utilities;
using System.Text.Json;


namespace playwrightCSharp.tests.API
{
    public class Login : PlaywrightTest
    {
        private ReUsableFunctions _reusableFunction;
        private string _apiURL = ExtractSecret.BaseApiUrl;
        private LoginCredentials _loginCredentials;

        [SetUp]
        public async Task Setup()
        {
            _reusableFunction = new ReUsableFunctions();
            await _reusableFunction.Initialize(Playwright);
            _loginCredentials = new LoginCredentials();
        }

        [Test]
        public async Task LoginValidTest()
        {
            _loginCredentials.username = ExtractSecret.TestEmail;
            _loginCredentials.password = ExtractSecret.TestPassword;
            var sendLoginRequest = await _reusableFunction.GetRequest($"{_apiURL}/users", _loginCredentials);
            Assert.That(sendLoginRequest.Status, Is.EqualTo(200));

            var response = await sendLoginRequest.JsonAsync<JsonElement>();

            Assert.That(response.GetProperty("firstName").GetString(), Is.EqualTo("Joe"));
            Assert.That(response.GetProperty("lastName").GetString(), Is.EqualTo("Smith"));
            Assert.That(response.GetProperty("emailAddress").GetString(), Is.EqualTo("joe@smith.com"));
            Assert.That(response.GetProperty("id").GetInt32(), Is.EqualTo(1));
        }

        [Test]
        [TestCase("InvalidEmail", "InvalidPassword")]
        [TestCase("joe@smith.com", "")]
        [TestCase("", "InvalidPassword")]
        public async Task InvalidLoginTest(string username, string password)
        {
            _loginCredentials.username = username;
            _loginCredentials.password = password;
            var sendLoginRequest = await _reusableFunction.GetRequest($"{_apiURL}/users", _loginCredentials);
            Assert.That(sendLoginRequest.Status, Is.EqualTo(401));
            var response = await sendLoginRequest.JsonAsync<JsonElement>();

            Assert.That(response.GetProperty("message").GetString(), Is.EqualTo("Access Denied"));
        }
    }
}
