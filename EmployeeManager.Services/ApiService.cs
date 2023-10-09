using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EmployeeManager.Models;
using EmployeeManager.Helpers;

namespace EmployeeManager.Services
{
    public class ApiService : IApiService
    {
        #region Fields
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://gorest.co.in/public/v2";
        private const string Token = "0bf7fb56e6a27cbcadc402fc2fce8e3aa9ac2b40d4190698eb4e8df9284e2023";
        // TODO: BaseUrl and Token will be moved to a config file and will not be used hard-coded
        #endregion

        #region Constructor
        public ApiService()
        {
            _httpClient = new HttpClient
            {
                DefaultRequestHeaders =
                {
                    {"Authorization", $"Bearer {Token}"}
                }
            };
        }
        #endregion

        #region Public Methods
        public async Task<Result<List<Employee>>> GetEmployees()
        {
            return await SendGetRequest<List<Employee>>($"{BaseUrl}/users");
        }

        public async Task<Result<List<Employee>>> SearchUsersByName(string name)
        {
            return await SendGetRequest<List<Employee>>($"{BaseUrl}/users?name={name}");
        }

        public async Task<Result<Employee>> CreateUser(Employee newUser)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(newUser);
                var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{BaseUrl}/users", content);

                return response.IsSuccessStatusCode
                    ? new Result<Employee> { Data = JsonConvert.DeserializeObject<Employee>(await response.Content.ReadAsStringAsync()) }
                    : throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new Result<Employee> { ErrorMessage = ErrorHandler.HandleException(ex) };
            }
        }

        public async Task<Result<bool>> DeleteUser(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/users/{id}");
                return response.IsSuccessStatusCode
                    ? new Result<bool> { Data = true }
                    : throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new Result<bool> { ErrorMessage = ErrorHandler.HandleException(ex) };
            }
        }
        #endregion

        #region Private Methods
        private async Task<Result<T>> SendGetRequest<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(content);
                    return new Result<T> { Data = data };
                }

                throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new Result<T> { ErrorMessage = ErrorHandler.HandleException(ex) };
            }
        }
        #endregion
    }
}
