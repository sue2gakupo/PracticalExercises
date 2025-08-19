namespace HW_2_Practice.Models
{
    /// <summary>
    /// 空氣品質統計摘要模型
    /// </summary>
    public class AirQualitySummary
    {
        public int TotalStations { get; set; }                    // 總監測站數量
        public Dictionary<string, int> CountyDistribution { get; set; } = new();  // 各縣市站數分布
        public Dictionary<string, int> StatusDistribution { get; set; } = new();  // 各狀態分布
        public double AverageAqi { get; set; }                   // 平均 AQI
        public int MaxAqi { get; set; }                          // 最高 AQI
        public int MinAqi { get; set; }                          // 最低 AQI
        public string? WorstStation { get; set; }                // 空氣品質最差站點
        public string? BestStation { get; set; }                 // 空氣品質最佳站點
        public DateTime LastUpdateTime { get; set; }             // 最後更新時間
    }
}