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

        public void AddComment(CommentDTO commentDto)
        {
            // Check if the deck exists before adding a comment
            var deck = _deckRepository.GetDeckById(commentDto.DeckId);
            if (deck == null)
            {
                throw new KeyNotFoundException($"Deck with ID {commentDto.DeckId} was not found.");
            }

            var comment = CommentDTO.ToEntity(commentDto);
            _deckRepository.AddComment(comment);
        }

        public List<CommentDTO> GetCommentsByDeckId(int deckId)
        {
            // Check if the deck exists before attempting to retrieve comments
            var deck = _deckRepository.GetDeckById(deckId);
            if (deck == null)
            {
                throw new KeyNotFoundException($"Deck with ID {deckId} was not found.");
            }

            // Retrieve comments
            var comments = _deckRepository.GetCommentsByDeckId(deckId);
            if (comments == null || !comments.Any())
            {
                throw new Exception($"No comments found for Deck with ID {deckId}.");
            }

            return comments.Select(comment => CommentDTO.FromEntity(comment)).ToList();
        }



    }
}
