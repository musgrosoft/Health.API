CREATE VIEW vw_MonthlyNamedExercises AS

SELECT
  DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0) as CreatedDate,
  Description,
  AVG(Metres) as Metres,
  AVG(DATEDIFF(MINUTE, '0:00:00', Time)) as Time
FROM
  Exercises
GROUP BY 
	DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0),
	Description
