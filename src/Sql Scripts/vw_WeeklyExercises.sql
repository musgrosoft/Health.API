CREATE VIEW vw_WeeklyExercises AS

SELECT
  DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate) as CreatedDate,
  SUM(Metres) as Metres,
  SUM(DATEDIFF(MINUTE, '0:00:00', Time)) as Time
FROM
  Exercises
GROUP BY DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate)
