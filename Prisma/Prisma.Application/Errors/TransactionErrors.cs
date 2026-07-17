using Prisma.Domain.Models;

namespace Prisma.Application.Errors;

public static class TransactionErrors
{
    public static readonly Error Failed = new( "TRANSACTION.FAILED", "Ocorreu um erro ao executar a transação.", ErrorType.Failure);
}
