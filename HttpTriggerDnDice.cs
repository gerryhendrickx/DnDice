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
            log.LogInformation("DnDice function processed a request.");

            string roll = req.Query["roll"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            roll = roll ?? data?.roll;

            string result = rollTheDice(roll);


            return new OkObjectResult(result);
        }

        public static string rollTheDice(string roll)
        {
            Random ran = new Random();
            string result = $"Roll: {roll} =" ;
            int total = 0;

            //First split in big parts: ex. 1D20+2D6+10 to [1D20, 2D6, 10]
            string[] roleParts = roll.Split('+');

            foreach (string part in roleParts)
            {
                //Then split nb of dice from type of dice.
                char[] delimiters = { 'd', 'D'};
                string[] nbAndType = part.Split(delimiters);
                int nb = Convert.ToInt32(nbAndType[0].Trim());

                if(nbAndType.Length > 1){
                    int type = Convert.ToInt32(nbAndType[1].Trim());

                    for(int i = 0; i < nb; i++){
                        int value = ran.Next(1, type);
                        result += " " + value;
                        total += value;
                    }
                }
                else { //only number, not a dice roll
                    result += " " + nb;
                    total += nb;
                } 

            }

            return result + " = " + total;
        }
    }
}
