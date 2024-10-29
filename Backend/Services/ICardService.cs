using Domain.DTOs;

namespace Backend.Services
{
    public interface ICardService
    {
        CardDTO GetCardById(int id);
        List<CardDTO> GetAllCards();
        CardDTO CreateCard(CardDTO card);
        CardDTO UpdateCard(CardDTO card);
        IResult DeleteCard(int id);
    }
}