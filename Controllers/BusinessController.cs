using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Entities;
using RentItAPI.Models;
using RentItAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentItAPI.Controllers
{

    
    [Route("api/business")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateBusiness([FromBody] CreateBusinessDto dto)
        {
            var id = _businessService.Create(dto);
            return Created($"api/business/{id}", null);
        }
        [HttpGet("{businessId}")]
        public ActionResult GetBusinessById([FromRoute] int businessId)
        {
            var business = _businessService.GetById(businessId);
            return Ok(business);
        }
        [HttpGet]
        public ActionResult GetAllBusinesses()
        {
            var businesses = _businessService.GetAll();
            return Ok(businesses);
        }
    }
}
