CREATE VIEW vw_Weights_Daily AS

SELECT 
    Cal.Date, 
    AverageDailyWeights.Kg AS Kg,
	T.Kg AS TargetKg,
	AverageDailyWeights.FatRatioPercentage, 
    AVG(AverageDailyWeights.Kg) OVER (ORDER BY Cal.Date ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageKg,
	AVG(AverageDailyWeights.FatRatioPercentage) OVER (ORDER BY Cal.Date ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageFatRatioPercentage
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
ON T.Date = Cal.CreatedDate
