using AirQualityAPI.Models;
using HW_2_Practice.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AirQualityAPI.Services
{
    /// <summary>
    /// 空氣品質服務實作 - 包含所有商業邏輯
    /// </summary>
    public class AirQualityService : IAirQualityService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AirQualityService> _logger;

        // 第三方 API 網址 - 台灣環保署空氣品質資料
        private const string API_URL = "https://data.epa.gov.tw/api/v2/aqx_p_432?api_key=9be7b239-557b-4c10-9775-78cadfc555e9&limit=1000&sort=ImportDate%20desc&format=json";

        /// <summary>
        /// 建構函式 - 透過依賴注入取得所需服務
        /// </summary>
        public AirQualityService(HttpClient httpClient, ILogger<AirQualityService> logger)
        {
            _httpClient = httpClient;  // 用於 HTTP 呼叫
            _logger = logger;          // 用於記錄日誌
        }

        /// <summary>
        /// 私有方法：從第三方 API 取得原始資料
        /// 功能：資料介接的核心邏輯
        /// </summary>
        private async Task<List<AirQualityData>> FetchAirQualityDataAsync()
        {
            try
            {
                _logger.LogInformation("正在從政府開放資料平台取得空氣品質資料...");

                // 呼叫第三方 API
                var response = await _httpClient.GetAsync(API_URL);
                response.EnsureSuccessStatusCode();  // 確保 HTTP 成功狀態

                // 讀取 JSON 回應
                var jsonContent = await response.Content.ReadAsStringAsync();

                // 反序列化 JSON 為物件
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonContent);

                if (apiResponse?.Records == null)
                {
                    _logger.LogWarning("API 回應中沒有 records 資料");
                    return new List<AirQualityData>();
                }

                _logger.LogInformation($"成功取得 {apiResponse.Records.Count} 筆空氣品質資料");
                return apiResponse.Records;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "呼叫第三方 API 時發生網路錯誤");
                throw new ApplicationException("無法連接到資料來源", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "解析 JSON 資料時發生錯誤");
                throw new ApplicationException("資料格式錯誤", ex);
            }
        }

        /// <summary>
        /// 取得所有空氣品質資料
        /// 商業邏輯：過濾掉沒有站名的資料
        /// </summary>
        public async Task<IEnumerable<AirQualityData>> GetAllAirQualityDataAsync()
        {
            var data = await FetchAirQualityDataAsync();
            return data.Where(x => !string.IsNullOrEmpty(x.SiteName));
        }

        /// <summary>
        /// 根據縣市查詢空氣品質資料
        /// 商業邏輯：不區分大小寫的模糊搜尋
        /// </summary>
        public async Task<IEnumerable<AirQualityData>> GetAirQualityByCountyAsync(string county)
        {
            var data = await FetchAirQualityDataAsync();
            return data.Where(x => !string.IsNullOrEmpty(x.County) &&
                                  x.County.Contains(county, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 根據 AQI 範圍篩選資料
        /// 商業邏輯：範圍篩選並按 AQI 降冪排序
        /// </summary>
        public async Task<IEnumerable<AirQualityData>> GetAirQualityByAqiRangeAsync(int minAqi, int maxAqi)
        {
            var data = await FetchAirQualityDataAsync();
            return data.Where(x => x.Aqi >= minAqi && x.Aqi <= maxAqi && x.Aqi > 0)
                      .OrderByDescending(x => x.Aqi);
        }

        /// <summary>
        /// 根據站點名稱查詢特定資料
        /// 商業邏輯：模糊搜尋並回傳第一筆符合的資料
        /// </summary>
        public async Task<AirQualityData?> GetAirQualityBySiteAsync(string siteName)
        {
            var data = await FetchAirQualityDataAsync();
            return data.FirstOrDefault(x => !string.IsNullOrEmpty(x.SiteName) &&
                                           x.SiteName.Contains(siteName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 取得空氣品質統計摘要
        /// 商業邏輯：複雜的資料分析和統計計算
        /// </summary>
        public async Task<AirQualitySummary> GetAirQualitySummaryAsync()
        {
            var data = await FetchAirQualityDataAsync();
            var validData = data.Where(x => x.Aqi > 0).ToList();

            if (!validData.Any())
            {
                return new AirQualitySummary { LastUpdateTime = DateTime.Now };
            }

            var summary = new AirQualitySummary
            {
                TotalStations = validData.Count,

                // 群組統計：各縣市站數分布
                CountyDistribution = validData
                    .Where(x => !string.IsNullOrEmpty(x.County))
                    .GroupBy(x => x.County!)
                    .ToDictionary(g => g.Key, g => g.Count()),

                // 群組統計：各狀態分布
                StatusDistribution = validData
                    .Where(x => !string.IsNullOrEmpty(x.Status))
                    .GroupBy(x => x.Status!)
                    .ToDictionary(g => g.Key, g => g.Count()),

                // 數值統計
                AverageAqi = Math.Round(validData.Average(x => x.Aqi), 2),
                MaxAqi = validData.Max(x => x.Aqi),
                MinAqi = validData.Min(x => x.Aqi),

                // 極值查詢
                WorstStation = validData.OrderByDescending(x => x.Aqi).First().SiteName,
                BestStation = validData.OrderBy(x => x.Aqi).First().SiteName,
                LastUpdateTime = DateTime.Now
            };

            return summary;
        }

        /// <summary>
        /// 私有內部類別：API 回應的包裝結構
        /// </summary>
        private class ApiResponse
        {
            [JsonPropertyName("records")]
            public List<AirQualityData> Records { get; set; } = new();
        }
    }
}