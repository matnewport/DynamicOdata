use ScheduleDB

declare @reportdate as datetime = '20211204'
declare @DutyNumber as int = 1003
declare @schedule as varchar(10) = 'Weekday'
;

with cte_schedules as
(
	select
	st.ScheduleTypeName,
	st.ScheduleTypeDisplay,
	c.ScheduleDate,
	c.SchedulingUnit,
	c.DayOfTheWeek,
	c.IsBaseSchedule,
	c.VehicleScheduleID,
	v.ScheduleName,
	v.ScheduleTypeID,
	v.[Description],
	v.Scenario

	FROM dbo.Calendars as c with (NOLOCK)
	INNER JOIN dbo.VehicleSchedules as v with (NOLOCK)
	ON c.VehicleScheduleID = v.VehicleScheduleID
	INNER JOIN dbo.VehicleSchedules_Blocks as  vb with (NOLOCK)
	ON v.VehicleScheduleID = vb.VehicleScheduleID
	AND @ReportDate BETWEEN vb.ValidFrom AND vb.ValidTo
	inner join ScheduleTypes as st with (NOLOCK)
	on v.ScheduleTypeID = st.ScheduleTypeID

	where c.ScheduleDate = @ReportDate
	and st.ScheduleTypeName = @Schedule

	group by
	st.ScheduleTypeName,
	st.ScheduleTypeDisplay,
	c.ScheduleDate,
	c.SchedulingUnit,
	c.DayOfTheWeek,
	c.IsBaseSchedule,
	c.VehicleScheduleID,
	v.ScheduleName,
	v.ScheduleTypeID,
	v.[Description],
	v.Scenario
)
,
cte_blocks as
(
	  select
	  b.[InternalBlockNumber],
	  b.[BlockNumber],
	  b.[OperatingDays],
	  b.[BlockStartPlace],
	  b.[BlockStartTime],
	  b.[InServiceStartPlace],
	  b.[InServiceStartTime],
	  b.[InServiceEndPlace],
	  b.[InServiceEndTime],
	  b.[BlockEndPlace],
	  b.[BlockEndTime],
	  vt.TypeCharacter,
	  b.[ValidFrom],
	  b.[ValidTo]

	  FROM [ScheduleDB].[dbo].[Blocks] as b with (NOLOCK)
	  left outer join VehicleTypes as vt with (NOLOCK)
	  on b.VehicleTypeID = vt.VehicleTypeID

	  where 
	  @ReportDate >= b.ValidFrom and @ReportDate <= b.ValidTo

	  group by
	  b.[InternalBlockNumber],
	  b.[BlockNumber],
	  b.[OperatingDays],
	  b.[BlockStartPlace],
	  b.[BlockStartTime],
	  b.[InServiceStartPlace],
	  b.[InServiceStartTime],
	  b.[InServiceEndPlace],
	  b.[InServiceEndTime],
	  b.[BlockEndPlace],
	  b.[BlockEndTime],
	  vt.TypeCharacter,
	  b.[ValidFrom],
	  b.[ValidTo]
)
,

cte_duties as
(
	 select
	 dp.DutyNumber,
	 dp.InternalDutyID,
	 dp.InternalBlockNumber,
	 dp.CrewScheduleID,
	 dp.PieceStartPlace,
	 ISNULL(dp.PieceStartTime, 0) as PieceStartTime,
	 dp.PieceEndPlace,
	 ISNULL(dp.PieceEndTime, 0) as PieceEndTime,
	 dp.PieceReportPlace,
	 dp.PieceReportTime,
	 dp.PieceClearPlace,
	 dp.PieceClearTime,
	 dp.PieceSequence,
	 dp.ValidFrom,
	 dp.ValidTo

	 from DutyPieces as dp with (NOLOCK)

	 where 
	 @ReportDate >= dp.ValidFrom and @ReportDate <= dp.ValidTo
	 and @DutyNumber = dp.DutyNumber

	 group by
	 dp.DutyNumber,
	 dp.InternalDutyID,
	 dp.InternalBlockNumber,
	 dp.CrewScheduleID,
	 dp.PieceStartPlace,
	 ISNULL(dp.PieceStartTime, 0),
	 dp.PieceEndPlace,
	 ISNULL(dp.PieceEndTime, 0),
	 dp.PieceReportPlace,
	 dp.PieceReportTime,
	 dp.PieceClearPlace,
	 dp.PieceClearTime,
	 dp.PieceSequence,
	 dp.ValidFrom,
	 dp.ValidTo
)
,

cte_trips as
(
	select
	t.InternalTripNumber,
	t.RouteID ,
	tp.DirectionID,
	t.OperatingDays,
	t.TripNumber,
	tps.PlaceSequence,
	tps.IsTimingPoint,
	tst.StopSequence,
	tst.PassingTime,
	st.StopID,
	st.IsBusStop,
	st.[Description],
	p.[AlternateName],
	t.Terminal,
	ti.VehicleScheduleID,
	ti.InternalBlockNumber,
	tt.TripTypeName,
	tp.TripPatternID,
	tp.FromToVia,
	tp.ViaVariant,
	tp.VariantDescription,
	t.DutyNumber,
	cmt.DriverText,
	cmt.PublicText

	from 
	dbo.TripIndex ti
	left outer join dbo.Trips t
	ON ti.InternalTripNumber = t.InternalTripNumber
	AND @ReportDate >= t.ValidFrom AND @ReportDate <= t.ValidTo
	AND @ReportDate >= ti.ValidFrom and @ReportDate <= ti.ValidTo

	left outer JOIN dbo.TripStopTimes tst
	ON t.InternalTripNumber = tst.InternalTripNumber
	AND @ReportDate >= tst.ValidFrom AND @ReportDate <= tst.ValidTo

	left outer JOIN dbo.TripPatterns tp
	ON t.RouteID = tp.RouteID
	AND t.TripPatternID = tp.TripPatternID
	AND @ReportDate >= tp.ValidFrom AND @ReportDate <= tp.ValidTo

	left outer JOIN dbo.TripPatternStops tps
	ON tp.RouteID = tps.RouteID
	AND tp.TripPatternID = tps.TripPatternID
	AND tps.StopSequence = tst.StopSequence
	AND @ReportDate >= tps.ValidFrom AND @ReportDate <= tps.ValidTo

	left outer join TripTypes as tt
	on t.TripTypeID = tt.TripTypeID

	left outer join Comments as cmt
	on tst.CommentID = cmt.CommentID
	and @ReportDate >= cmt.ValidFrom and @ReportDate <= cmt.ValidTo

	left outer join Stops as st
	on tps.StopID = st.StopID
	and @ReportDate >= st.ValidFrom
	and @ReportDate <= st.ValidTo

	left outer join Places as p
	on tps.PlaceCode = p.PlaceCode
	and @ReportDate >= p.ValidFrom
	and @ReportDate <= p.ValidTo

	
	--AND @ReportDate >= ti.ValidFrom and @ReportDate <= ti.ValidTo

	group by
	t.InternalTripNumber,
	t.RouteID ,
	tp.DirectionID,
	t.OperatingDays,
	t.TripNumber,
	tps.PlaceSequence,
	tst.StopSequence,
	st.StopID,
	st.IsBusStop,
	st.[Description],
	p.[AlternateName],
	tps.IsTimingPoint,
	tst.PassingTime,
	t.Terminal,
	ti.VehicleScheduleID,
	ti.InternalBlockNumber,
	tt.TripTypeName,
	tp.TripPatternID,
	tp.FromToVia,
	tp.ViaVariant,
	tp.VariantDescription,
	t.DutyNumber,
	cmt.DriverText,
	cmt.PublicText
)

/*
************
END CTES,
BEGIN MODEL
************
*/


select 
d.DutyNumber,
@ReportDate as Effective_Date,
s.ScheduleTypeName,
b.BlockNumber,
b.TypeCharacter as Vehicle_Type,
RTRIM(CONCAT(b.BlockNumber, ' ', b.TypeCharacter)) as Block_Display,
b.InternalBlockNumber,
t.RouteID,
t.Terminal,
t.DirectionID,
t.TripNumber,
t.TripTypeName,

case
when  t.TripTypeName IN ('Pull-out') THEN -1
when  t.TripTypeName IN ('Pull-in') THEN 99999
when  t.TripTypeName IN ('Deadhead') THEN (lag(t.tripnumber) over (PARTITION BY b.blocknumber, b.internalblocknumber, t.routeid ORDER BY t.PassingTime))+1
else t.TripNumber
end as TripNumSort,

t.TripPatternID,
t.FromToVia,
t.ViaVariant,
t.VariantDescription,
t.PlaceSequence,
t.StopSequence,
t.StopID,
t.AlternateName,
t.DriverText,
t.PublicText,
t.PassingTime,

case 
when 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((t.PassingTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((t.PassingTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((t.PassingTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((t.PassingTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'p'
END
as Formatted_Passing_Time,

d.PieceReportPlace,
d.PieceReportTime,
(d.PieceStartTime - (60 * 10) ) as PieceReportTimeCalc,

case 
when 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceReportTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceReportTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceReportTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceReportTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'p'
END as Formatted_Piece_Report_Time,

d.PieceStartPlace,
d.PieceStartTime,

case 
when 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceStartTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceStartTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceStartTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceStartTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'p'
END
as Formatted_Piece_Start_Time,

d.PieceEndPlace,
d.PieceEndTime,

case 
when 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceEndTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceEndTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceEndTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceEndTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'p'
END
as Formatted_Piece_End_Time,

RIGHT( CAST((d.PieceEndTime - d.PieceStartTime)/3600 as VARCHAR),2) + 'h' +
RIGHT('0' + CAST(((d.PieceEndTime - d.PieceStartTime)/60) % 60 as VARCHAR),2)
as Platform_Time,

RIGHT( CAST((d.PieceClearTime - d.PieceReportTime)/3600 as VARCHAR),2) + 'h' +
RIGHT('0' + CAST(((d.PieceClearTime - d.PieceReportTime)/60) % 60 as VARCHAR),2)
as Spread_Time,

d.PieceClearPlace,
d.PieceClearTime,

case 
when 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceClearTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceClearTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceClearTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceClearTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'p'
END
as Formatted_Piece_Clear_Time,

--Sum of Platform Time is the Total Platform

case when d.PieceStartTime < d.PieceEndTime
then (d.PieceEndTime - d.PieceStartTime) 
else (86400 - (d.PieceStartTime - d.PieceEndTime))
end
as Total_Plat_1,

case when d.PieceReportTime < d.PieceClearTime
then (d.PieceClearTime - d.PieceReportTime) 
else (86400 - (d.PieceReportTime - d.PieceClearTime))
end
as Total_Spread_1

/*
QUERY GOES TO A TEMP TABLE PRIOR TO FINAL PROCESSING
*/
into #Temp_Final


from cte_schedules as s
inner join cte_blocks as b
on s.SchedulingUnit = b.BlockStartPlace

inner join cte_trips as t
on s.VehicleScheduleID = t.VehicleScheduleID
and b.InternalBlockNumber = t.InternalBlockNumber

inner join cte_duties as d
on b.InternalBlockNumber = d.InternalBlockNumber
and t.DutyNumber = d.DutyNumber

--where t.RouteID = '5'
--and t.DirectionID = 1

where
(t.IsTimingPoint = 1
or t.TripTypeName <> 'Regular')

group by
d.DutyNumber,
s.ScheduleTypeName,
b.BlockNumber,
b.TypeCharacter ,
RTRIM(CONCAT(b.BlockNumber, ' ', b.TypeCharacter)),
b.InternalBlockNumber,
t.RouteID,
t.Terminal,
t.DirectionID,
t.TripNumber,
t.TripTypeName,
--case
--when  t.TripTypeName IN ('Pull-out') THEN -1
--when  t.TripTypeName IN ('Pull-in') THEN 99999
--when  t.TripTypeName IN ('Deadhead') THEN (lag(t.tripnumber) over (PARTITION BY b.blocknumber, b.typecharacter, b.internalblocknumber, t.routeid, t.terminal, t.directionid ORDER BY t.PassingTime))+1
--else t.TripNumber
--end ,
t.TripPatternID,
t.FromToVia,
t.ViaVariant,
t.VariantDescription,
t.PlaceSequence,
t.StopSequence,
t.StopID,
t.AlternateName,
t.DriverText,
t.PublicText,
t.PassingTime,

case 
when 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((t.PassingTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((t.PassingTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((t.PassingTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(t.PassingTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((t.PassingTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((t.PassingTime / 60) % 60 AS VARCHAR),2) + 'p'
END
,
d.PieceReportPlace,
d.PieceReportTime,
(d.PieceStartTime - (60 * 10) ),

case 
when 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceReportTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceReportTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceReportTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceReportTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceReportTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceReportTime / 60) % 60 AS VARCHAR),2) + 'p'
end,

d.PieceStartPlace,
d.PieceStartTime,

case 
when 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceStartTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceStartTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceStartTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceStartTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceStartTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceStartTime / 60) % 60 AS VARCHAR),2) + 'p'
END
,

d.PieceEndPlace,
d.PieceEndTime,

case 
when 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceEndTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceEndTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceEndTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceEndTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceEndTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceEndTime / 60) % 60 AS VARCHAR),2) + 'p'
END
,
d.PieceClearPlace,
d.PieceClearTime,

case 
when 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) < 12 
 THEN RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'a'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) = 12
 THEN RIGHT(CAST(((d.PieceClearTime / 3600)) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'p'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) = 24
 THEN RIGHT(CAST(((d.PieceClearTime / 3600)-12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) > 24
 THEN RIGHT(CAST(((d.PieceClearTime / 3600) -24) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'x'
WHEN 
 RIGHT(CAST(d.PieceClearTime / 3600 AS VARCHAR),2) > 12
 THEN RIGHT(CAST(((d.PieceClearTime / 3600) -12) AS VARCHAR),2) +
      RIGHT('0' + CAST((d.PieceClearTime / 60) % 60 AS VARCHAR),2) + 'p'
END
,

RIGHT( CAST((d.PieceEndTime - d.PieceStartTime)/3600 as VARCHAR),2) + 'h' +
RIGHT('0' + CAST(((d.PieceEndTime - d.PieceStartTime)/60) % 60 as VARCHAR),2),

RIGHT( CAST((d.PieceClearTime - d.PieceReportTime)/3600 as VARCHAR),2) + 'h' +
RIGHT('0' + CAST(((d.PieceClearTime - d.PieceReportTime)/60) % 60 as VARCHAR),2),

--Sum of Platform Time is the Total Platform

case when d.PieceStartTime < d.PieceEndTime
then (d.PieceEndTime - d.PieceStartTime) 
else (86400 - (d.PieceStartTime - d.PieceEndTime))
end,

case when d.PieceReportTime < d.PieceClearTime
then (d.PieceClearTime - d.PieceReportTime) 
else (86400 - (d.PieceReportTime - d.PieceClearTime))
end

having 
t.PassingTime <= d.PieceEndTime
-- when a trip passes over Midnight, Hastus correctly recognizes it as a 24h+ trip and assigns an overflow value
-- in Paddle Print, this is denoted with an x after the time. 229x vs 229a
order by t.PassingTime, TripNumSort



SELECT f.*,
DENSE_RANK() OVER (PARTITION BY f.BlockNumber, f.RouteID, f.TripTypeName ORDER BY f.TripNumSort) as TripIndex

from #Temp_Final as f