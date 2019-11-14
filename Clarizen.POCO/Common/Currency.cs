using System;

namespace Ekin.Clarizen
{
    public class Currency
    {
        public double value { get; set; }
        public string currency { get; set; }

        public Currency()
        {
        }

        public Currency(double value, string currency)
        {
            this.value = value;
            this.currency = currency;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = obj as Currency;

            if (item == null)
            {
                return false;
            }

            bool currencyEquals = true;
            if (!string.IsNullOrWhiteSpace(currency) && !string.IsNullOrWhiteSpace(item.currency))
            {
                currencyEquals = currency.Equals(item.currency, StringComparison.InvariantCultureIgnoreCase);
            }

            return currencyEquals && value == item.value;
        }
    }
}