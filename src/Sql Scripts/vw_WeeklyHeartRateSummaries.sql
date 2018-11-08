CREATE VIEW vw_WeeklyHeartRateSummaries AS

SELECT
  DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate) as CreatedDate,
  SUM(CardioMinutes) as CardioMinutes,
  SUM(PeakMinutes) as PeakMinutes
FROM
  vw_DailyHeartRateSummaries
GROUP BY DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate)