using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{

    [Produces("application/json")]
    //[Route("api/Wibble")]
    public class WibbleController : ODataController
    {
        private readonly HealthContext _context;
        
        public WibbleController(HealthContext context)
        {
            _context = context;
            
        }

       // [HttpGet]
        [EnableQuery]
        public IQueryable<Models.Weight> Get()
        {
            return _context.Weights.AsQueryable();
        }


    }
}
