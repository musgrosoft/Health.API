CREATE VIEW vw_FitbitSleeps_Daily AS


		SELECT 
		CAST(StartTime AS DATE) AS StartDate,		
		SUM(LightMinutes)/60 AS LightMinutes,
		SUM(DeepMinutes)/60 AS DeepMinutes,
		SUM(RemMinutes)/60 AS RemMinutes,
		(SUM(LightMinutes) + SUM(DeepMinutes) + SUM(RemMinutes))  AS TotalSleepMinutes,
		MinutesAsleep,
		MAX(EndTime AS DATE) AS EndDate
		FROM
		dbo.MyFitbitSleeps
		GROUP BY CAST(StartTime AS DATE)
