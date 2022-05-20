using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

namespace azurecognitiveservice
{
    public class ContinuesTranslate
    {
        public async Task TranslationContinuousRecognitionAsync()
        {
            var config = SpeechTranslationConfig.FromSubscription(Utils.subscriptionKey, Utils.YourServiceRegion);
            string fromLanguage = "en-US";
            config.SpeechRecognitionLanguage = fromLanguage;
            //config.AddTargetLanguage("en-IN");
            config.AddTargetLanguage("es-ES");


            const string voice = "en-GB-LibbyNeural";
            //en-GB-LibbyNeural
            config.VoiceName = voice;

            using (var recognizer = new TranslationRecognizer(config))
            {

                recognizer.Recognizing += (s, e) =>
                {
                    //Console.WriteLine($"RECOGNIZING in '{fromLanguage}': Text={e.Result.Text}");
                    foreach (var element in e.Result.Translations)
                    {
                        //Console.WriteLine($"    TRANSLATING into '{element.Key}': {element.Value}");
                    }
                };

                recognizer.Recognized += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.TranslatedSpeech)
                    {
                        Console.WriteLine($"\n{e.Result.Reason.ToString()}-- Recognized text in {fromLanguage}:.");
                        Console.WriteLine($"You Spoke:-- {e.Result.Text}");
                        foreach (var element in e.Result.Translations)
                        {
                            Console.WriteLine($"TRANSLATING into '{element.Key}':-- {element.Value}");
                        }
                    }
                };

                recognizer.Synthesizing += (s, e) =>
                {
                    var audio = e.Result.GetAudio();
                    if (audio.Length != 0)
                    {
                        Console.WriteLine($"AudioSize: {audio.Length}");
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    Console.WriteLine($"\nRecognition canceled. Reason: {e.Reason}; ErrorDetails: {e.ErrorDetails}");
                };

                recognizer.SessionStarted += (s, e) =>
                {
                    Console.WriteLine("\nTranslate speech started .");
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    Console.WriteLine("\nTranslate speech stopped.");
                };

                Console.WriteLine("Say something...");
                await recognizer.StartContinuousRecognitionAsync();

                do
                {
                    Console.WriteLine("Press Enter to stop");
                } while (Console.ReadKey().Key != ConsoleKey.Enter);

                await recognizer.StopContinuousRecognitionAsync();
            }
        }

        public async Task Main(string[] args)
        {
            await TranslationContinuousRecognitionAsync();
        }
    }
}