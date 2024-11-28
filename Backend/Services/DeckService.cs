using Domain.Entities;
using Domain.DTOs;
using Backend.SecurityLogic;
using Backend.Repositories.Interfaces;
using Backend.Repositories.Relational;

namespace Backend.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        private readonly IUserRepository _userRepository;

        public DeckService(IDeckRepository deckRepository, IUserRepository userRepository)
        {
            _deckRepository = deckRepository;
            _userRepository = userRepository;
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
            // Sanitize the comment before adding
            var sanitizedComment = Sanitizer.Sanitize(commentDto);

            // Check if the deck exists before adding a comment
            var deck = _deckRepository.GetDeckById(sanitizedComment.DeckId);
            if (deck == null)
            {
                throw new KeyNotFoundException($"Deck with ID {sanitizedComment.DeckId} was not found.");
            }

            // Check if the user exists based on the Username
            var user = _userRepository.GetUserByUsername(sanitizedComment.Username);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with username '{sanitizedComment.Username}' was not found.");
            }

            // Set the UserId from the found user
            sanitizedComment.UserId = user.Id;

            // Add the sanitized comment
            _deckRepository.AddComment(sanitizedComment);
        }

        public List<CommentDTO> GetCommentsByDeckId(int deckId)
        {
            // Check if the deck exists before attempting to retrieve comments
            var deck = _deckRepository.GetDeckById(deckId);
            if (deck == null)
            {
                throw new KeyNotFoundException($"Deck with ID {deckId} was not found.");
            }

            // Retrieve and sanitize comments
            var comments = _deckRepository.GetCommentsByDeckId(deckId);
            if (comments == null || !comments.Any())
            {
                throw new Exception($"No comments found for Deck with ID {deckId}.");
            }

            var sanitizedComments = Sanitizer.Sanitize(comments.ToList());
            return sanitizedComments;
        }



    }
}
