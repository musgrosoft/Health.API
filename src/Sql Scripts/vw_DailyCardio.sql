CREATE VIEW vw_DailyCardio AS

SELECT 
	CreatedDate,
	MAX(CardioAndAboveMinutes) AS Cardio
FROM

(
SELECT
  CreatedDate,
  CardioMinutes + PeakMinutes AS CardioAndAboveMinutes
FROM
  vw_DailyHeartRateSummaries 

UNION

SELECT
  CreatedDate,
  Time AS CardioAndAboveMinutes
FROM
	vw_DailyExercises
) AS Things
GROUP BY CreatedDate