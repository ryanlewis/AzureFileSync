﻿using PowerArgs;

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
        public string Dir { get; set; }

        [ArgHiddenFromUsage]
        public bool DevMode { get; set; }

        public bool Verbose { get; set; }
        public bool DeleteIfExists { get; set; }
    }
}