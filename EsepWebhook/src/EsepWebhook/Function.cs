using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EsepWebhook
{
    public class Function
    {
        /// <summary>
        /// A simple function that processes the GitHub webhook and sends a message to Slack.
        /// </summary>
        /// <param name="input">The GitHub webhook payload.</param>
        /// <param name="context">Lambda execution context for logging.</param>
        /// <returns>The response returned by Slack API (or error message).</returns>
        public string FunctionHandler(object input, ILambdaContext context)
        {
            // Log the incoming input
            context.Logger.LogInformation($"FunctionHandler received: {input}");

            try
            {
                // Deserialize the input to get the issue details
                dynamic json = JsonConvert.DeserializeObject<dynamic>(input.ToString());
                
                // Extract the issue URL and create a payload for Slack
                string issueUrl = json?.issue?.html_url;
                if (string.IsNullOrEmpty(issueUrl))
                {
                    throw new Exception("Issue URL is missing in the payload.");
                }

                string payload = $"{{'text':'Issue Created: {issueUrl}'}}";

                // Send the message to Slack via the URL from the environment variable
                var client = new HttpClient();
                var webRequest = new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("SLACK_URL"))
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(webRequest).Result;  // .Result to wait for the response synchronously
                response.EnsureSuccessStatusCode();

                // Read and return the response from Slack API
                using var reader = new StreamReader(response.Content.ReadAsStream());
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                // Log and return the error message if something goes wrong
                context.Logger.LogError($"Error occurred: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}

