using Microsoft.Playwright.NUnit;
using playwrightCSharp.models.api;
using playwrightCSharp.utilities;

namespace playwrightCSharp.tests.API
{
    public class CreateACourse : PlaywrightTest
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
            _loginCredentials.username = ExtractSecret.TestEmail;
            _loginCredentials.password = ExtractSecret.TestPassword;
        }

        [TearDown]
        public async Task TearDown()
        {
            await _reusableFunction.DeleteRequest($"{_apiURL}/courses", _loginCredentials);
            var coursesResponse = await _reusableFunction.GetRequest($"{_apiURL}/courses", _loginCredentials);
            var courses = await coursesResponse.JsonAsync();

            var createdCourse = courses.Value.EnumerateArray()
                .FirstOrDefault(course => course.GetProperty("title").GetString() == "Test Course");

            if (createdCourse.ValueKind != System.Text.Json.JsonValueKind.Undefined)
            {
                var courseId = createdCourse.GetProperty("id").GetInt32();
                var deleteResponse = await _reusableFunction.DeleteRequest($"{_apiURL}/courses/{courseId}", _loginCredentials);
                Assert.That(deleteResponse.Status, Is.EqualTo(204));
            }
            await _reusableFunction.Dispose();
        }

        [Test]
        public async Task CreateCourse()
        {


            var getResponse = await _reusableFunction.GetRequest($"{_apiURL}/users", _loginCredentials);
            var userJson = await getResponse.JsonAsync();
            var userId = userJson?.GetProperty("id").GetInt32();

            Assert.That(getResponse.Status, Is.EqualTo(200));

            Course newCourse = new Course
            {
                Title = "Test Course",
                Description = "This is a test course created via API",
                EstimatedTime = "5 hours",
                MaterialsNeeded = "Computer, Internet",
                UserId = userId.Value
            };
            var postCourse = await _reusableFunction.PostRequest($"{_apiURL}/courses", newCourse, _loginCredentials);
            Assert.That(postCourse.Status, Is.EqualTo(201));

            var getCourseDetails = await _reusableFunction.GetRequest($"{_apiURL}/courses", _loginCredentials);
            var courses = await getCourseDetails.JsonAsync();

            var createdCourse = courses.Value.EnumerateArray()
                .FirstOrDefault(course => course.GetProperty("title").GetString() == "Test Course");

            Assert.That(createdCourse.GetProperty("title").GetString(), Is.EqualTo(newCourse.Title));
            Assert.That(createdCourse.GetProperty("description").GetString(), Is.EqualTo(newCourse.Description));
            Assert.That(createdCourse.GetProperty("estimatedTime").GetString(), Is.EqualTo(newCourse.EstimatedTime));
            Assert.That(createdCourse.GetProperty("materialsNeeded").GetString(), Is.EqualTo(newCourse.MaterialsNeeded));

        }
    }
}
