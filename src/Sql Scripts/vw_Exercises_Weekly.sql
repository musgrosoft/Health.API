CREATE VIEW vw_WeeklyExercises AS

SELECT
  DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate) as CreatedDate,
  SUM(Metres) as Metres,
  SUM(TotalSeconds) as TotalSeconds
FROM
  vw_DailyExercises
GROUP BY DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate)
