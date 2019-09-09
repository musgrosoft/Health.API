CREATE VIEW vw_FitbitSleeps_Daily AS


		SELECT 
		CAST(DATEADD(hour,-12,StartTime) AS DATE) AS CorrectedStartDate,		
		SUM(LightMinutes)/60 AS LightMinutes,
		SUM(DeepMinutes)/60 AS DeepMinutes,
		SUM(RemMinutes)/60 AS RemMinutes,
		(SUM(LightMinutes) + SUM(DeepMinutes) + SUM(RemMinutes))  AS TotalSleepMinutes,
		SUM (MinutesAsleep) AS MinutesAsleep
		FROM
		dbo.MyFitbitSleeps
		GROUP BY CAST(DATEADD(hour,-12,StartTime) AS DATE)
