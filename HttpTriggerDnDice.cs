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
            //First split in big parts: ex. 1D20+2D6+10 to [1D20, 2D6, 10]
            char[] delimiterChars = { '+'};
            string[] roleParts = roll.Split(delimiterChars);

            foreach (string part in roleParts)
            {
                //Then split nb of dice from type of dice.
                
            }



            delimiterChars = { 'd', 'D'};

            Random ran = new Random();
            int num = ran.Next(1, Convert.ToInt32(words[1]));

            return num.ToString();
        }
    }
}
