﻿using DAL.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IScheduleService
    {
        public int GetWeeksTotal(int scheduleId);
        public IEnumerable<SQLSchedule> GetSchedules(); 
    }
}
