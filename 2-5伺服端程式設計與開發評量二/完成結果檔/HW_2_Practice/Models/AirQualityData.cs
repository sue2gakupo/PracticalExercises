using System.Text.Json.Serialization;

namespace HW_2_Practice.Models
{

    public class AirQualityData
    {
        
        [JsonPropertyName("sitename")]
        public string? SiteName { get; set; }    

        [JsonPropertyName("county")]
        public string? County { get; set; }      

        [JsonPropertyName("aqi")]
        public string? AqiString { get; set; }   
        // 計算屬性：將字串轉換為數值，方便大小值比較和篩選
        [JsonIgnore] // 在序列化時忽略這個屬性，不會在swagger中顯示
        public int Aqi => int.TryParse(AqiString, out var result) ? result : 0;

        [JsonPropertyName("pollutant")]
        public string? Pollutant { get; set; }   // 主要污染物

        [JsonPropertyName("status")]
        public string? Status { get; set; }      // 空氣品質狀態

        // ↓各種污染物濃度↓
        [JsonPropertyName("so2")]
        public string? SO2 { get; set; }

        [JsonPropertyName("co")]
        public string? CO { get; set; }

        [JsonPropertyName("pm2.5")]
        public string? PM25 { get; set; }

        [JsonPropertyName("publishtime")]
        public string? PublishTime { get; set; }  // 發布時間

        [JsonPropertyName("longitude")]
        public string? Longitude { get; set; }    // 經度

        [JsonPropertyName("latitude")]
        public string? Latitude { get; set; }     // 緯度

        [JsonPropertyName("siteid")]
        public string? SiteId { get; set; } 
    }
}