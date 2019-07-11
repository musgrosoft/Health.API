CREATE VIEW vw_DailyAlcoholIntakes AS

SELECT
    Date,
    Units,
	TargetUnits,
	SUM(TargetUnits) OVER (ORDER BY Date) + 5147.7 as CumSumTargetUnits,	
	SUM(Units) OVER (ORDER BY Date) as CumSumUnits
FROM
(
	SELECT 
		Date,
		Units,
		CASE 
			WHEN Date > '2018/05/29' THEN 4
			ELSE NULL
		END AS TargetUnits
	FROM 
		Drinks D
		INNER JOIN CalendarDates C
		ON D.CreatedDate = C.Date
		 
) AS DailyAlcohol