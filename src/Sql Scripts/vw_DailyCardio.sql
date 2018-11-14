CREATE VIEW vw_DailyCardio AS
	
SELECT 
	CreatedDate,
	SUM(Minutes) AS Minutes
FROM
	(
	SELECT
		CreatedDate,
		Bpm,
		Minutes
	FROM
		vw_DailyHeartRates
	WHERE
		Bpm >= 125
	) AS CardioBpm
GROUP BY 
	CreatedDate
	