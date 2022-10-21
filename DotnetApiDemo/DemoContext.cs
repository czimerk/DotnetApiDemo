using DotnetApiDemo.Domain;
using GraphQLDemo.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiDemo
{
    public class DemoContext : DbContext
    {
        private const string A1 = "7f72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string A2 = "8f72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string A3 = "9f72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string A4 = "af72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string A5 = "bf72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string U1 = "cf72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string U2 = "df72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string U3 = "ef72ddbe-1932-4cb6-9204-8049dde91d82";
        private const string O1 = "1f72ddbe-1932-4cb6-9204-8049dde91d83";
        private const string O2 = "2f72ddbe-1932-4cb6-9204-8049dde91d84";
        private const string O3 = "3f72ddbe-1932-4cb6-9204-8049dde91d85";
        private const string O4 = "4f72ddbe-1932-4cb6-9204-8049dde91d85";

        public DemoContext(DbContextOptions<DemoContext> options)
            : base(options)
        {

        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<User> Users { get; set; }

        public void Seed()
        {
            Articles.AddRange(new Article()
            {
                Id = new Guid(A1),
                Name = "Article 1",
                Price = 1.1,
                Unit = "$",
                Created = new DateTime(2019, 01, 01)
            },
            new Article()
            {
                Id = new Guid(A2),
                Name = "Article 2",
                Price = 2.1,
                Unit = "$",
                Created = new DateTime(2019, 02, 01)
            },
            new Article()
            {
                Id = new Guid(A3),
                Name = "Article 3",
                Price = 3.1,
                Unit = "$",
                Created = new DateTime(2020, 01, 01)
            },
            new Article()
            {
                Id = new Guid(A4),
                Name = "Article 4",
                Price = 5.1,
                Unit = "$",
                Created = new DateTime(2021, 01, 01)
            },
            new Article()
            {
                Id = new Guid(A5),
                Name = "Article 5",
                Price = 3.5,
                Unit = "$",
                Created = new DateTime(2021, 05, 01)
            });

            Users.AddRange(new User()
            {
                Email = "john.doe@gmail.com",
                Id = new Guid(U1),
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(2000, 01, 01),
                RegisteredDate = new DateTime(2022, 03, 03),
                Password = "Password123"
            },
            new User()
            {
                Email = "jane.doe@gmail.com",
                Id = new Guid(U2),
                FirstName = "Jane",
                LastName = "Doe",
                BirthDate = new DateTime(2000, 03, 03),
                RegisteredDate = new DateTime(2022, 05, 03),
                Password = "Password456"
            },
            new User()
            {
                Email = "admin.allen@gmail.com",
                Id = new Guid(U3),
                FirstName = "Admin",
                LastName = "Allen",
                BirthDate = new DateTime(2000, 03, 03),
                RegisteredDate = new DateTime(2021, 03, 03),
                Password = "Password789"
            });


            SaveChanges();
            Orders.AddRange(
                new Order()
                {
                    Id = new Guid(O1),
                    OrderDate = new DateTime(2021, 03, 01, 20, 02, 2),
                    UserId = new Guid(U1),
                    User = Users.First(u => u.Id == new Guid(U1)),
                    OrderLines = new List<OrderLine>() {
                        new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A1)),
                            Quantity = 1,
                        },
                         new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A2)),
                            Quantity = 3,
                        },
                          new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A3)),
                            Quantity = 1,
                        }
                    }
                },
                  new Order()
                  {
                      Id = new Guid(O2),
                      OrderDate = new DateTime(2022, 01, 02, 13, 02, 2),
                      UserId = new Guid(U2),
                      User = Users.First(u => u.Id == new Guid(U2)),
                      OrderLines = new List<OrderLine>() {
                        new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A4)),
                            Quantity = 1,
                        },
                         new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A5)),
                            Quantity = 6,
                        },
                          new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A1)),
                            Quantity = 1,
                        }
                    }
                  },
                  new Order()
                  {
                      Id = new Guid(O3),
                      OrderDate = new DateTime(2022, 01, 02, 13, 02, 2),
                      UserId = new Guid(U2),
                      User = Users.First(u => u.Id == new Guid(U2)),
                      OrderLines = new List<OrderLine>() {
                        new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A1)),
                            Quantity = 1,
                        },
                         new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A2)),
                            Quantity = 1,
                        },
                          new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A4)),
                            Quantity = 1,
                        }
                    }
                  },
                  new Order()
                  {
                      Id = new Guid(O4),
                      OrderDate = new DateTime(2021, 12, 02, 11, 02, 2),
                      UserId = new Guid(U2),
                      User = Users.First(u => u.Id == new Guid(U2)),
                      OrderLines = new List<OrderLine>() {
                        new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A4)),
                            Quantity = 1,
                        },
                         new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A1)),
                            Quantity = 2,
                        },
                          new OrderLine(){
                            Article = Articles.FirstOrDefault(a=>a.Id == new Guid(A5)),
                            Quantity = 2,
                        }
                    }
                  }
            );

            SaveChanges();
        }
    }
}
