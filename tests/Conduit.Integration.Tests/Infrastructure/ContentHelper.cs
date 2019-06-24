namespace Conduit.Integration.Tests.Infrastructure
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Users.Commands.LoginUser;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Newtonsoft.Json;

    public static class ContentHelper
    {
        public static StringContent GetRequestContent(object request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }

        public static async Task<StringContent> GetRequestContentWithAuthorization(object request, HttpClient client)
        {
            var seedUserLoginRequest = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "joey.mckenzie@gmail.com",
                    Password = "#password1!"
                }
            };

            var response = await client.PostAsync("/api/users/login", GetRequestContent(seedUserLoginRequest));
            var responseContent = await GetResponseContent<UserViewModel>(response);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseContent.User.Token);
            return GetRequestContent(request);
        }

        public static async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(stringResponse);
            return result;
        }
    }
}