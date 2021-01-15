using BeerNetApi.Managers;
using BeerNetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeerNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly IBeersManager _beerManager;
        private readonly IBeerRatesManager _beerRatesManager;
        private readonly UserManager<BeerNetUser> _userManager;

        public BeersController(IBeersManager beerManager, IBeerRatesManager beerRatesManager, UserManager<BeerNetUser> userManager)
        {
            _beerManager = beerManager;
            _beerRatesManager = beerRatesManager;
            _userManager = userManager;
        }

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
        /// Adds new beer rate for currently logged user
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("rate/{beerId}")]
        public IActionResult Post(int beerId, [FromBody] PostBeerRateModel postBeerRateModel)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var userId = _userManager.FindByEmailAsync(email).Result.Id;

            _beerRatesManager.AddBeerRate(beerId, userId, postBeerRateModel.Description, postBeerRateModel.Rate.Value);
            return NoContent();
        }

        /// <summary>
        /// Updates description and/or rate in existing beer rate
        /// </summary>
        [HttpPut]
        [Authorize]
        [Route("rate/{beerRateId}")]
        public IActionResult Put(int beerRateId, [FromBody] PostBeerRateModel postBeerRateModel)
        {
            _beerRatesManager.EditBeerRate(beerRateId, postBeerRateModel.Description, postBeerRateModel.Rate.Value);
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
