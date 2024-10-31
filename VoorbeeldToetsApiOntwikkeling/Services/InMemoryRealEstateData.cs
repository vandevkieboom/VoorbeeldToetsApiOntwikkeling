using VoorbeeldToetsApiOntwikkeling.Models;

namespace VoorbeeldToetsApiOntwikkeling.Services
{
    public class InMemoryRealEstateData : IRealEstateData
    {
        static List<Property> _properties;
        private static int _nextId;

        public InMemoryRealEstateData()
        {
            _properties = new List<Property>
            {
                //some properties hardcoded for testing
                new Property
                {
                    Id = 1,
                    Address = "Mechelsesteenweg 123",
                    City = "Antwerpen",
                    NumberOfRooms = 3,
                    PropertyType = PropertyType.House,
                    Price = 250000m,
                    IsSold = false
                },
                new Property
                {
                    Id = 2,
                    Address = "Kasteelpleinstraat 10",
                    City = "Antwerpen",
                    NumberOfRooms = 4,
                    PropertyType = PropertyType.Apartment,
                    Price = 180000m,
                    IsSold = true
                },
                new Property
                {
                    Id = 3,
                    Address = "Noorderplaats 15",
                    City = "Antwerpen",
                    NumberOfRooms = 2,
                    PropertyType = PropertyType.House,
                    Price = 220000m,
                    IsSold = false
                },
                new Property
                {
                    Id = 4,
                    Address = "Plantin en Moretuslei 40",
                    City = "Antwerpen",
                    NumberOfRooms = 5,
                    PropertyType = PropertyType.House,
                    Price = 320000m,
                    IsSold = false
                },
                new Property
                {
                    Id = 5,
                    Address = "Lange Leemstraat 45",
                    City = "Antwerpen",
                    NumberOfRooms = 3,
                    PropertyType = PropertyType.Apartment,
                    Price = 210000m,
                    IsSold = true
                }
            };

            _nextId = _properties.Max(p => p.Id) + 1;
        }

        public IEnumerable<Property> GetForSale()
        {
            return _properties.Where(x => x.IsSold == false);
        }

        public IEnumerable<Property> GetSold()
        {
            return _properties.Where(x => x.IsSold == true);
        }

        public Property? Get(int id)
        {
            return _properties.FirstOrDefault(x => x.Id == id);
        }

        public Property Add(Property newProperty)
        {
            newProperty.Id = _nextId++;
            newProperty.IsSold = false;
            _properties.Add(newProperty);
            return newProperty;
        }

        public void Update(Property property)
        {
            var existingProperty = _properties.FirstOrDefault(x => x.Id == property.Id);
            if (existingProperty is not null)
            {
                existingProperty.Address = property.Address;
                existingProperty.City = property.City;
                existingProperty.NumberOfRooms = property.NumberOfRooms;
                existingProperty.PropertyType = property.PropertyType;
                existingProperty.Price = property.Price;
                existingProperty.IsSold = property.IsSold;
            }
        }
    }
}
