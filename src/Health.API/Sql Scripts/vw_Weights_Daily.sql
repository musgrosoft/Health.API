CREATE VIEW vw_Weights_Daily AS

SELECT 
    CalendarDate, 
    Kg, 
	FatRatioPercentage, 
    AVG(Kg) OVER (ORDER BY CalendarDate ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageKg,
	AVG(FatRatioPercentage) OVER (ORDER BY CalendarDate ROWS BETWEEN 9 PRECEDING AND CURRENT ROW) AS MovingAverageFatRatioPercentage,
	CASE 
        WHEN CalendarDate >= '2019/01/01' THEN 86    - ((3.000/365) * (DATEDIFF(day , '2019/01/01' , CalendarDate)))
		WHEN CalendarDate >= '2018/09/01' THEN 88.7  - ((0.250/30)  * (DATEDIFF(day , '2018/09/01' , CalendarDate)))
		WHEN CalendarDate >= '2018/05/01' THEN 90.74 - ((0.500/30)  * (DATEDIFF(day , '2018/05/01' , CalendarDate)))
        ELSE NULL
    END AS TargetKg
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
		RIGHT JOIN dbo.Calendar Cal
		ON Cal.CalendarDate = AverageDailyWeights.CreatedDate