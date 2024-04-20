namespace FitnessApp.API.Services;

public interface IFitnessAppRepository
{
    Task<bool> SaveChangesAsync();
}
