CREATE VIEW vw_MinuteHeartRates AS

SELECT 
	CreatedDate,
	MAX(Bpm) AS Bpm
FROM 
	vw_RawMinuteHeartRates
GROUP BY
	CreatedDate