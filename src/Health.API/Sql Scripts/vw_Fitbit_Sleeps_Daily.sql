CREATE VIEW vw_FitbitSleeps_Daily AS


		SELECT 
		CAST(StartTime AS DATE) AS StartDate,		
		SUM(LightMinutes)/60 AS LightMinutes,
		SUM(DeepMinutes)/60 AS DeepMinutes,
		SUM(RemMinutes)/60 AS RemMinutes,
		(SUM(LightMinutes) + SUM(DeepMinutes) + SUM(RemMinutes))  AS TotalSleepMinutes,
		MAX(EndTime) AS EndDate
		FROM
		dbo.Sleeps
		GROUP BY CAST(StartTime AS DATE)
