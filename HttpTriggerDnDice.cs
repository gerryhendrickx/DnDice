using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HendrickxConsulting.DnDice
{
    public static class HttpTriggerDnDice
    {
        [FunctionName("HttpTriggerDnDice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string roll = req.Query["roll"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            roll = roll ?? data?.roll;

            string result = rollTheDice(roll);

            string responseMessage = $"{roll}: {result} ";

            return new OkObjectResult(responseMessage);
        }

        public static string rollTheDice(string roll)
        {
            char[] delimiterChars = { 'd', 'D', ' ', '+'};
            string[] words = roll.Split(delimiterChars);

            Random ran = new Random();
            int num = ran.Next(1, Convert.ToInt32(words[1]));

            return num.ToString();
        }
    }
}
