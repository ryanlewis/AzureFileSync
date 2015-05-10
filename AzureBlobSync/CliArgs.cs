using PowerArgs;

namespace AzureBlobSync
{
    public class CliArgs
    {
        [ArgRequired(PromptIfMissing = true)]
        public string ConnString { get; set; }

        [ArgRequired(PromptIfMissing = true)]
        public string Container { get; set; }

        [ArgRequired(PromptIfMissing = true)]
        [ArgDefaultValue(".")]
        public string Directory { get; set; }

        public bool DevMode { get; set; }

        public bool Verbose { get; set; }
    }
}