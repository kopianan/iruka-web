using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iruka.DAL
{
    public class DALEvents
    {
        public static List<EventDTO> GetAllEvent()
        {
            var events = Global.DB.Event.Where(x => x.isActive == true).ToList();
            var eventDTOList = Mapper.Map<List<Event>, List<EventDTO>>(events);

            return eventDTOList;
        }
    }
}