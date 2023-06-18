namespace Banks.Models;

public interface ITransaction
{
        IBankAccount AccountFrom { get; }
        IBankAccount AccountTo { get; }
        decimal Money { get; }
        void Rollback();
        void Execute();
}
