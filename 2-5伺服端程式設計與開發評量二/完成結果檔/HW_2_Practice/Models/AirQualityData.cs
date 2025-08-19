using System.Text.Json.Serialization;

namespace AirQualityAPI.Models
{
    /// <summary>
    /// 空氣品質資料模型 - 對應第三方API回傳的JSON結構
    /// </summary>
    public class AirQualityData
    {
        // JsonPropertyName 屬性：將 C# 屬性名稱對應到 JSON 欄位名稱
        [JsonPropertyName("sitename")]
        public string? SiteName { get; set; }    // 監測站名稱

        [JsonPropertyName("county")]
        public string? County { get; set; }      // 縣市

        [JsonPropertyName("aqi")]
        public string? AqiString { get; set; }   // AQI 字串值

        // 計算屬性：將字串轉換為數值，方便後續處理
        public int Aqi => int.TryParse(AqiString, out var result) ? result : 0;

        [JsonPropertyName("pollutant")]
        public string? Pollutant { get; set; }   // 主要污染物

        [JsonPropertyName("status")]
        public string? Status { get; set; }      // 空氣品質狀態

        // 各種污染物濃度
        [JsonPropertyName("so2")]
        public string? SO2 { get; set; }

        [JsonPropertyName("co")]
        public string? CO { get; set; }

        [JsonPropertyName("pm25")]
        public string? PM25 { get; set; }

        [JsonPropertyName("publishtime")]
        public string? PublishTime { get; set; }  // 發布時間

        [JsonPropertyName("longitude")]
        public string? Longitude { get; set; }    // 經度

        [JsonPropertyName("latitude")]
        public string? Latitude { get; set; }     // 緯度
    }
}