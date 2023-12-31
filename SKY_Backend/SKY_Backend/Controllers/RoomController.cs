﻿using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.RoomService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-rooms-info")]
        public IActionResult GetRoomsInfo(string date)
        {
            try
            {
                var result = _roomService.GetRoomsInfo(date);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
