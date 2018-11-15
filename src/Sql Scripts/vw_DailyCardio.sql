CREATE VIEW vw_DailyCardio AS
	
	SELECT
		DATEADD(dd, 0, DATEDIFF(dd, 0, CreatedDate)) AS CreatedDate,
		COUNT(Bpm) AS CardioMinutes
	FROM
		vw_MinuteHeartRates
	WHERE
		Bpm >= 125
	GROUP BY 
		DATEADD(dd, 0, DATEDIFF(dd, 0, CreatedDate))
