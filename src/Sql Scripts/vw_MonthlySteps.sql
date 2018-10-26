CREATE VIEW vw_MonthlySteps AS

SELECT
  DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0) as CreatedDate,
  Avg(Count) as Count
FROM
  StepCounts
GROUP BY DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0)