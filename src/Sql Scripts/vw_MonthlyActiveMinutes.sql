CREATE VIEW vw_MonthlyActiveMinutes AS

SELECT
  DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0) as CreatedDate,
  AVG(FairlyActiveMinutes + VeryActiveMinutes) as ActiveMinutes
FROM
  ActivitySummaries
GROUP BY DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0)
