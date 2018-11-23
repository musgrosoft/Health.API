CREATE VIEW vw_WeeklyCardio AS

SELECT
  DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate) as CreatedDate,
  SUM(Cardio) as CardioAndAboveMinutes
FROM
  vw_DailyCardio
GROUP BY DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate)