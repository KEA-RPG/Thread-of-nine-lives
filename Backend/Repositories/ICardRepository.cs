﻿using Domain.DTOs;
using Domain.Entities;

namespace Backend.Repositories
{
    public interface ICardRepository
    {
        public void AddCard(Card card);
        public void DeleteCard(Card card);
        public void UpdateCard(Card card);
        public List<Card> GetAllCards();
        public Card GetCardById(int id);





    }
}
