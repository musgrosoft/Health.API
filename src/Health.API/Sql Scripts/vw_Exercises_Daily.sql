CREATE VIEW vw_DailyExercises AS

SELECT 
	Date AS CreatedDate, 
	SUM(COALESCE(Metres,0)) AS Metres,
	SUM(COALESCE(TotalSeconds,0)) AS TotalSeconds
FROM 
	CalendarDates C
LEFT OUTER JOIN
	Exercises E
ON	C.Date = E.CreatedDate
GROUP BY Date
