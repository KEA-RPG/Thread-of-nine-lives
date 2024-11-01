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
            var newDeck = Deck.FromDTO(deckDTO);

            _deckRepository.AddDeck(newDeck);

            deckDTO.Id = newDeck.Id;
            return deckDTO;
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
        public DeckDTO GetDeckById(int id)
        {
            var deck = _deckRepository.GetDeckById(id);
            if (deck == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                var deckDTO = DeckDTO.FromEntity(deck);
                return deckDTO;
            }
        }

        public List<DeckDTO> GetUserDecks(string userName)
        {
            return _deckRepository.GetUserDecks(userName);
        }
        
        
        public Deck UpdateDeck(Deck deck, DeckDTO deckDTO)
        {
            if(deck != null && deckDTO == null)
            {
                // Update the existing deck with the new values
                deck.Name = deckDTO.Name;
                deck.IsPublic = deckDTO.IsPublic;
                deck.DeckCards = deckDTO.Cards.Select(card => new DeckCard
                {
                    CardId = card.Id,
                }).ToList();

                return deck;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public DeckDTO UpdateDeck(DeckDTO deckDTO, string role, string userName)
        {
            Deck existingDeck = null;

            if (userName != null)
            {
                existingDeck = _deckRepository.GetDeckById(deckDTO.Id, userName);

                Deck updatedDeck = UpdateDeck(existingDeck, deckDTO);

                _deckRepository.UpdateDeck(updatedDeck);

                return DeckDTO.FromEntity(updatedDeck);
            }
            else
            {
                var existingDeckAdmin = _deckRepository.GetDeckById(deckDTO.Id);

                Deck updatedDeckAdmin = UpdateDeck(existingDeck, deckDTO);

                _deckRepository.UpdateDeck(updatedDeckAdmin);

                return DeckDTO.FromEntity(updatedDeckAdmin);
            }


        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _deckRepository.GetPublicDecks();
        }

    }
}
