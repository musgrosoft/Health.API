CREATE VIEW vw_DailyExercises AS

SELECT 
	CalendarDate AS CreatedDate, 
	SUM(COALESCE(Metres,0)) AS Metres,
	SUM(COALESCE(TotalSeconds,0)) AS TotalSeconds
FROM 
	Calendar C
LEFT OUTER JOIN
	Exercises E
ON	C.CalendarDate = E.CreatedDate
GROUP BY CalendarDate
