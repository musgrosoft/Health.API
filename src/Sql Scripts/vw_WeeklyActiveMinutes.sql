CREATE VIEW vw_WeeklyActiveMinutes AS

SELECT
  DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate) as CreatedDate,
  SUM(FairlyActiveMinutes + VeryActiveMinutes) as ActiveMinutes
FROM
  ActivitySummaries
GROUP BY DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate)
