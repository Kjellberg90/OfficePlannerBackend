﻿using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IGroupService
    {
        public IEnumerable<string> GetGroups(); 
    }
}