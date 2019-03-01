CREATE VIEW vw_DailyExercises AS

SELECT 
	CalendarDate AS CreatedDate, 
	SUM(Metres) AS Metres,
	SUM(TotalSeconds) AS TotalSeconds
FROM 
	Calendar C
LEFT OUTER JOIN
	Exercises E
ON	C.CalendarDate = E.CreatedDate
GROUP BY CalendarDate
