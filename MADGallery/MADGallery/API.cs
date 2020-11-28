using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MADGallery
{

    public class Result
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public Photo[] photos { get; set; }
        public int total_results { get; set; }
        public string next_page { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public string photographer { get; set; }
        public string photographer_url { get; set; }
        public int photographer_id { get; set; }
        public Src src { get; set; }
        public bool liked { get; set; }
    }

    public class Src
    {
        public string original { get; set; }
        public string large2x { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
        public string small { get; set; }
        public string portrait { get; set; }
        public string landscape { get; set; }
        public string tiny { get; set; }

    }

    public class SrcBool
    {
        public bool original { get; set; }
        public bool large2x { get; set; }
        public bool large { get; set; }
        public bool medium { get; set; }
        public bool small { get; set; }
        public bool portrait { get; set; }
        public bool landscape { get; set; }
        public bool tiny { get; set; }
    }

}
