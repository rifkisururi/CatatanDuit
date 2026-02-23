namespace CatatanDuit.Api.Models;

public enum WalletType
{
    Savings,
    Checking,
    EWallet,
    Cash
}

public enum CategoryType
{
    Income,
    Expense
}

public enum TransactionType
{
    Debit,
    Credit
}

public enum TransactionSource
{
    Web,
    Wa,
    Job
}
