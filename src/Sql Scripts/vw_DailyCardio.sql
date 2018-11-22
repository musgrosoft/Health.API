CREATE VIEW vw_DailyCardio 
WITH SCHEMABINDING AS

	SELECT
		DATEADD(dd, 0, DATEDIFF(dd, 0, CreatedDate)) AS CreatedDate,
		COUNT(Bpm) AS CardioMinutes
	FROM
		(
			SELECT 
				CreatedDate,
				MAX(Bpm) AS Bpm
			FROM 
				(
				
				SELECT
					DATEADD(mi, DATEDIFF(mi, 0, CreatedDate), 0) as CreatedDate,
					MAX(Source) as Source,
					AVG(Bpm) as Bpm
				FROM Health.HeartRates
				GROUP BY
					DATEADD(mi, DATEDIFF(mi, 0, CreatedDate), 0),
					Source

				) AS RawMinuteHeartRates
			GROUP BY
				CreatedDate
		) AS MinuteHeartRates
	WHERE
		Bpm >= 125
	GROUP BY 
		DATEADD(dd, 0, DATEDIFF(dd, 0, CreatedDate));
		