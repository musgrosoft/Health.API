CREATE VIEW vw_Sleeps_Weekly AS

SELECT 
    DATEADD(dd,-(DATEPART(dw, DateOfSleep)-1), DateOfSleep) as DateOfSleep,
    AVG(MinutesAsleep) AS MinutesAsleep
FROM

vw_Sleeps_Daily

GROUP BY (DATEADD(dd,-(DATEPART(dw, DateOfSleep)-1), DateOfSleep))