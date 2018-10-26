CREATE VIEW vw_DailyStepCounts AS

SELECT
  CreatedDate,
  Count,
  SUM(Count) OVER (ORDER BY CreatedDate) as CumSumCount,
  Target,
  SUM(Target) OVER (ORDER BY CreatedDate) as CumSumTarget
FROM
  StepCounts