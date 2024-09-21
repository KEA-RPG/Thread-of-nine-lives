using Domain.Entities;

namespace Backend.Services
{
    public interface ICardService
    {
        Card GetCardById(int id);
        List<Card> GetAllCards();
        Card CreateCard(Card card);
        Card UpdateCard(Card card);
        void DeleteCard(int id);
    }
}