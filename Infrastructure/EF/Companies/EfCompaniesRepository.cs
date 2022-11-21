using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Companies;
using Domain;

public class EfCompaniesRepository : ICompaniesRepository
{
    private readonly PlanitContextProvider _planitContextProvider;

    // For add method : Application -> UsesCases -> Account

    public EfCompaniesRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }
    
    
    public IEnumerable<Companies> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
            return context.Companies.ToList<Domain.Companies>();
    }

    public Companies FetchById(int id)
    {
        using var context = _planitContextProvider.NewContext();
            var companies = context.Companies.FirstOrDefault(companies => companies.IdCompanies == id);

            if (companies == null)
                throw new KeyNotFoundException($"Companies with {id} has not been found");
            return companies;
    }

    public Companies FetchByName(string name)
    {
        using var context = _planitContextProvider.NewContext();
        var companies = context.Companies.FirstOrDefault(companies => companies.CompaniesName == name);

        return companies;
    }

    public Companies Create(Companies companie)
    {
        using var context = _planitContextProvider.NewContext();
            try
            {
                context.Companies.Add(companie);
                context.SaveChanges();
                return companie;
            }
            catch (DbUpdateConcurrencyException e)
            {
                return null;
            }
    }

    public bool Update(Companies companies)
    {
        using var context = _planitContextProvider.NewContext();
            try
            {
                context.Companies.Update(companies);
                return context.SaveChanges() == 1;
            }
            catch (DbUpdateConcurrencyException e)
            {
                return false;
            }
    }
    
    public bool Delete(Companies companies)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Companies.Remove(companies);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }
}