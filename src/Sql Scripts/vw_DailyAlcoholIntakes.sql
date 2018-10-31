CREATE VIEW vw_DailyAlcoholIntakes AS

SELECT
  CreatedDate,
  Units,
  SUM(Units) OVER (ORDER BY CreatedDate) as CumSumUnits,
  Target,
  SUM(Target) OVER (ORDER BY CreatedDate) as CumSumTarget
FROM
  AlcoholIntakes