using Backend.Repositories;
using Domain.Entities;
using Domain.DTOs;

namespace Backend.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;

        public DeckService(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        public DeckDTO CreateDeck(DeckDTO deck)
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

        public List<DeckDTO> GetUserDecks(string userName)
        {
            return _deckRepository.GetUserDecks(userName);
        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _deckRepository.GetPublicDecks();
        }

        public DeckDTO GetDeckById(int id)
        {
            return _deckRepository.GetDeckById(id);
        }

        public DeckDTO UpdateDeck(DeckDTO deck)
        {
            _deckRepository.UpdateDeck(deck);
            return _deckRepository.GetDeckById(deck.Id);
        }
    }
}
