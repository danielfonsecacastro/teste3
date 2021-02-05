using Bari.Domain.Entities;

namespace Bari.Domain.Interfaces
{
    public interface IMessageService
    {
        void Proccess(string body);
        Message Create(string value);
    }
}
