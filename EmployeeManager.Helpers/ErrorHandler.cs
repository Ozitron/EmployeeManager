using System.Net.Http;
using System;

namespace EmployeeManager.Helpers
{
    public static class ErrorHandler
    {
        public static string HandleException(Exception ex)
        {
            return $"An unexpected error occurred: {ex.Message}";
        }

        // TODO: Will be implemented
        public static void HandleHttpResponseError(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    throw new Exception("Bad request. Please check the request and try again.");

                case System.Net.HttpStatusCode.Unauthorized:
                case System.Net.HttpStatusCode.Forbidden:
                    throw new Exception("You don't have the necessary permissions for the resource.");

                case System.Net.HttpStatusCode.NotFound:
                    throw new Exception("The requested resource was not found.");

                case System.Net.HttpStatusCode.UnprocessableEntity:
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Failed to process the request. Errors: {errorContent}");

                default:
                    throw new Exception("An error occurred while contacting the server.");
            }
        }
    }
}
