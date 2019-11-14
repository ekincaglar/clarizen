using System;
using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen
{
    public class Query : IClarizenQuery
    {
        private string _select { get; set; }
        private string _from { get; set; }
        private string _where { get; set; }
        private string _groupBy { get; set; }
        private string _orderBy { get; set; }
        private bool _limitSet { get; set; }
        private int _limit { get; set; }
        private bool _offsetSet { get; set; }
        private int _offset { get; set; }

        public IClarizenQuery Select(string query)
        {
            _select = query;
            return this;
        }

        public IClarizenQuery From(string query)
        {
            _from = query;
            return this;
        }

        public IClarizenQuery Where(string query)
        {
            _where = query;
            return this;
        }

        public IClarizenQuery GroupBy(string query)
        {
            _groupBy = query;
            return this;
        }

        public IClarizenQuery OrderBy(string query)
        {
            _orderBy = query;
            return this;
        }

        public IClarizenQuery Limit(int rows)
        {
            _limit = rows;
            _limitSet = true;
            return this;
        }

        public IClarizenQuery Offset(int startFrom)
        {
            _offset = startFrom;
            _offsetSet = true;
            return this;
        }

        public string ToCZQL()
        {
            string czql = string.Empty;
            if (!String.IsNullOrWhiteSpace(_select))
                czql += "SELECT " + _select;
            if (!String.IsNullOrWhiteSpace(_from))
                czql += " FROM " + _from;
            if (!String.IsNullOrWhiteSpace(_where))
                czql += " WHERE " + _where;
            if (!String.IsNullOrWhiteSpace(_groupBy))
                czql += " GROUP BY " + _groupBy;
            if (!String.IsNullOrWhiteSpace(_orderBy))
                czql += " ORDER BY " + _orderBy;
            if (_limitSet)
                czql += " LIMIT " + _limit;
            if (_offsetSet)
                czql += " OFFSET " + _offset;
            return czql;
        }
    }
}