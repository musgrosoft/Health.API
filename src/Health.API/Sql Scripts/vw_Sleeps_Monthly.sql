CREATE VIEW vw_Sleeps_Monthly AS

SELECT 
    DATEADD(m, DATEDIFF(m, 0, DateOfSleep), 0) as DateOfSleep,
    AVG(MinutesAsleep) AS MinutesAsleep
FROM

vw_Sleeps_Daily

GROUP BY (DATEADD(m, DATEDIFF(m, 0, DateOfSleep), 0))