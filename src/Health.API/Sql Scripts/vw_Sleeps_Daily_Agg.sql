CREATE VIEW vw_Sleeps_Daily_Agg AS

SELECT
  DateOfSleep,
  MinutesAsleep,
  SUM(MinutesAsleep) OVER (ORDER BY DateOfSleep) as CumSumMinutes
FROM
  vw_Sleeps_Daily
