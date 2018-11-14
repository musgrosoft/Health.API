CREATE VIEW vw_RawMinuteHeartRates AS
	
SELECT
	DATEADD(mi, DATEDIFF(mi, 0, CreatedDate), 0) as CreatedDate,
	MAX(Source) as Source,
	AVG(Bpm) as Bpm
FROM HeartRates
GROUP BY
	DATEADD(mi, DATEDIFF(mi, 0, CreatedDate), 0),
	Source
	