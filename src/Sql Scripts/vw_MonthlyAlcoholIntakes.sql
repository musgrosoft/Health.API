CREATE VIEW vw_MonthlyAlcoholIntakes AS

SELECT
  DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0) as CreatedDate,
  Avg(Units) as Units
FROM
  AlcoholIntakes
GROUP BY DATEADD(m, DATEDIFF(m, 0, CreatedDate), 0)