CREATE VIEW vw_Weights_Daily AS

SELECT 
    dbo.CalendarDates.Date, 
    dbo.Weights.Kg AS Kg,
	dbo.Targets.Kg AS TargetKg,
	FatRatioPercentage, 
    AVG(Kg) OVER (ORDER BY Date ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageKg,
	AVG(FatRatioPercentage) OVER (ORDER BY Date ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageFatRatioPercentage
FROM

	(
		SELECT 
		CAST(CreatedDate AS DATE) AS CreatedDate,
		AVG(Kg) AS Kg,
		AVG(FatRatioPercentage) AS FatRatioPercentage
		FROM
		dbo.Weights
		GROUP BY CAST(CreatedDate AS DATE)
	) AS AverageDailyWeights 

RIGHT JOIN dbo.CalendarDates Cal
ON Cal.Date = AverageDailyWeights.CreatedDate
LEFT JOIN Targets T
ON T.Date = AverageDailyWeights.CreatedDate
