	select
		dateadd(mi, datediff(mi, 0, CreatedDate), 0) as CreatedDate,
		max(Source) as Source,
		avg(Bpm) as aBpm
	from HeartRates
	group by 
		dateadd(mi, datediff(mi, 0, CreatedDate), 0),
		Source
	--having  avg(Bpm) >= 125
) as MinuteHeartRates