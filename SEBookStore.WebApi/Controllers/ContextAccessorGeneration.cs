﻿//@GeneratedCode
namespace SEBookStore.WebApi.Controllers
{
    /// <summary>
    /// Generated by the generator
    /// </summary>
    partial class ContextAccessor
    {
        /// <summary>
        /// Generated by the generator
        /// </summary>
        partial void GetEntitySetHandler<TEntity>(ref Logic.Contracts.IEntitySet<TEntity>? entitySet, ref bool handled) where TEntity : Logic.Entities.EntityObject, new()
        {
            if (typeof(TEntity) == typeof(SEBookStore.Logic.Entities.Book))
            {
                entitySet = GetContext().BookSet as Logic.Contracts.IEntitySet<TEntity>;
                handled = true;
            }
        }
    }
}
