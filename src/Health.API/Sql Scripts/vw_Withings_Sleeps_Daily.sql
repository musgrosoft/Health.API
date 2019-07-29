CREATE VIEW vw_WithingsSleeps_Daily AS


		SELECT 
		CAST(StartDate AS DATE) AS StartDate,		
		SUM(LightSleepDuration)/60 AS LightSleepMinutes,
		SUM(DeepSleepDuration)/60 AS DeepSleepMinutes,
		SUM(RemSleepDuration)/60 AS RemSleepMinutes,
		(SUM(LightSleepDuration) + SUM(DeepSleepDuration) + SUM(RemSleepDuration)) / 60 AS TotalSleepMinutes,
		MAX(EndDate) AS EndDate
		FROM
		dbo.MyWithingsSleeps
		GROUP BY CAST(StartDate AS DATE)
