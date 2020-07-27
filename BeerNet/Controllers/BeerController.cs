using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerNet.Data;
using BeerNet.Managers;
using BeerNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeerNet.Controllers
{
    public class BeerController : Controller
    {
        private readonly IBeerManager _beerManager;

        public BeerController(IBeerManager beerManager)
        {
            _beerManager = beerManager;
        }
        

    }
}
