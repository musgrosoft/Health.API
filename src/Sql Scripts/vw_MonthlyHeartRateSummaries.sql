CREATE VIEW vw_MonthlyHeartRateSummaries AS

SELECT
  DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0) as CreatedDate,
  AVG(CardioMinutes) as CardioMinutes,
  AVG(PeakMinutes) as PeakMinutes
FROM
  vw_DailyHeartRateSummaries
GROUP BY DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0)
