using EFDatabase.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFDatabase
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new GNAggregatorContext())
            {
                
                //await db.SaveChangesAsync();

                //var u2 = await db.Users.FirstOrDefaultAsync(x => x.Name == "User2222");
                //db.Users.Remove(u2);
                //await db.SaveChangesAsync();

                //var us3 = db.Users.ToList();
                //foreach (var u in us3) 
                //{ Console.WriteLine($"ID:{u.Id}, name:{u.Name}, @:{u.Email}, pas:{u.Password}, CrDate:{u.CreatedDate}, Adm:{u.IsAdmin}, Ban:{u.IsBanned}"); }
            }
        }
    }
}
