CREATE VIEW vw_DailyNamedExercises AS

SELECT
  CreatedDate,
  Description,
  SUM(Metres) as Metres,
  SUM(DATEDIFF(MINUTE, '0:00:00', Time)) as Time
FROM
  Exercises
GROUP BY CreatedDate,
	Description
