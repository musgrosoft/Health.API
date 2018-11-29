CREATE VIEW vw_DailyWeights AS

SELECT 
    CreatedDate, 
    Kg, 
	FatRatioPercentage,
    TargetKg, 
    avg(Kg) OVER (ORDER BY CreatedDate ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageKg,
	avg(FatRatioPercentage) OVER (ORDER BY CreatedDate ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageFatRatioPercentage
FROM
	(
		SELECT 
		CAST(CreatedDate AS DATE) AS CreatedDate,
		Avg(Kg) AS Kg,
		Avg(FatRatioPercentage) AS FatRatioPercentage,
		Avg(TargetKg) AS TargetKg
		FROM
		Weights
		GROUP BY CAST(CreatedDate AS DATE)
	) as DailyWeights
