CREATE VIEW vw_Sleeps_Daily AS

SELECT 
    CAST(DateOfSleep AS DATE) AS DateOfSleep,
    SUM(MinutesAsleep) AS MinutesAsleep
FROM

SleepSummaries

GROUP BY (CAST(DateOfSleep AS DATE))

