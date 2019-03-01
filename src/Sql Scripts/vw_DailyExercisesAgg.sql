CREATE VIEW vw_DailyExercisesAgg AS

SELECT
  CreatedDate,
  Metres,
  TotalSeconds,
  TotalSeconds/60 as TotalMinutes,
  SUM(TotalSeconds/60) OVER (ORDER BY CreatedDate) as CumSumMinutes
FROM
  vw_DailyExercises
