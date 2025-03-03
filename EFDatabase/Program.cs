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
                /*  var users = new List<User>()
                  {
                    new User
                      {
                      Id = new Guid(),
                      Name = "User1",
                      Email = "123@11.com",
                      Password = "111",
                      CreatedDate = DateTime.Now,
                      IsAdmin = true,
                      IsBanned = false,
                      },

                      new User
                      {
                          Id = new Guid(),
                          Name = "User1",
                          Email = "123@11.com",
                          Password = "111",
                          CreatedDate = DateTime.Now,
                          IsAdmin = true,
                          IsBanned = false,
                      },

                      new User
                      {
                          Id = new Guid(),
                          Name = "User1",
                          Email = "123@11.com",
                          Password = "111",
                          CreatedDate = DateTime.Now,
                          IsAdmin = true,
                          IsBanned = false,
                      }
                  };*/

                //var us = db.Users.ToList();
                //us[0].Name = "User2222";
                //us[0].Email = "22@22.net";
                //us[0].Password = "222";
                //us[0].IsBanned = false;
                //us[0].IsAdmin = false;

                //us[1].Name = "User Vasily";
                //us[1].Email = "Vasily@Ivanov.ru";
                //us[1].Password = "33";
                //us[1].IsBanned = false;
                //us[1].IsAdmin = false;

                //await db.Users.AddRangeAsync(users);
                await db.SaveChangesAsync();

                var u2 = await db.Users.FirstOrDefaultAsync(x => x.Name == "User2222");
                db.Users.Remove(u2);
                await db.SaveChangesAsync();

                var us3 = db.Users.ToList();
                foreach (var u in us3) 
                { Console.WriteLine($"ID:{u.Id}, name:{u.Name}, @:{u.Email}, pas:{u.Password}, CrDate:{u.CreatedDate}, Adm:{u.IsAdmin}, Ban:{u.IsBanned}"); }
            }
        }
    }
}
