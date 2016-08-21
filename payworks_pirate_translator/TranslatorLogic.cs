using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace payworks_pirate_translator
{
    public class TranslatorLogic
    {
        //TODO: sanitize input https://xkcd.com/327/
        private static string _inputToTranslate = null;
        private static string _outputOfTranslation = null;
        public static string RunTranslation(string s)
        {
            _inputToTranslate = s;
            Task.Run(MainAsync).Wait(); //Wait, and await in the task ensures avoidance of deadlock
            if (_outputOfTranslation != null)
            {
                return _outputOfTranslation;
            }

            else
            {
                throw new NullReferenceException("Output was null, translation unsuccessful!");
            }
        }

        //The main thread is responsible for the GUI, therefore it's not an optimal solution to run tasks on the main thread which could require time to complete
        //Hence the idea: make the tasks responsible for the network comms run on a different thread.
        //The way I implemented the comms ensures one request to be sent at a time excluding use-cases where I would have otherwise needed the implementation of a queue.
        private static async Task MainAsync()
        {   
            //Ensuring valid URI. Without encoding if the input is eg. &format=xml I won't get the desired response
            Uri whereIsMyBooty =
                new Uri(
                    $"http://isithackday.com/arrpi.php?text={WebUtility.UrlEncode(_inputToTranslate)}&format=json"); 

            await HttpComms(whereIsMyBooty);
        }

        //follow the law of Demeter
        private static async Task HttpComms(Uri accessUri)
        {
            string responseString;
            //using try-catch so I can handle exceptions upon getting an unsuccessful response.
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(accessUri);
                    response.EnsureSuccessStatusCode(); // Making sure the communication was successful

                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                throw new UnsuccessfulTranslationException("Translation was unsuccessful");
            }

            //deserializing the response into the structure defined in TranslatorHelper.cs
            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);

            _outputOfTranslation = apiResponse.Translation.Pirate;

            //used debug purposes in the command line version
            //Console.WriteLine("-----------");
            //Console.WriteLine("X marks the spot:");
            //Console.WriteLine(apiResponse.Translation.Pirate);
        }
    }

    //I figured throwing an exception of my own upon an unsuccessful translation is a more clean approach
    public class UnsuccessfulTranslationException : Exception
    {
        public UnsuccessfulTranslationException(string message) : base(message)
        {
        }
    }
}