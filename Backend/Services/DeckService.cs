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

        public DeckDTO CreateDeck(DeckDTO deckDTO)
        {
            var addedDeck = _deckRepository.AddDeck(deckDTO);

            return addedDeck;
        }

        public void DeleteDeck(int id)
        {
            var deck = _deckRepository.GetDeckById(id);
            if (deck == null)
            {
                throw new KeyNotFoundException();
            }

            _deckRepository.DeleteDeck(deck.Id);

        }
        public DeckDTO GetDeckById(int id)
        {
            var deckDTO = _deckRepository.GetDeckById(id);
            if (deckDTO == null)
            {
                throw new KeyNotFoundException();
            }
            return deckDTO;

        }

        public List<DeckDTO> GetUserDecks(string userName)
        {
            return _deckRepository.GetUserDecks(userName);
        }


        public DeckDTO UpdateDeck(DeckDTO deckDTO)
        {

            _deckRepository.UpdateDeck(deckDTO);

            return _deckRepository.GetDeckById(deckDTO.Id);
        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _deckRepository.GetPublicDecks();
        }

    }
}
