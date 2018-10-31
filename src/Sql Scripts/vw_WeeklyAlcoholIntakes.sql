CREATE VIEW vw_WeeklyAlcoholIntakes AS

SELECT
  DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate) as CreatedDate,
  SUM(Units) as Units
FROM
  AlcoholIntakes
GROUP BY DATEADD(dd,-(DATEPART(dw, CreatedDate)-1), CreatedDate)