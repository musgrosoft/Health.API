CREATE VIEW vw_DailyHeartRates AS

SELECT
	DATEADD(dd, 0, DATEDIFF(dd, 0, CreatedDate)) AS CreatedDate,
	Bpm,
	Count(Bpm) as Minutes
FROM
	vw_MinuteHeartRates
GROUP BY
	DATEADD(dd, 0, DATEDIFF(dd, 0, CreatedDate)),
	Bpm 
	