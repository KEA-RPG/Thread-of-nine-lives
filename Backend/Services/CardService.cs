using Domain.Entities;
using Domain.DTOs;
using System.CodeDom;
using Backend.Repositories.Interfaces;

namespace Backend.Services
{
    public class CardService : ICardService
    {

        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public CardDTO CreateCard(CardDTO cardDTO)
        {
            var dbCard = _cardRepository.AddCard(cardDTO);

            cardDTO.Id = dbCard.Id;

            return cardDTO;
        }

        public void DeleteCard(int id)
        {
            var card = _cardRepository.GetCardById(id);
            _cardRepository.DeleteCard(card);

        }

        public List<CardDTO> GetAllCards()
        {
            var cards = _cardRepository.GetAllCards();
            return cards;
        }

        public CardDTO GetCardById(int id)
        {
            var card = _cardRepository.GetCardById(id);
            if (card != null)
            {

                return card;
            }
            else
            {
                throw new KeyNotFoundException("No card by that ID");
            }
        }

        public CardDTO UpdateCard(CardDTO cardDTO)
        {

            var existingCard = _cardRepository.GetCardById(cardDTO.Id);
            if(existingCard == null)
            {
                throw new KeyNotFoundException("No card by that ID");
            }

            existingCard.Name = cardDTO.Name;
            existingCard.Description = cardDTO.Description;
            existingCard.Attack = cardDTO.Attack;
            existingCard.Defence = cardDTO.Defence;
            existingCard.Cost = cardDTO.Cost;
            existingCard.ImagePath = cardDTO.ImagePath;


            _cardRepository.UpdateCard(existingCard);
            return existingCard;
        }
    }
}
