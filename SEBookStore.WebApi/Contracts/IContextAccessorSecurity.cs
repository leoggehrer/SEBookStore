//@CodeCopy
#if ACCOUNT_ON
namespace SEBookStore.WebApi.Contracts
{
    partial interface IContextAccessor
    {
        string SessionToken { set; }
    }
}
#endif
