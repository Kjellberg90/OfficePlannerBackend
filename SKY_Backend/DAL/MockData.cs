using DAL.Models;
using System.Data;

namespace DAL
{
    public class MockData
    {
        public IEnumerable<Group> _groups { get; private set; }
        public IEnumerable<Room> _rooms { get; private set; }
        public IEnumerable<OldBooking> _bookings { get; private set; }
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
                new Group { Id = 1, Name = "DQT", TeamMembers = 6},
                new Group { Id = 2, Name = "Phoenix", TeamMembers = 4},
                new Group { Id = 3, Name = "Bazinga", TeamMembers = 4},
                new Group { Id = 4, Name = "ConSys", TeamMembers = 4},
                new Group { Id = 5, Name = "PAST", TeamMembers = 6},
                new Group { Id = 6, Name = "Heimdall", TeamMembers = 6},
                new Group { Id = 7, Name = "Battery", TeamMembers = 5},
                new Group { Id = 8, Name = "ConCor", TeamMembers = 7},
                new Group { Id = 9, Name = "Everest", TeamMembers = 6},
                new Group { Id = 10, Name = "Portal", TeamMembers = 2}

            };

            _rooms = new List<Room>
            {
                new Room { ID = 1, Name = "United", Seats = 6, BookedBy = 9},
                new Room { ID = 2, Name = "Innovation", Seats = 8, BookedBy = 8},
                new Room { ID = 3, Name = "Committment", Seats = 6, BookedBy = 2},
                new Room { ID = 4, Name = "Collaboration", Seats = 6, BookedBy = 1},
                new Room { ID = 5, Name = "Inspired", Seats = 6, BookedBy = 3}
            };

            _bookings = new List<OldBooking>
            {
                new OldBooking {Id = 1, DayNr = 1, Collaboration = 3, Committment = 7, Innovation = 6, InspiredA = 2, InspiredB = 4, United = 1 },
                new OldBooking {Id = 2, DayNr = 2, Collaboration = 3, Committment = 7, Innovation = 6, InspiredA = 2, InspiredB = 4, United = 1 },
                new OldBooking {Id = 3, DayNr = 3, Collaboration = 9, Committment = 10, Innovation = 5, InspiredA = 8, InspiredB = 8, United = null},
                new OldBooking {Id = 4, DayNr = 4, Collaboration = 9, Committment = 10, Innovation = 5, InspiredA = 8, InspiredB = 8, United = null},
                new OldBooking {Id = 5, DayNr = 5, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 6, DayNr = 6, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 7, DayNr = 7, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 8, DayNr = 8, Collaboration = 10, Committment = 2, Innovation = 9, InspiredA = 7, InspiredB = 7, United = 3},
                new OldBooking {Id = 9, DayNr = 9, Collaboration = 10, Committment = 2, Innovation = 9, InspiredA = 7, InspiredB = 7, United = 3},
                new OldBooking {Id = 10, DayNr = 10, Collaboration = 1, Committment = 7, Innovation = 5, InspiredA = 6, InspiredB = 6, United = 4},
                new OldBooking {Id = 11, DayNr = 11, Collaboration = 1, Committment = 7, Innovation = 5, InspiredA = 6, InspiredB = 6, United = 4},
                new OldBooking {Id = 12, DayNr = 12, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 13, DayNr = 13, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 14, DayNr = 14, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 15, DayNr = 15, Collaboration = 9, Committment = 7, Innovation = 5, InspiredA = 6, InspiredB = 6, United = 10},
                new OldBooking {Id = 16, DayNr = 16, Collaboration = 9, Committment = 7, Innovation = 5, InspiredA = 6, InspiredB = 6, United = 10},
                new OldBooking {Id = 17, DayNr = 17, Collaboration = 2, Committment = 4, Innovation = 1, InspiredA = 8, InspiredB = 8, United = 3},
                new OldBooking {Id = 18, DayNr = 18, Collaboration = 2, Committment = 4, Innovation = 1, InspiredA = 8, InspiredB = 8, United = 3},
                new OldBooking {Id = 19, DayNr = 19, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 20, DayNr = 20, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null},
                new OldBooking {Id = 21, DayNr = 21, Collaboration = null, Committment = null, Innovation = null, InspiredA = null, InspiredB = null, United = null}
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

        public IEnumerable<OldBooking> GetBookingsInfo()
        {
            return _bookings;
        }
    }
}