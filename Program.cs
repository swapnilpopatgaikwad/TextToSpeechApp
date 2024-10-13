using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace TextToSpeechApp
{
    public class Program
    {
        static string boyVoice = "Microsoft David Desktop";
        static string girlVoice = "Microsoft Zira Desktop";

        static List<Tuple<string, string>> GetConversationList()
        {
            return Dialogues.ConversationList;
        }

        static async Task Main(string[] args)
        {
            string audioPath = @"D:\SwapWork\Text-to-Speech\Output\";

            // Get the title from the first conversation
            List<Tuple<string, string>> conversations = GetConversationList();
            var title = RemoveSpacesAndSpecialSymbols(conversations.FirstOrDefault().Item2);

            string folderPath = Path.Combine(audioPath, title);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string mp3FilePath = Path.Combine(folderPath, title + ".mp3");

            GenerateSpeechToMp3(mp3FilePath, conversations);

            Console.WriteLine("Audio created successfully.");
            Console.ReadKey();
        }

        public static string RemoveSpacesAndSpecialSymbols(string input)
        {
            // Regex to remove anything that is not a letter or number
            return Regex.Replace(input, @"[^a-zA-Z0-9]", "");
        }

        static void GenerateSpeechToMp3(string filePath, List<Tuple<string, string>> conversations)
        {
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                string tempWavPath = Path.GetTempFileName() + ".wav";
                synthesizer.SetOutputToWaveFile(tempWavPath);

                int totalConversations = conversations.Count;

                for (int i = 0; i < totalConversations; i++)
                {
                    var conversation = conversations[i];
                    synthesizer.SelectVoice(conversation.Item1);
                    synthesizer.Speak(conversation.Item2);

                    // Add a 50ms pause at index 0
                    if (i == 0)
                    {
                        Task.Delay(50).Wait();
                    }

                    Console.Clear();
                    ShowProgressBar(i + 1, totalConversations);
                }

                ConvertWavToMp3(tempWavPath, filePath);
            }
        }

        static void ConvertWavToMp3(string inputWavPath, string outputMp3Path)
        {
            FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official).Wait();

            var conversion = FFmpeg.Conversions.New();

            conversion.AddParameter($"-i {inputWavPath} -ac 1 -ab 64k -ar 22050 {outputMp3Path}");
            conversion.Start().Wait();
        }

        static void ShowProgressBar(int current, int total)
        {
            int progress = (int)((double)current / total * 100);
            string bar = new string('#', progress / 2) + new string('-', 50 - progress / 2);
            Console.WriteLine($"[{bar}] {progress}% - {current}/{total} conversations processed");
        }
    }
}




//static async Task CreateVideoWithImageAndAudio(string imagePath, string audioPath, string videoPath)
//{
//    // Download and configure FFmpeg if not already available
//    await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);

//    // Create the video conversion process
//    var videoConversion = FFmpeg.Conversions.New();

//    // Add parameters for image (video input) and audio
//    videoConversion.AddParameter($"-loop 1 -i {imagePath} -i {audioPath} -c:v libx264 -tune stillimage -c:a aac -b:a 192k -pix_fmt yuv420p -shortest");

//    // Set the output video file
//    videoConversion.SetOutput(videoPath);

//    // Hook to track progress of the video conversion
//    videoConversion.OnProgress += (sender, e) =>
//    {
//        Console.Clear();
//        ShowProgressBar(e.Duration.Seconds, (int)e.TotalLength.TotalSeconds);
//    };

//    // Start the conversion process
//    await videoConversion.Start();
//}

