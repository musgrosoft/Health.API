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
		C.Date,
		D.Units,
		T.Units AS TargetUnits
	FROM 
		Drinks D
		INNER JOIN CalendarDates C
		ON D.CreatedDate = C.Date
		LEFT JOIN Targets T
		On T.Date = C.Date
		 
) AS DailyAlcohol