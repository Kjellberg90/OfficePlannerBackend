﻿using DAL.Models;
using System.Data;

namespace DAL
{
    public class MockData
    {
        public IEnumerable<Group> _groups { get; private set; }
        public IEnumerable<Room> _rooms { get; private set; }
        public IEnumerable<Booking> _newBookings { get; private set; }
        
        private static MockData _instance;

        public static MockData Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new MockData();
                }
                return _instance;
            }
        }


        public MockData()
        {
            _groups = new List<Group>
            {
                new Group { Id = 1, Name = "DQT", TeamMembers = 6, Division = "A"},
                new Group { Id = 2, Name = "Phoenix", TeamMembers = 4, Division = "A"},
                new Group { Id = 3, Name = "Bazinga", TeamMembers = 4, Division = "A"},
                new Group { Id = 4, Name = "ConSys", TeamMembers = 4, Division = "A"},
                new Group { Id = 5, Name = "PAST", TeamMembers = 6, Division = "B"},
                new Group { Id = 6, Name = "Heimdall", TeamMembers = 6, Division = "B"},
                new Group { Id = 7, Name = "Battery", TeamMembers = 5, Division = "B"},
                new Group { Id = 8, Name = "ConCor", TeamMembers = 7, Division = "C"},
                new Group { Id = 9, Name = "Everest", TeamMembers = 6, Division = "C"},
                new Group { Id = 10, Name = "Portal", TeamMembers = 2, Division = "C"}

            };

            _rooms = new List<Room>
            {
                new Room { ID = 1, Name = "United", Seats = 6},
                new Room { ID = 2, Name = "Innovation", Seats = 8},
                new Room { ID = 3, Name = "Committment", Seats = 6},
                new Room { ID = 4, Name = "Collaboration", Seats = 6},
                new Room { ID = 5, Name = "Inspired", Seats = 8}
            };
        }

        
        public IEnumerable<Group> GetGroupsInfo()
        {
            return _groups;
        }

        public IEnumerable<Room> GetRoomsInfo()
        {
            return _rooms;
        }
    }
}