namespace DomainModel.Services
{
    using System;
    using DomainModel.Entities;

    public interface IOrderSubmitter
    {
        void SubmitOrder(Cart cart);
    }
}
