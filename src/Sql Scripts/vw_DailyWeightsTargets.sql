CREATE VIEW vw_DailyWeightsTargets AS

SELECT 
    CalendarDate,
    CASE 
        WHEN CalendarDate >= '2019/01/01' THEN 86 - ((3.000/365) * (DATEDIFF(day , '2019/01/01', CalendarDate)))
		WHEN CalendarDate >= '2018/09/01' THEN 88.7 - ((0.250/30)  * (DATEDIFF(day, '2018/09/01', CalendarDate)))
		WHEN CalendarDate >= '2018/05/01' THEN 90.74 - ((0.500/30) * (DATEDIFF(day, '2018/05/01' , CalendarDate)))
        ELSE NULL
    END AS TargetWeight
FROM 
    Calendar