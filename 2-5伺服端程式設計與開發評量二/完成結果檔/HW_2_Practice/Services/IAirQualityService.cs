using HW_2_Practice.Models;

namespace HW_2_Practice.Services
{

    // 空氣品質服務介面 - 定義所有商業邏輯
  
    public interface IAirQualityService
    {
        //取得所有空氣品質資料
        Task<IEnumerable<AirQualityData>> GetAllAirQualityDataAsync();

        //根據縣市查詢空氣品質資料
        Task<IEnumerable<AirQualityData>> GetAirQualityByCountyAsync(string county);

        //根據 AQI 範圍篩選資料
        Task<IEnumerable<AirQualityData>> GetAirQualityByAqiRangeAsync(int minAqi, int maxAqi);

        //根據站點名稱查詢特定資料
        Task<AirQualityData?> GetAirQualityBySiteAsync(string siteName);

        //取得統計摘要
        Task<AirQualitySummary> GetAirQualitySummaryAsync();
    }
}