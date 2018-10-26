CREATE VIEW vw_WeeklyStepCounts AS

SELECT
  DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate) as CreatedDate,
  SUM(Count) as Count
FROM
  StepCounts
GROUP BY DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate)