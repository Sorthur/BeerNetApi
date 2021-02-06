using System;
using System.Collections.Generic;
using System.Linq;
using BeerNetApi.Data;
using BeerNetApi.Models;
using BeerNetApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BeerNetApi.Managers
{
    public class BeersManager : IBeersManager
    {
        private readonly ApplicationDbContext _dbContext;
        public BeersManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetNumberOfBeers(BeerFilter beerFilter)
        {
            return _dbContext.Beers
                .Where(b =>
                (string.IsNullOrEmpty(beerFilter.BeerName) || b.Name.Contains(beerFilter.BeerName)) &&
                (string.IsNullOrEmpty(beerFilter.BreweryName) || b.Brewery.Name.Contains(beerFilter.BreweryName)) &&
                (beerFilter.Country == null || b.Brewery.Country == beerFilter.Country) &&
                (beerFilter.BeerStyle == null || b.Style == beerFilter.BeerStyle) &&
                (beerFilter.ExtractFrom == null || b.Extract >= beerFilter.ExtractFrom) &&
                (beerFilter.ExtractTo == null || b.Extract <= beerFilter.ExtractTo) &&
                (beerFilter.AbvFrom == null || b.Abv >= beerFilter.AbvFrom) &&
                (beerFilter.AbvTo == null || b.Abv <= beerFilter.AbvTo))
                .Count();
        }

        public Beer GetBeer(int id)
        {
            return _dbContext.Beers
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == id);
        }

        public List<Beer> GetBeers(BeerFilter beerFilter)
        {
            if (beerFilter.Limit <= 0)
            {
                return null;
            }

            var beers = _dbContext.Beers
                .Skip(beerFilter.Offset)
                .Take(beerFilter.Limit)
                .Where(b =>
                (string.IsNullOrEmpty(beerFilter.BeerName) || b.Name.Contains(beerFilter.BeerName)) &&
                (string.IsNullOrEmpty(beerFilter.BreweryName) || b.Brewery.Name.Contains(beerFilter.BreweryName)) &&
                (beerFilter.Country == null || b.Brewery.Country == beerFilter.Country) &&
                (beerFilter.BeerStyle == null || b.Style == beerFilter.BeerStyle) &&
                (beerFilter.ExtractFrom == null || b.Extract >= beerFilter.ExtractFrom) &&
                (beerFilter.ExtractTo == null || b.Extract <= beerFilter.ExtractTo) &&
                (beerFilter.AbvFrom == null || b.Abv >= beerFilter.AbvFrom) &&
                (beerFilter.AbvTo == null || b.Abv <= beerFilter.AbvTo))
                .Include(b => b.Brewery)
                .ToList();

            foreach (var beer in beers)
            {
                beer.Brewery.Beers.Clear();
            }
            return beers;
        }

        public List<Beer> GetTopBeers(int numberOfBeers, int minNumberOfRatings)
        {
            return _dbContext.Beers
                .Where(b => b.BeerRates.Count() >= minNumberOfRatings)
                .OrderByDescending(b => b.AverageRating)
                .Take(numberOfBeers)
                .Include(b => b.Brewery)
                //.Include(b => b.BeerRates)
                //    .ThenInclude(b => b.User)
                .ToList();
        }

        public void AddBeer(Beer beer)
        {
            _dbContext.Beers.Add(beer);
            _dbContext.SaveChanges();
        }

        public void DodajListeLosowychPiw()
        {
            var pinta = new Brewery()
            {
                Name = "Pinta",
                Country = Country.Poland,
                Description = "PINTA – We brew from the very first day of craft revolution in Poland! Our great journey started on 28th of March 2011, when we brewed our first batch of „Atak Chmielu” („Attack of the Hops”) – first commercial American IPA in Poland. Since that day we are still brewing new ales, lagers, sahti and sour beers. Alone or in collaboration with our friends from polish and foreign breweries."
            };
            var beers = new List<Beer>()
            {
                new Beer
                {
                    Name = "PINTA Risfactor Double BA Bourbon & Rum",
                    Brewery = pinta,
                    Style=BeerStyle.Imperial_Stout,
                    Extract=30f,
                    Abv=13f,
                    Description = "PINTA Russian Imperial Stout Double BA - to czarny jak noc, mocno palony, czekoladowy stout o gęstej konsystencji i aromacie będącym mariażem dwóch destylatów. \n\n Ponad tona chlebowego i ciastkowego słodu bazowego oraz ćwierć tony najlepszych, angielskich słodów palonych. Kilka miesięcy leżakowania w niskich temperaturach. Maturacja w dwóch rodzajach beczek dla nadania posmaków i aromatów trunków bazowych oraz charakterystycznego profilu dębiny. \n\n Double Barrel Aged Bourbon & Rum oznacza, że piwo po okresie leżakowania zostało przetoczone najpierw do beczek po amerykańskim Bourbonie Jack Daniel’s, a następnie do beczek po jamajskim Rumie.Oba rodzaje beczek wykonane są z dębu amerykańskiego, który nadaje piwu soczyste nuty orzechów, karmelu i wanilii.",
                BeerRates = new List<BeerRate>(),
                AverageRating=0
                },
                new Beer
                {
                    Name = "PINTA Imperator Bałtycki",
                    Brewery = pinta,
                    Style=BeerStyle.Baltic_Porter,
                    Extract=24.7f,
                    Abv=9.1f,
                    Description = "Porter Bałtycki - ciemny, mocny lager to piwowarski skarb Polski. W klasycznej wersji - o ekstrakcie ok. 20°Plato - warzony jest z powodzeniem przez kilka polskich browarów. Jako PINTA doceniamy wartość Portera Bałtyckiego, ale po swojemu robimy krok naprzód. Przedstawiamy Imperatora Bałtyckiego - ciemnego giganta ze słodu i chmielu. PINTA Imperator Bałtycki - piwo od święta.",
                    BeerRates = new List<BeerRate>(),
                    AverageRating=0
                },
                new Beer
                {
                    Name = "PINTA Kwas Theta BA",
                    Brewery = pinta,
                    Style = BeerStyle.Lambic,
                    Extract =24.7f,
                    Abv =12f,
                    Description = "PINTA Kwas Theta BA - Russian Imperial Stout zakwaszony bakteriami kwasu mlekowego oraz przefermentowany drożdżami górnej fermentacji. W kolejnym etapie połączony z sokiem z polskich, kwaśnych wiśni dla nadania owocowego profilu. Następnie przebity do dębowych beczek, gdzie został poddany działaniu dzikich drożdży przez okres sześciu miesięcy. W efekcie pierwsze skrzypce gra wyraźna kwasowość o charakterze mlekowo-owocowym. Na drugim planie kombinacja akcentów palonych, gorzkiej czekolady i brettowego ,,funky'' wspartego przez posmakami drewna dębowego, wanilii i tanin.",
                    BeerRates = new List<BeerRate>(),
                    AverageRating = 0
                },
                new Beer
                {
                    Name = "PINTA Risfactor",
                    Brewery = pinta,
                    Style = BeerStyle.Imperial_Stout,
                    Extract =30f,
                    Abv =11.5f,
                    Description = "PINTA RISFACTOR 30° - ponad tona chlebowego i ciastkowego słodu bazowego na warkę. Ponad ćwierć tony najlepszych, angielskich słodów palonych. Kilka miesięcy leżakowania w niskich temperaturach. W efekcie powstał klasyczny, czarny jak noc Imperialny Stout o gęstej konsystencji i aromacie czekolady, kakao, belgijskich pralin i cukierków toffi.",
                    BeerRates = new List<BeerRate>(),
                    AverageRating = 0
                },
                new Beer
                {
                    Name = "PINTA Modern Drinking",
                    Brewery = pinta,
                    Style = BeerStyle.American_IPA,
                    Extract =15.5f,
                    Abv = 6.4f,
                    Description = "PINTA Modern Drinking 15,5* - tym razem dzielimy się z Wami wiedzą na temat West Coast IPA. Podczas wrześniowego chmielobrania w Yakima (USA), piwowarzy z Portland powtarzali w kółko jedną zasadę. Właśnie ją wdrożyliśmy - i warząc naszą wersję klasyka z Zachodniego Wybrzeża, ręka nam nie zadrżała. Do kotła wpakowaliśmy niemiłosierną ilość aromatycznego chmielu Mosaic™ - prawie w całości podczas ostatnich 15 minut gotowania. A co tam. Drugie tyle zużyliśmy na intensywne chmielenie na zimno. \n PINTA Modern Drinking 15,5* - bądź na czasie!",
                    BeerRates = new List<BeerRate>(),
                    AverageRating = 0
                },
                new Beer
                {
                    Name = "PINTA Hopus Pokus",
                    Brewery = pinta,
                    Style = BeerStyle.Specialty_IPA,
                    Extract =16.5f,
                    Abv =6.8f,
                    Description = "PINTA Hop Pokus 16,5° - czary mary, hokus pokus! Najpierw wyczarowujemy iglasty i cytrusowy aromat chmielu zbieranego przez Amerykanów. Potem łączymy treściwość słodów jasnych z kawowymi i czekoladowymi nutami słodów palonych - to od Niemców. Finiszujemy poważną goryczką, mocniejszą niż w większości klasycznych piw ciemnych - dzięki chmielarzom z Lublina. I powtarzamy wszystko jeszcze raz. PINTA Hop Pokus - czarujące!",
                    BeerRates = new List<BeerRate>(),
                    AverageRating = 0
                },
            };

            _dbContext.Breweries.Add(pinta);
            _dbContext.Beers.AddRange(beers);
            _dbContext.SaveChanges();
            var beer = _dbContext.Beers.Where(b => b.Extract == 30f).FirstOrDefault();
            if (beer != null)
            {
                var rate1 = new BeerRate()
                {
                    Beer = beer,
                    Description = "Wspaniałe piwo",
                    Rate = 4.5f,
                    User = _dbContext.BeerNetUsers.FirstOrDefault()
                };
                var rate2 = new BeerRate()
                {
                    Beer = beer,
                    Description = "Okrutnik. Nie polecam",
                    Rate = 1.1f,
                    User = _dbContext.BeerNetUsers.FirstOrDefault()
                };
                beer.BeerRates.Add(rate1);
                beer.BeerRates.Add(rate2);
                beer.AverageRating = (rate1.Rate + rate2.Rate) / 2;
                _dbContext.SaveChanges();
            }
        }
    }
}
