CREATE VIEW vw_MonthlyExercises AS

SELECT
  DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0) as CreatedDate,
  AVG(Metres) as Metres,
  AVG(TotalSeconds) as TotalSeconds
FROM
  vw_DailyExercises
GROUP BY DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0)
