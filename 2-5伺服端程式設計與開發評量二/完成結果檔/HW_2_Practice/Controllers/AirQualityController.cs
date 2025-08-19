using AirQualityAPI.Models;
using AirQualityAPI.Services;
using HW_2_Practice.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityAPI.Controllers
{
    /// <summary>
    /// 空氣品質 API 控制器
    /// 職責：處理 HTTP 請求，協調服務層，回應結果
    /// </summary>
    [ApiController]                    // 標記為 API 控制器
    [Route("api/[controller]")]        // 路由模板：api/airquality
    public class AirQualityController : ControllerBase
    {
        private readonly IAirQualityService _airQualityService;

        /// <summary>
        /// 建構函式 - 依賴注入服務
        /// </summary>
        public AirQualityController(IAirQualityService airQualityService)
        {
            _airQualityService = airQualityService;
        }

        /// <summary>
        /// API 端點 1：取得所有空氣品質監測資料
        /// HTTP Method: GET
        /// Route: GET api/airquality/all
        /// </summary>
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

        /// <summary>
        /// API 端點 2：根據縣市名稱查詢空氣品質資料
        /// HTTP Method: GET
        /// Route: GET api/airquality/by-county/{county}
        /// 路徑參數：county (縣市名稱)
        /// </summary>
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

        /// <summary>
        /// API 端點 3：根據 AQI 等級篩選空氣品質資料
        /// HTTP Method: GET
        /// Route: GET api/airquality/by-aqi-range?minAqi=0&maxAqi=100
        /// 查詢參數：minAqi, maxAqi
        /// </summary>
        [HttpGet("by-aqi-range")]
        public async Task<ActionResult<IEnumerable<AirQualityData>>> GetAirQualityByAqiRange(
            [FromQuery] int minAqi = 0,      // 預設最小值
            [FromQuery] int maxAqi = 500)    // 預設最大值
        {
            try
            {
                var data = await _airQualityService.GetAirQualityByAqiRangeAsync(minAqi, maxAqi);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
            }
        }

        /// <summary>
        /// API 端點 4：根據站點名稱查詢特定監測站資料
        /// HTTP Method: GET
        /// Route: GET api/airquality/by-site/{siteName}
        /// 路徑參數：siteName (監測站名稱)
        /// </summary>
        [HttpGet("by-site/{siteName}")]
        public async Task<ActionResult<AirQualityData>> GetAirQualityBySite(string siteName)
        {
            try
            {
                var data = await _airQualityService.GetAirQualityBySiteAsync(siteName);
                if (data == null)
                {
                    return NotFound($"找不到監測站 '{siteName}' 的資料");
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
            }
        }

        /// <summary>
        /// API 端點 5：取得空氣品質統計摘要
        /// HTTP Method: GET
        /// Route: GET api/airquality/summary
        /// </summary>
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