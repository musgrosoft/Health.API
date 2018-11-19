CREATE VIEW vw_DailyHeartRateSummaries AS

SELECT
  CreatedDate,
  SUM(CardioMinutes) as CardioMinutes,
  SUM(PeakMinutes) as PeakMinutes
FROM
  HeartRateSummaries
GROUP BY CreatedDate