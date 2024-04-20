using FitnessApp.API.DbContexts;

namespace FitnessApp.API.Services;

public class FitnessAppRepository : IFitnessAppRepository
{
    private readonly FitnessAppContext _context;

    public FitnessAppRepository(FitnessAppContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
