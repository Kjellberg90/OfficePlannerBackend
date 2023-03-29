using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.AdminRoomBookingService
{
    public interface IAdminRoomBookingService
    {
        public void UpdateBookings(UpdateBookingsDTO[] updateBookings, string date);
        public List<GroupBookedToRoom> GetBookingsForRoom();
        public void PostGroupToRoomBooking(GroupToRoomBookingDTO postGroupToRoomDTO);
        public void DeleteGroupToRoomBooking(int bookingId);
        public void DeleteOldSingleRoomBookings();
        public void EditGroupToRoomBooking(int bookingId, GroupToRoomBookingDTO groupToRoomBooking);
        public void RefreshBookings();
    }
}
