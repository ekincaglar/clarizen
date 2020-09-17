using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Request
{
    public class Upsert
    {
        public IEntity InsertEntity { get; set; }
        public IEntity UpdateEntity { get; set; }

        /// <summary>
        /// Define the request for an Upsert opertation
        /// </summary>
        /// <param name="insertEntity">The entity Id and the fields that will be used to create the entity</param>
        /// <param name="updateEntity">The entity Id and the fields that will be used to update the entity</param>
        public Upsert(IEntity insertEntity, IEntity updateEntity)
        {
            InsertEntity = insertEntity;
            UpdateEntity = updateEntity;
        }
    }
}