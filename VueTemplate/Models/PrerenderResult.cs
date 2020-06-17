using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueTemplate.Models
{
    public class SpaPrerenderRequest
    {
        public string Domain { get; set; }
        public string Path { get; set; }

        public string CacheIdentifier()
        {
            return $"{Domain}:{Path}";
        }
    }
    public class PrerenderResult
    {
        public string Styles { get; set; }
        public string Scripts { get; set; }
        public string ResourceHints { get; set; }
        public PreloadFile[] PreloadFiles { get; set; }
        public string Content { get; set; }
        public string State { get; set; }
        public bool IsNotFound { get; set; }
    }

    public class PreloadFile
    {
        public string File { get; set; }
        public string FileWithoutQuery { get; set; }
        public string AsType { get; set; }
        public string Extension { get; set; }
    }
}
