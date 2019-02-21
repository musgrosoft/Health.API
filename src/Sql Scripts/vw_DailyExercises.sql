CREATE VIEW vw_DailyExercises AS

SELECT
  CreatedDate,
  SUM(Metres) as Metres,
  SUM(TotalSeconds) as TotalSeconds
FROM
  Exercises
GROUP BY CreatedDate
