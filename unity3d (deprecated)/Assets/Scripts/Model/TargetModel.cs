using Newtonsoft.Json;
using System;

namespace Assets
{
    public class TargetModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("audio_type")]
        public string AudioType { get; set; }

        [JsonProperty("audio_base64")]
        public string AudioBase64 { get; set; }

        [JsonProperty("jpeg_base64")]
        public string JpegBase64 { get; set; }

        [JsonProperty("png_base64")]
        public string PngBase64 { get; set; }

        [JsonProperty("vuforia_id")]
        public string VuforiaId { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("is_trackable")]
        public bool IsTrackable { get; set; }

        [JsonProperty("hex_color")]
        public string HexColor { get; set; }

        [JsonProperty("rate")]
        public int? Rate { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("updated")]
        public DateTime Updated { get; set; }
    }
}