using Backend.Repositories;
using Domain.Entities;

namespace Backend.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;

        public DeckService(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        public Deck CreateDeck(Deck deck)
        {
            _deckRepository.AddDeck(deck);

            return deck;
        }

        public void DeleteDeck(int id)
        {
            var deck = _deckRepository.GetDeckById(id);
            if (deck == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                _deckRepository.DeleteDeck(deck);
            }
        }

        public List<Deck> GetUserDecks(int id)//TODO: parse user id
        {
            return _deckRepository.GetUserDecks(id);
        }

        public Deck GetDeckById(int id)
        {
            return _deckRepository.GetDeckById(id);
        }

        public Deck UpdateDeck(Deck deck)
        {
            _deckRepository.UpdateDeck(deck);
            return _deckRepository.GetDeckById(deck.Id);
        }
    }
}
