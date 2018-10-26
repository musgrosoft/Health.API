CREATE VIEW vw_DailyActiveMinutes AS

SELECT
  CreatedDate,
  FairlyActiveMinutes + VeryActiveMinutes as ActiveMinutes,
  TargetActiveMinutes,
  SUM(FairlyActiveMinutes + VeryActiveMinutes) OVER (ORDER BY CreatedDate) as CumSumActiveMinutes,
  SUM(TargetActiveMinutes) OVER (ORDER BY CreatedDate) as CumSumTargetActiveMinutes

FROM
  ActivitySummaries

