CREATE VIEW vw_DailyHeartRateSummaries AS

SELECT
  CreatedDate,
  MAX(CardioMinutes) as CardioMinutes,
  MAX(PeakMinutes) as PeakMinutes
FROM
  HeartRateSummaries
GROUP BY CreatedDate