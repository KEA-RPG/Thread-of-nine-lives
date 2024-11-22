using Domain.DTOs;

namespace Backend.Repositories.Interfaces
{
    public interface ICardRepository
    {
        public CardDTO AddCard(CardDTO card);
        public void DeleteCard(CardDTO card);
        public void UpdateCard(CardDTO card);
        public List<CardDTO> GetAllCards();
        public CardDTO GetCardById(int id);





    }
}
