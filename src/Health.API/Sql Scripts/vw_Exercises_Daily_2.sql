CREATE VIEW vw_DailyExercises_2 AS

SELECT 
	C.Date AS CreatedDate, 
	E.Metres,
	E.TotalSeconds,
	E.Description
FROM 
	CalendarDates C
LEFT OUTER JOIN
	Exercises E
ON	C.Date = E.CreatedDate

