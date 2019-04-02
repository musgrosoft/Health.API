CREATE VIEW vw_DailyAlcoholIntakes AS

SELECT
    CalendarDate,
    Units,
	TargetUnits,
	SUM(TargetUnits) OVER (ORDER BY CalendarDate) + 5147.7 as CumSumTargetUnits,	
	SUM(Units) OVER (ORDER BY CalendarDate) as CumSumUnits
FROM
(
	SELECT 
		CalendarDate,
		Units,
		CASE 
			WHEN CalendarDate > '2018/05/29' THEN 4
			ELSE NULL
		END AS TargetUnits
	FROM 
		AlcoholIntakes AI
		INNER JOIN Calendar C
		ON AI.CreatedDate = C.CalendarDate
		 
) AS DailyAlcohol