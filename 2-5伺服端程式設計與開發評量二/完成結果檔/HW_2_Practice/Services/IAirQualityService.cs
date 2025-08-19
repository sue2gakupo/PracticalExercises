using HW_2_Practice.Models;

namespace AirQualityAPI.Services
{
    /// <summary>
    /// 空氣品質服務介面 - 定義所有業務邏輯方法的契約
    /// </summary>
    public interface IAirQualityService
    {
        /// <summary>取得所有空氣品質資料</summary>
        Task<IEnumerable<AirQualityData>> GetAllAirQualityDataAsync();

        /// <summary>根據縣市查詢空氣品質資料</summary>
        Task<IEnumerable<AirQualityData>> GetAirQualityByCountyAsync(string county);

        /// <summary>根據 AQI 範圍篩選資料</summary>
        Task<IEnumerable<AirQualityData>> GetAirQualityByAqiRangeAsync(int minAqi, int maxAqi);

        /// <summary>根據站點名稱查詢特定資料</summary>
        Task<AirQualityData?> GetAirQualityBySiteAsync(string siteName);

        /// <summary>取得統計摘要</summary>
        Task<AirQualitySummary> GetAirQualitySummaryAsync();
    }
}