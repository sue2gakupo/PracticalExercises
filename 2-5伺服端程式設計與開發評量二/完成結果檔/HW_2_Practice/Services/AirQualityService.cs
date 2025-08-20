using HW_2_Practice.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HW_2_Practice.Services
{

    // 空氣品質服務實作 - 包含所有商業邏輯
    public class AirQualityService : IAirQualityService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AirQualityService> _logger;

        // 第三方 API 網址 - 台灣環保署空氣品質資料
        private const string API_URL = "https://data.moenv.gov.tw/api/v2/aqx_p_432?api_key=9e565f9a-84dd-4e79-9097-d403cae1ea75&limit=1000&sort=ImportDate%20desc&format=JSON";

       
        //建構函式 - 透過依賴注入取得所需服務
        public AirQualityService(HttpClient httpClient, ILogger<AirQualityService> logger)
        {
            _httpClient = httpClient;  // 用於 HTTP 呼叫
            _logger = logger;          // 用於記錄日誌//在啟動的terminal視窗看到回傳訊息
        }

       
        // 私有方法：從第三方 API 取得原始資料
        // 功能：資料介接的核心邏輯
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


        // 取得所有空氣品質資料
        // 商業邏輯：過濾掉沒有站名的資料
        public async Task<IEnumerable<AirQualityData>> GetAllAirQualityDataAsync()
        {
            var data = await FetchAirQualityDataAsync();
            return data.Where(x => !string.IsNullOrEmpty(x.SiteName));
        }


        // 根據縣市查詢空氣品質資料
        // 商業邏輯：不區分大小寫的模糊搜尋
        public async Task<IEnumerable<AirQualityData>> GetAirQualityByCountyAsync(string county)
        {
            var data = await FetchAirQualityDataAsync();
            return data.Where(x => !string.IsNullOrEmpty(x.County) &&
                                  x.County.Contains(county, StringComparison.OrdinalIgnoreCase));
        }

     
        // 根據 AQI 範圍篩選資料
        // 商業邏輯：範圍篩選並按 AQI 降冪排序
        public async Task<IEnumerable<AirQualityData>> GetAirQualityByAqiRangeAsync(int minAqi, int maxAqi)
        {
            var data = await FetchAirQualityDataAsync();
            return data.Where(x => x.Aqi >= minAqi && x.Aqi <= maxAqi && x.Aqi > 0)
                      .OrderByDescending(x => x.Aqi);
        }

   
        // 根據站點名稱查詢特定資料
        // 商業邏輯：模糊搜尋並回傳第一筆符合的資料
        public async Task<AirQualityData?> GetAirQualityBySiteAsync(string siteName)
        {
            var data = await FetchAirQualityDataAsync();
            return data.FirstOrDefault(x => !string.IsNullOrEmpty(x.SiteName) &&
                                           x.SiteName.Contains(siteName, StringComparison.OrdinalIgnoreCase));
        }

   
        // 取得空氣品質統計摘要
        // 商業邏輯：複雜的資料分析和統計計算
        public async Task<AirQualitySummary> GetAirQualitySummaryAsync()
        {
            var data = await FetchAirQualityDataAsync();
            var validData = data.Where(x => x.Aqi > 0).ToList();
            var now = DateTime.Now;

            if (!validData.Any())
            {
                return new AirQualitySummary { LastUpdateTime = now };
            }

            // 計算最大與最小 AQI
            var maxAqi = validData.Max(x => x.Aqi);
            var minAqi = validData.Min(x => x.Aqi);

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
                MaxAqi = maxAqi,
                MinAqi = minAqi,

                // 極值查詢
                // 找出所有 AQI 等於最大/最小的站名
                WorstStation = string.Join(", ", validData.Where(x => x.Aqi == maxAqi).Select(x => x.SiteName)),
                BestStation = string.Join(", ", validData.Where(x => x.Aqi == minAqi).Select(x => x.SiteName)),
                LastUpdateTime = now
            };

            return summary;
        }

    
        // 私有內部類別：API 回應的包裝結構
       
        private class ApiResponse
        {
            [JsonPropertyName("records")]
            public List<AirQualityData> Records { get; set; } = new();
        }
    }
}