using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

namespace LuisWithSpeech
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //https://docs.microsoft.com/pt-br/azure/cognitive-services/speech-service/how-to-recognize-intents-from-speech-csharp
            var config = SpeechConfig.FromSubscription("a3557de3a21742f797871ef49da4ab5a", "westus");
            config.SpeechRecognitionLanguage = "pt-BR";
            
            using (var recognizer = new IntentRecognizer(config))
            {
                var model = LanguageUnderstandingModel.FromAppId("6b8a09b9-0f41-4ac5-b273-a02dd25fcf7e");
                recognizer.AddIntent(model, "HomeAutomation.TurnOff", "off");
                recognizer.AddIntent(model, "HomeAutomation.TurnOn", "on");

                Console.WriteLine("Diga algo...");
                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                if (result.Reason == ResultReason.RecognizedIntent)
                {
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent Id: {result.IntentId}.");
                    Console.WriteLine($"    Language Understanding JSON: {result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult)}.");
                }
                else if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent not recognized.");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }

                Console.ReadLine();
            }

        }
    }
}
