CREATE VIEW vw_MonthlyCardio AS

SELECT
  DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0) as CreatedDate,
  AVG(Cardio) as CardioAndAboveMinutes
FROM
  vw_DailyCardio
GROUP BY DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0)