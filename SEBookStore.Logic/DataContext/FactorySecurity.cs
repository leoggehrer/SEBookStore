﻿//@CodeCopy
#if ACCOUNT_ON
using SEBookStore.Logic.Contracts;

namespace SEBookStore.Logic.DataContext
{
    partial class Factory
    {
        /// <summary>
        /// Creates an instance of IContext.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <returns>An instance of IContext.</returns>
        public static IContext CreateContext(string sessionToken)
        {
            var result = new ProjectDbContext(sessionToken);

            return result;
        }
    }
}
#endif
