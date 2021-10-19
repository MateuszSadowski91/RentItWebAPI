﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;

namespace RentItAPI.Controllers
{
    [Route("api/business/{businessId}/item/{itemId}/reservation")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPut("{reservationId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult CancelReservation([FromRoute] int reservationId, string message)
        {
            _reservationService.CancelReservation(reservationId, message);
            return Ok();
        }

        [HttpPost]
        public ActionResult MakeReservation([FromRoute] int itemId, [FromBody] MakeReservationDto dto)
        {
            var newReservationId = _reservationService.MakeReservation(itemId, dto);
            return Created($"api/business/businessId/item/{itemId}/reservation/{newReservationId}", null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllReservations([FromQuery] ReservationQuery query)
        {
            var result = _reservationService.GetAll(query);
            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllReservationsInBusiness([FromQuery] ReservationQuery query, [FromRoute] int businessId)
        {
            var result = _reservationService.GetAllForBusiness(query, businessId);
            return Ok(result);
        }
    }
}