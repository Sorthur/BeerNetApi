using BeerNetApi.Managers;
using BeerNetApi.Models;
using BeerNetApi.Models.PostModels;
using BeerNetApi.SwaggerExamples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IBreweriesManager _breweriesManager;
        private readonly UserManager<BeerNetUser> _userManager;

        public BeersController(IBeersManager beerManager, IBeerRatesManager beerRatesManager, IBreweriesManager breweriesManager, UserManager<BeerNetUser> userManager)
        {
            _beerManager = beerManager;
            _beerRatesManager = beerRatesManager;
            _breweriesManager = breweriesManager;
            _userManager = userManager;
        }

        /// <summary>
        ///     Returns beer. Brewery information and all beer rates included
        /// </summary>
        /// <response code="200">Found matching beer</response> 
        /// <response code="204">Beer was not found</response>
        [HttpGet("{beerId}")]
        [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
        public IActionResult Get(int beerId)
        {
            var beer = _beerManager.GetBeer(beerId);
            if (beer != null)
            {
                return Ok(beer);
            }
            return NoContent();
        }

        /// <summary>
        ///     Returns list of matching beers. Brewery information included, beer rates ignored
        /// </summary>
        /// <response code="200">Found matching beers</response> 
        /// <response code="204">No beers were found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<BeersExample>), StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery] BeerFilter beerFilter)
        {
            var beers = _beerManager.GetBeers(beerFilter);
            if (beers != null)
            {
                return Ok(beers);
            }
            return NoContent();
        }


        /// <summary>
        ///     Looks for brewery in db for given id and then adds created beer to db
        /// </summary>
        /// <response code="204">Adding beer was succesful</response>
        /// <response code="400">Wrong breweryId</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Post(BeerPostModel model)
        {
            var beer = (Beer)model;
            var brewery = _breweriesManager.GetBrewery(model.BreweryId.Value);
            if (brewery == null)
            {
                return BadRequest($"Brewery with id={model.BreweryId} not found");
            }
            _beerManager.AddBeer(beer);
            brewery.Beers.Add(beer);
            _breweriesManager.UpdateBrewery(brewery);
            return NoContent();
        }

        /// <summary>
        /// Returns number of matching beers
        /// </summary>
        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfBeers([FromQuery] BeerFilter beerFilter)
        {
            return Ok(_beerManager.GetNumberOfBeers(beerFilter));
        }

        /// <summary>
        /// Returns specific beer rate. User and beer information ignored
        /// </summary>
        /// <response code="200">Found matching beer rate</response>
        /// <response code="400">Wrong beerRateId</response>
        [HttpGet]
        [Route("rate/{beerRateId}")]
        [ProducesResponseType(typeof(BeerRateExample), StatusCodes.Status200OK)]
        public IActionResult GetBeerRate(int beerRateId)
        {
            var beerRate = _beerRatesManager.GetBeerRate(beerRateId);
            if (beerRate != null)
            {
                return Ok(beerRate);
            }
            return BadRequest($"Beer rate with id={beerRateId} not found");
        }

        /// <summary>
        /// Adds new beer rate for currently logged user. Only one rate per beer is allowed
        /// </summary>
        /// <response code="200">Beer added successfully</response>
        /// <response code="405">User already rated this beer</response>
        [HttpPost]
        [Authorize]
        [Route("rate/{beerId}")]
        public IActionResult Post(int beerId, [FromBody] BeerRatePostModel beerRatePostModel)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var userId = _userManager.FindByEmailAsync(email).Result.Id;

            if (_beerRatesManager.DidUserRateBeer(beerId, userId))
            {
                return StatusCode(405, "User already rated this beer");
            }

            _beerRatesManager.AddBeerRate(beerId, userId, beerRatePostModel.Description, beerRatePostModel.Rate.Value);
            return NoContent();
        }

        /// <summary>
        /// Updates description and/or rate in existing beer rate
        /// </summary>
        /// <response code="200">Beer added successfully</response>
        [HttpPut]
        [Authorize]
        [Route("rate/{beerRateId}")]
        public IActionResult Put(int beerRateId, [FromBody] BeerRatePostModel beerRatePostModel)
        {
            _beerRatesManager.EditBeerRate(beerRateId, beerRatePostModel.Description, beerRatePostModel.Rate.Value);
            return NoContent();
        }


        /// <summary>
        /// For development purposes only
        /// </summary>
        [HttpPost("gor")]
        [Authorize(Roles = UserRoles.Admin)]
        internal IActionResult Post(Beer newBeer)
        {
            _beerManager.DodajListeLosowychPiw();
            throw new NotImplementedException("Method yet to do");
        }
    }
}
