using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen
{
    public class Relation
    {
        /// <summary>
        /// Relation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A list of field names to retrieve
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition Where { get; set; }

        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public OrderBy[] Orders { get; set; }

        /// <summary>
        /// The query result of link objects
        /// </summary>
        public bool FromLink { get; set; }

        public Relation() { }

        public Relation(string name, string[] fields, ICondition where, OrderBy[] orders, bool fromLink)
        {
            Name = name;
            Fields = fields;
            Where = where;
            Orders = orders;
            FromLink = fromLink;
        }

    }
}