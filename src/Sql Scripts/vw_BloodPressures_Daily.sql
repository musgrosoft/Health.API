CREATE VIEW vw_BloodPressures_Daily AS

SELECT 
    CreatedDate, 
    Systolic, 
    Diastolic, 
    AVG(Systolic) OVER (ORDER BY CreatedDate ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageSystolic,
	AVG(Diastolic) OVER (ORDER BY CreatedDate ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageDiastolic
FROM 
(
	SELECT 
	CAST(CreatedDate AS DATE) AS CreatedDate,
	AVG(Systolic) AS Systolic,
	AVG(Diastolic) AS Diastolic
	FROM BloodPressures
	GROUP BY CAST(CreatedDate AS DATE)
) AS DailyBloodpressures;