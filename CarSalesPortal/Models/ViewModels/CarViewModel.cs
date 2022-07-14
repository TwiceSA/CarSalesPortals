using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarSalesPortal.Models.ViewModels
{
    public class CarViewModel
    {
        public Car Car { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }

        private List<Currency> CList = new List<Currency>();

        private List<Currency> CreateList()
        {
            CList.Add(new Currency("USD", "USD"));
            CList.Add(new Currency("ZAR", "ZAR"));
            CList.Add(new Currency("EUR", "EUR"));
            return CList;

        }
        public CarViewModel()
        {
            Currencies = CreateList();
        }


        public class Currency
        {
            public String Id { get; set; }
            public String Name { get; set; }

            public Currency(String id, String name)
            {
                Id = id;
                Name = name;
            }
        }
    }

}
