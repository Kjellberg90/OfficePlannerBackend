using DAL;
using DAL.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoomService : IRoomService
    {
        private readonly IRoomAccess _roomAccess;
        private readonly IBookingAccess _bookingAccess;
        private readonly IDateConverter _dateConverter;
        private readonly IGroupAccess _groupAccess;

        public RoomService(IRoomAccess roomAccess, IBookingAccess bookingAccess, IDateConverter dateConverter, IGroupAccess groupAccess)
        {
            _roomAccess = roomAccess;
            _bookingAccess = bookingAccess;
            _dateConverter = dateConverter;
            _groupAccess = groupAccess;
        }

        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);
            var booking = _bookingAccess.ReadBookingsData()
                .Where(b => b.DayNr == dayNr)
                .FirstOrDefault();

            var roomsList = booking.Rooms;

            var roomInfoList = new List<RoomInfoDTO>();

            foreach (var room in roomsList)
            {

                var group = _groupAccess.ReadGroupsData()
                    .Where(g => g.Id == room.BookedBy)
                    .FirstOrDefault();
                
                int groupSize;
                string groupName;
                
                if (group == null)
                {
                    groupSize = 0;
                    groupName = string.Empty;
                }
                else
                {
                    groupSize = group.TeamMembers;
                    groupName = group.Name;
                }


                roomInfoList.Add(new RoomInfoDTO
                {
                    Name = room.Name,
                    Seats = room.Seats,
                    AvailableSeats = room.Seats - groupSize,
                    GroupName = groupName
                });
            }

            return roomInfoList.OrderBy(x => x.Name);
        }
    }
}
