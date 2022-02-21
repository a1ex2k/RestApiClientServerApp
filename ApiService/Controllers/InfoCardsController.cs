using ApiService.DataService;
using ApiService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("infocards")]
    public class InfoCardsController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InfoCard>>> Get()
        {
            var collection = DataProvider.GetCardsAsync().Result;
            return Ok(collection);
        }

        [HttpPost]
        public async Task<ActionResult<InfoCard>> Post(InfoCard infocard)
        {
            bool success = DataProvider.AddNewCardAsync(infocard).Result;
            if (success) return Ok();
            else return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult<InfoCard>> Put(InfoCard infocard)
        {
            bool success = DataProvider.UpdateCardAsync(infocard).Result;
            if (success) return Ok();
            else return NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<InfoCard>> Delete(string id)
        {
            bool success = DataProvider.RemoveCard(id);
            if (success) return Ok();
            else return NotFound();
        }
    }
}
