using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using playwrightCSharp.models.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playwrightCSharp.utilities
{
    public class ReUsableFunctions : PlaywrightTest
    {
        private IAPIRequestContext? _apiContext;

        public async Task Initialize(IPlaywright playwright)
        {
            _apiContext = await playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = ExtractSecret.BaseApiUrl
            });
        }

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[Random.Shared.Next(s.Length)])
                          .ToArray()
            );
        }

        public async Task<IAPIResponse> PostRequest(string url, object data, object auth)
        {
            string username, password;

            if (auth is Dictionary<string, string> authDict)
            {
                username = authDict["username"];
                password = authDict["password"];
            }
            else if (auth is LoginCredentials loginCreds)
            {
                username = loginCreds.username;
                password = loginCreds.password;
            }
            else
            {
                throw new ArgumentException("Auth must be Dictionary<string, string> or LoginCredentials");
            }

            var credentials = $"{username}:{password}";
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            var response = await _apiContext.PostAsync(url, new()
            {
                DataObject = data,
                Headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Basic {encoded}"
                }
            });

            return response;
        }

        public async Task Dispose()
        {
            if (_apiContext != null)
            {
                await _apiContext.DisposeAsync();
            }
        }

        public async Task<IAPIResponse> GetRequest(string url, object auth)
        {
            string username, password;

            if (auth is Dictionary<string, string> authDict)
            {
                username = authDict["username"];
                password = authDict["password"];
            }
            else if (auth is LoginCredentials loginCreds)
            {
                username = loginCreds.username;
                password = loginCreds.password;
            }
            else
            {
                throw new ArgumentException("Auth must be Dictionary<string, string> or LoginCredentials");
            }

            var credentials = $"{username}:{password}";
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            var response = await _apiContext.GetAsync(url, new()
            {
                Headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Basic {encoded}"
                }
            });

            return response;
        }
        public async Task<IAPIResponse> DeleteRequest(string url, object auth)
        {
            string username, password;

            if (auth is Dictionary<string, string> authDict)
            {
                username = authDict["username"];
                password = authDict["password"];
            }
            else if (auth is LoginCredentials loginCreds)
            {
                username = loginCreds.username;
                password = loginCreds.password;
            }
            else
            {
                throw new ArgumentException("Auth must be Dictionary<string, string> or LoginCredentials");
            }

            var credentials = $"{username}:{password}";
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            var response = await _apiContext.DeleteAsync(url, new()
            {
                Headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Basic {encoded}"
                }
            });

            return response;
        }
    }
}