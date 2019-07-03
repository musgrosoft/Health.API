CREATE TABLE [Calendar]
(
    [CalendarDate] DATETIME
)

DECLARE @StartDate DATETIME
DECLARE @EndDate DATETIME
SET @StartDate = '2010-01-01'
SET @EndDate = '2030-01-01'

WHILE @StartDate <= @EndDate
      BEGIN
             INSERT INTO [Calendar]
             (
                   CalendarDate
             )
             SELECT
                   @StartDate

             SET @StartDate = DATEADD(dd, 1, @StartDate)
      END