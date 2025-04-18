﻿//@CodeCopy
namespace SEBookStore.Logic.Entities
{
    /// <summary>
    /// Represents an abstract base class for entities with an identifier.
    /// </summary>
    public abstract partial class EntityObject : CommonContracts.IIdentifiable
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        [Key]
        public IdType Id { get; set; }
    }
}
