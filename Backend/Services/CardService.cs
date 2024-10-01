
using Backend.Repositories;
using Domain.Entities;

namespace Backend.Services
{
    public class CardService : ICardService
    {

        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public Card CreateCard(Card card)
        {
            _cardRepository.AddCard(card);

            return card;
        }

        public void DeleteCard(int id)
        {
            var card = _cardRepository.GetCardById(id);
            if(card == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                _cardRepository.DeleteCard(card);
            }
        }

        public List<Card> GetAllCards()
        {
            return _cardRepository.GetAllCards();
        }

        public Card GetCardById(int id)
        {
            return _cardRepository.GetCardById(id);
        }

        public Card UpdateCard(Card card)
        {
            _cardRepository.UpdateCard(card);
            return _cardRepository.GetCardById(card.Id);
        }
    }
}
