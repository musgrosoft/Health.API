CREATE VIEW vw_DailyExercises AS

SELECT
  CreatedDate,
  SUM(Metres) as Metres,
  SUM(DATEDIFF(MINUTE, '0:00:00', Time)) as Time
FROM
  Exercises
GROUP BY CreatedDate
