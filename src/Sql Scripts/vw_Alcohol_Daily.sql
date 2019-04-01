CREATE VIEW vw_DailyAlcoholIntakes AS

SELECT 
    CalendarDate,
    CASE 
        WHEN CalendarDate > '2018/05/29' THEN 4
        ELSE NULL
    END AS TargetUnits,
	SUM(
		CASE 
			WHEN CalendarDate > '2018/05/29' THEN 4
			ELSE NULL
	    END 
		* 1.000) OVER (ORDER BY CalendarDate) as CumSumTargetUnits,
	Units,
	SUM(Units) OVER (ORDER BY CreatedDate) as CumSumUnits
FROM 
	AlcoholIntakes AI
	INNER JOIN Calendar C
	ON AI.CreatedDate = C.CalendarDate