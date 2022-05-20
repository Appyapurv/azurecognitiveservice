using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace azurecognitiveservice
{
    public class ImageAnalysis
    {
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";

        public  void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision");
            Console.WriteLine();
            ComputerVisionClient client = Authenticate(Utils.endpoint, Utils.subscriptionKey);

            AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();
            Console.WriteLine("enter something to stop");
            var read = Console.ReadLine();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            var results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
        }
    }
}