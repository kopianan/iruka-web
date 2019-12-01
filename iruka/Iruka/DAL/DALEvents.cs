using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Iruka.EF.Model.Enum;

namespace Iruka.DAL
{
    public class DALEvents
    {
        public static List<EventDTO> GetAllPendingEvents()
        {
            var events = Global.DB.Event
                .Where(x => x.IsActive && x.EventStatus == EventStatus.Pending)
                .OrderByDescending(x => x.ScheduleDate)
                .ToList();
            var toReturn = new List<EventDTO>();

            foreach (var @event in events)
            {
                var eventDto = Mapper.Map<Event, EventDTO>(@event);
                eventDto.ScheduleDate = Global.DateToString(@event.ScheduleDate);

                toReturn.Add(eventDto);
            }

            return toReturn;
        }

        public static List<EventDTO> GetAllOnGoingEvents()
        {
            var events = Global.DB.Event
                .Where(x => x.IsActive && x.EventStatus == EventStatus.OnGoing)
                .OrderBy(x => x.Priority)
                .ToList();
            var toReturn = new List<EventDTO>();

            foreach (var @event in events)
            {
                var eventDto = Mapper.Map<Event, EventDTO>(@event);
                eventDto.ScheduleDate = Global.DateToString(@event.ScheduleDate);

                toReturn.Add(eventDto);
            }

            return toReturn;
        }

        public static List<EventDTO> GetAllFinishedEvents()
        {
            var events = Global.DB.Event
                .Where(x => x.IsActive && x.EventStatus == EventStatus.Finished)
                .OrderByDescending(x => x.ModifiedDate)
                .ToList();
            var toReturn = new List<EventDTO>();

            foreach (var @event in events)
            {
                var eventDto = Mapper.Map<Event, EventDTO>(@event);
                eventDto.ScheduleDate = Global.DateToString(@event.ScheduleDate);
                eventDto.ModifiedDate = Global.DateToString(@event.ModifiedDate);

                toReturn.Add(eventDto);
            }

            return toReturn;
        }
    }
}