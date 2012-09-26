using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Bot
{
    internal class CastList<TFrom, TTo> : ListBase<TTo>
        where TFrom : TTo
    {
        /// <summary>
        ///     Creats a new <see cref="CastList{TFrom,TTo}"/>.
        /// </summary>
        /// <param name="from">The source collection.</param>
        public CastList(IList<TFrom> from)
        {
            Contract.Requires(from != null);
            m_source = from;
        }

        /// <summary>
        ///     Gets the element at the specified index.
        /// </summary>
        protected override TTo GetItem(int index)
        {
            return m_source[index];
        }

        /// <summary>
        ///     Gets the number of contained elements.
        /// </summary>
        public override int Count
        {
            get { return m_source.Count; }
        }

        #region Implementation

        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(m_source != null);
        }

        private readonly IList<TFrom> m_source;

        #endregion
    }
}