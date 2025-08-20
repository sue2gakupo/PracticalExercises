using HW_2_Practice.Models;
using HW_2_Practice.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityAPI.Controllers
{
  
    // 空氣品質 API 控制器
    // 處理 HTTP 請求，協調服務層，回應結果
    [ApiController]                   
    [Route("api[controller]")]       
    public class AirQualityController : ControllerBase
    {
        private readonly IAirQualityService _airQualityService;
       
        public AirQualityController(IAirQualityService airQualityService)
        {
            _airQualityService = airQualityService;
        }

   

        // API 端點 1：取得所有空氣品質監測資料
        // Route: GET apiAirQuality/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AirQualityData>>> GetAllAirQualityData()
        {
            try
            {
                var data = await _airQualityService.GetAllAirQualityDataAsync();
                return Ok(data);  // 回傳 HTTP 200 + 資料
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");  // 回傳 HTTP 500
            }
        }

        // API 端點 2：根據縣市名稱查詢空氣品質資料
        // Route: GET apiAirQuality/by-county/{county}
        // 路徑參數：county (縣市名稱)

        [HttpGet("by-county/{county}")]
        public async Task<ActionResult<IEnumerable<AirQualityData>>> GetAirQualityByCounty(string county)
        {
            try
            {
                var data = await _airQualityService.GetAirQualityByCountyAsync(county);
                if (!data.Any())
                {
                    return NotFound($"找不到縣市 '{county}' 的空氣品質資料");  // HTTP 404
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
            }
        }

       
        // API 端點 3：根據 AQI 篩選空氣品質資料
        // Route: GET apiAirQuality/by-aqi-range?minAqi=0&maxAqi=100
        // 查詢參數：minAqi, maxAqi
        [HttpGet("by-aqi-range")]
        public async Task<ActionResult<IEnumerable<AirQualityData>>> GetAirQualityByAqiRange(
             int minAqi = 0,      // 預設最小值
             int maxAqi = 300)    // 預設最大值
        {
            try
            {
                var data = await _airQualityService.GetAirQualityByAqiRangeAsync(minAqi, maxAqi);
                // 檢查回傳的資料是否為空
                if (!data.Any())
                {
                    // 如果沒有找到任何資料，回傳 404 Not Found
                    return NotFound("找不到符合條件的空氣品質資料。");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
            }
        }

  
        // API 端點 4：根據站點名稱查詢特定監測站資料
        // Route: GET apiAirQuality/by-site/{siteName}
        // 路徑參數：siteName (監測站名稱)
        [HttpGet("by-site/{siteName}")]
        public async Task<ActionResult<AirQualityData>> GetAirQualityBySite(string siteName)
        {
            try
            {
                var data = await _airQualityService.GetAirQualityBySiteAsync(siteName);
                if (data == null) // 使用 '==' 進行 null 檢查
                {
                    return NotFound($"找不到監測站 '{siteName}' 的資料");
                }
                return Ok(data); // 直接回傳 data，無需轉型
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
            }
        }

  
        // API 端點 5：取得空氣品質統計摘要
        // Route: GET apiAirQuality/summary
        [HttpGet("summary")]
        public async Task<ActionResult<AirQualitySummary>> GetAirQualitySummary()
        {
            try
            {
                var summary = await _airQualityService.GetAirQualitySummaryAsync();

                return Ok(summary);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
            }
        }
    }
}