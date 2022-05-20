using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace azurecognitiveservice
{
    class ContinuesSpeechToText
    {
        public async Task Main(string[] args)
        {
            var speechConfig = SpeechConfig.FromSubscription(Utils.subscriptionKey, Utils.YourServiceRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            using var audioConfig = AudioConfig.FromWavFileInput(@"C:\Users\apurv\Downloads\Music/12.wav");
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
            var stopRecognition = new TaskCompletionSource<int>();

            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
            };

            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
            };

            recognizer.Canceled += (s, e) =>
            {
                Console.WriteLine($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }

                stopRecognition.TrySetResult(0);
            };

            recognizer.SessionStopped += (s, e) =>
            {
                Console.WriteLine("\n    Session stopped event.");
                stopRecognition.TrySetResult(0);
            };

        }
    }
}

//   string path1 = "/Users/apurvupadhyay/Downloads/azure-cognitive-services-sample-main2/Imagens/t3qWG.png";
//             var path = Path.Combine(Directory.GetCurrentDirectory(), "\\Imagens\\abc.txt");

//             // Read the file as one string. 
//             string text = System.IO.File.ReadAllText(path1);
//             var myfile = System.IO.File.OpenRead(@"/Users/apurvupadhyay/Downloads/azure-cognitive-services-sample-main2/Imagens/1.jpeg");