using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDBContext cardsDBContext;
        public CardsController(CardsDBContext cardsDBContext)
        {
            this.cardsDBContext = cardsDBContext;
        }

        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDBContext.Cards.ToListAsync();
            return Ok(cards);

        }
        //Get single card
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await cardsDBContext.Cards.FirstOrDefaultAsync(x=>x.Id==id);
            if (card != null)
            {
                return Ok(card); 
            }
            return NotFound("Card not Found");

        }
        //Add Card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();
            await cardsDBContext.Cards.AddAsync(card);
            await cardsDBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard),new { id = card.Id },card);
        }
        //Updating A card 
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard ([FromRoute] Guid id,[FromBody] Card card)
        {
            var existingCard = await cardsDBContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(existingCard != null)
            {
                existingCard.CardholderName = card.CardholderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.CVC=card.CVC;
                await cardsDBContext.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Card not Found");
        }

        //Delete A card 
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingCard = await cardsDBContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                cardsDBContext.Remove(existingCard);
                await cardsDBContext.SaveChangesAsync();
                return Ok(existingCard);
            }

            return NotFound("Card not Found");
        }
    }
}
