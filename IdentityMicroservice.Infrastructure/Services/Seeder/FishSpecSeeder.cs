using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using System.Collections.Generic;

namespace IdentityMicroservice.Infrastructure.Services.Seeder
{
    public class FishSpecSeeder
    {
        private readonly IdentityDbContext _context;

        public FishSpecSeeder(IdentityDbContext context)
        {
            _context = context;
        }

        public void SeedFishSpecs()
        {
            var fishList = new List<FishSpecs>
            {
                new FishSpecs
                {
                    Species = "Amano Shrimp",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Apistograma Cacatuoides",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Apple Snail",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Betta Fish",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Bristlenose Pleco",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Corydoras",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Discus",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Freshwater Angelfish",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Guppy",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Neon Tetra",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Oscar",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Rams",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Ramshorn Snail",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "RedBee Shrimp",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Siamese Algae Eater",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Tiger Barb",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Zebra Fish",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                },
                new FishSpecs
                {
                    Species = "Mollie",
                    WaterType = Domain.Enums.WaterType.FRESHWATER,
                    Diet = Domain.Enums.Diet.OMNIVORE,
                    WaterAcidity = 14.03M,
                    WaterTemperature = 27,
                    FishSize = 5
                }
            };

            _context.AddRange(fishList);
            _context.SaveChanges();

        }
    }
}

