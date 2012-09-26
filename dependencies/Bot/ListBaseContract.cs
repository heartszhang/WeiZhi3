using System.Diagnostics.Contracts;

namespace Bot
{
    [ContractClassFor(typeof(ListBase<>))]
    abstract class ListBaseContract<T> : ListBase<T>
    {
        protected override T GetItem(int index)
        {
            Contract.Requires(index >= 0);
            Contract.Requires(index < Count);
            return default(T);
        }

        public override int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }
    }
}