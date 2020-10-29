using BeerNetApi.Managers;
using BeerNetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly IBeersManager _beerManager;
        private readonly IBeerRatesManager _beerRatesManager;

        public BeersController(IBeersManager beerManager, IBeerRatesManager beerRatesManager)
        {
            _beerManager = beerManager;
            _beerRatesManager = beerRatesManager;
        }

        public class PostBeerRateModel
        {
            public string UserId { get; set; }
            public BeerRate BeerRate { get; set; }
        };

        /// <returns>
        /// HTTP 200 with json with one beer if it exists. Brewery information and all beer rates included. <br></br>
        /// or HTTP 204 if beer with given id does not exist
        /// </returns>
        [HttpGet("{beerId}")]
        public IActionResult Get(int beerId)
        {
            var beer = _beerManager.GetBeer(beerId);
            if (beer != null)
            {
                return Ok(beer);
            }
            return NoContent();
        }

        /// <returns>
        /// HTTP 200 with json with list of beers. Brewery information included, beer rates ignored. <br></br>
        /// or HTTP 204 if beer with given id does not exist
        /// </returns>
        [HttpGet]
        public IActionResult Get([FromQuery] BeerFilter beerFilter)
        {
            var beers = _beerManager.GetBeers(beerFilter);
            if (beers != null)
            {
                return Ok(beers);
            }
            return NoContent();
        }

        /// <returns>
        /// Number of beers for given filter
        /// </returns>
        [HttpGet]
        [Route("count")]
        public IActionResult GetNumberOfBeers([FromQuery] BeerFilter beerFilter)
        {
            return Ok(_beerManager.GetNumberOfBeers(beerFilter));
        }

        /// <returns>
        /// HTTP 200 with json with specific beer rate. User and beer information ignored. <br></br>
        /// or HTTP 204 if no beer rate was found
        /// </returns>
        [HttpGet]
        [Route("rate/{beerRateId}")]
        public IActionResult GetBeerRate(int beerRateId)
        {
            var beerRate = _beerRatesManager.GetBeerRate(beerRateId);
            if (beerRate != null)
            {
                return Ok(beerRate);
            }
            return NoContent();
        }

        /// <summary>
        /// Adds new beer rate
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("rate/{beerId}")]
        public IActionResult Post(int beerId, string userId, string description, float rate)
        {
            _beerRatesManager.AddBeerRate(beerId, userId, description, rate);
            return NoContent();
        }

        /// <summary>
        /// Updates description and/or rate in existing beer rate
        /// </summary>
        [HttpPut]
        [Authorize]
        [Route("rate/{beerId}")]
        public IActionResult Put(int beerId, [FromBody] PostBeerRateModel model)
        {
            _beerRatesManager.EditBeerRate(model.BeerRate, beerId, model.UserId);
            return NoContent();
        }


        /// <summary>
        /// For development purposes only
        /// </summary>
        [HttpPost("gor")]
        public IActionResult Post(Beer newBeer)
        {
            _beerManager.DodajListeLosowychPiw();
            throw new NotImplementedException("Method yet to do");
        }
    }
}
