using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Domain.Entities;
using IdentityMicroservice.Infrastructure.Persistence.DbContexts.Identity;
using IdentityMicroservice.Infrastructure.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Managers.FishTanks
{
    public class FishTankManager : IFishTankManager
    {
        private readonly IdentityDbContext _context;

        public FishTankManager(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrRemoveFish(int TankId, string FishName, bool isDeleted)
        {
            var existingFishTank = await _context.FishTanks.Include(x=>x.PetFish).Where(x => x.Id == TankId).ToListAsync();

            if (existingFishTank != null)
            {
                var fishInDb = await _context.FishSpecs.Where(x => x.Species == FishName).SingleOrDefaultAsync();

                //check if there are any other fish in the aquarium:
                if (fishInDb != null)
                {
                    if (isDeleted == false)
                    {
                        var biggestPetFish = await _context.FishTanks
                           .Where(t => t.Id == TankId)
                           .SelectMany(t => t.PetFish)
                           .Join(_context.FishSpecs,
                                 pf => pf.FishName,
                                 fs => fs.Species,
                                 (pf, fs) => fs)
                           .OrderByDescending(fs => fs.FishSize)
                           .FirstOrDefaultAsync();

                        var smallestPetFish = await _context.FishTanks
                          .Where(t => t.Id == TankId)
                          .SelectMany(t => t.PetFish)
                          .Join(_context.FishSpecs,
                                pf => pf.FishName,
                                fs => fs.Species,
                                (pf, fs) => fs)
                          .OrderBy(fs => fs.FishSize)
                          .FirstOrDefaultAsync();


                        if (biggestPetFish == null && smallestPetFish == null)
                        {
                            var fishToAdd = new PetFish
                            {
                                FishName = FishName,
                                FishTanks = existingFishTank
                            };

                            _context.PetFish.Add(fishToAdd);
                            await _context.SaveChangesAsync();

                            return true;
                        }

                        var res = CheckFishCompatibility(biggestPetFish, smallestPetFish, fishInDb);
                        if (res == true)
                        {
                            var fishToAdd = new PetFish
                            {
                                FishName = FishName,
                                FishTanks = existingFishTank
                            };

                            _context.PetFish.Add(fishToAdd);
                            await _context.SaveChangesAsync();
                        }

                        return res;
                    }
                    else
                    {

                        //var fishToRemove = await _context.PetFish.Include(x=>x.FishTanks).Where(x=>x.FishTanks.Where(y=>y.Id==TankId)).FirstOrDefaultAsync(x => x.FishName == FishName && x.FishTanks);
                        var fishToRemove = existingFishTank.First().PetFish.Where(x => x.FishName == FishName).FirstOrDefault();

                        if (fishToRemove == null)
                        {
                            return false;
                        }

                        _context.PetFish.Remove(fishToRemove);
                        await _context.SaveChangesAsync();

                        return true;
                    }

                }
            }
            throw new System.Exception("Create a fishtank first");
        }

        public async Task<AddOrUpdateTankResponseModel> CreateOrEditFishTank(AddOrUpdateTankRequestModel model, IdentityUser? user = null)
        {
            if (model.TankId == null)
            {
                var fishTanks = new FishTank
                {
                    Height = model.Height,
                    Width = model.Width,
                    Lenght = model.Length,
                    Volume = model.Height * model.Width * model.Length,
                    User = user

                };

                _context.FishTanks.Add(fishTanks);
                await _context.SaveChangesAsync();

                return fishTanks.ToModelMapper();
            }
            else
            {
                var existingFishTank = await _context.FishTanks.SingleOrDefaultAsync(x => x.Id == model.TankId);
                if (existingFishTank != null)
                {
                    existingFishTank.Height = model.Height;
                    existingFishTank.Width = model.Width;
                    existingFishTank.Lenght = model.Length;

                    _context.FishTanks.Update(existingFishTank);
                    await _context.SaveChangesAsync();

                    return existingFishTank.ToModelMapper();
                }

                return null;
            }
        }

        public async Task<GetTanksModel> GetFishTanks(int? TankId = null)
        {
            List<FishTank> fishTanks;
            var tankModel = new GetTanksModel();
            List<string> listOfFish;

            if (TankId == null)
            {
                //get all 
                fishTanks = await _context.FishTanks.Include(x => x.PetFish).ToListAsync();

                tankModel.TankModels = new Dictionary<int, List<string>>();

                foreach (var tank in fishTanks)
                {
                    listOfFish = tank.PetFish.Select(x => x.FishName).ToList();
                    tankModel.TankModels.Add(tank.Id, listOfFish);
                }
            }
            else
            {
                //get one
                fishTanks = await _context.FishTanks.Include(x => x.PetFish).Where(x => x.Id == TankId).ToListAsync();
                listOfFish = fishTanks[0].PetFish.Select(x => x.FishName).ToList();

                tankModel.TankModels = new Dictionary<int, List<string>>
                {
                        {  fishTanks[0].Id, listOfFish }
                };

            }

            return tankModel;
        }

        public async Task<bool> RemoveFishTank(int TankId)
        {
            var existingFishTank = await _context.FishTanks.SingleOrDefaultAsync(x => x.Id == TankId);
            if (existingFishTank != null)
            {
                _context.FishTanks.Remove(existingFishTank);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        private bool CheckFishCompatibility(FishSpecs biggestFish, FishSpecs smallestFish, FishSpecs fishToBeAdded)
        {
            if (biggestFish.WaterType != fishToBeAdded.WaterType)
                return false;

            if ((biggestFish.Diet == Domain.Enums.Diet.CARNIVORE || biggestFish.Diet == Domain.Enums.Diet.OMNIVORE) && biggestFish.FishSize - 5 > fishToBeAdded.FishSize)
                return false;

            if ((fishToBeAdded.Diet == Domain.Enums.Diet.CARNIVORE || fishToBeAdded.Diet == Domain.Enums.Diet.OMNIVORE) && fishToBeAdded.FishSize - 5 > smallestFish.FishSize)
                return false;

            if (biggestFish.WaterTemperature != fishToBeAdded.WaterTemperature)
                return false;

            if (biggestFish.WaterAcidity != fishToBeAdded.WaterAcidity)
                return false;
            return true;
        }
    }
}
