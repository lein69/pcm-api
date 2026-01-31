using Microsoft.AspNetCore.Identity;
using PCM.Api.Enums;
using PCM.Api.Models;

namespace PCM.Api.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // ================= ROLES =================

            string[] roles = { "Admin", "Treasurer", "Referee", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ================= ADMIN =================

            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@pcm.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // ================= ADMIN MEMBER =================

            if (!context.Members.Any(m => m.Email == "admin@pcm.com"))
            {
                var adminMember = new Member
                {
                    FullName = "System Admin",
                    Email = "admin@pcm.com",
                    PhoneNumber = "0000000000",
                    UserId = adminUser.Id,
                    JoinDate = DateTime.Now,
                    RankLevel = 5,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                context.Members.Add(adminMember);
                await context.SaveChangesAsync();
            }

            // ================= MEMBERS =================

            var membersData = new[]
            {
                new { UserName = "member1", Email = "member1@pcm.com", Name = "Nguyen Van A", Phone = "0123456789" },
                new { UserName = "member2", Email = "member2@pcm.com", Name = "Tran Thi B", Phone = "0987654321" },
                new { UserName = "member3", Email = "member3@pcm.com", Name = "Le Van C", Phone = "0111111111" },
                new { UserName = "member4", Email = "member4@pcm.com", Name = "Pham Thi D", Phone = "0222222222" },
                new { UserName = "member5", Email = "member5@pcm.com", Name = "Hoang Van E", Phone = "0333333333" }
            };

            foreach (var data in membersData)
            {
                var user = await userManager.FindByNameAsync(data.UserName);

                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = data.UserName,
                        Email = data.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Member123!");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Member");

                        var member = new Member
                        {
                            FullName = data.Name,
                            Email = data.Email,
                            PhoneNumber = data.Phone,
                            UserId = user.Id,
                            JoinDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 300)),
                            RankLevel = 3 + Random.Shared.NextDouble() * 2,
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };

                        context.Members.Add(member);
                    }
                }
            }

            await context.SaveChangesAsync();

            // ================= COURTS =================

            if (!context.Courts.Any())
            {
                context.Courts.AddRange(
                    new Court
                    {
                        Name = "Sân 1",
                        IsActive = true,
                        Description = "Sân chính",
                        CreatedDate = DateTime.Now
                    },
                    new Court
                    {
                        Name = "Sân 2",
                        IsActive = true,
                        Description = "Sân phụ",
                        CreatedDate = DateTime.Now
                    }
                );

                await context.SaveChangesAsync();
            }

            // ================= CATEGORIES =================

            if (!context.TransactionCategories.Any())
            {
                context.TransactionCategories.AddRange(
                    new TransactionCategory { Name = "Tiền sân", Type = TransactionType.Thu },
                    new TransactionCategory { Name = "Quỹ tháng", Type = TransactionType.Thu },
                    new TransactionCategory { Name = "Nước", Type = TransactionType.Chi },
                    new TransactionCategory { Name = "Phạt", Type = TransactionType.Chi }
                );

                await context.SaveChangesAsync();
            }

            // ================= TRANSACTIONS =================

            if (!context.Transactions.Any())
            {
                var adminMember = context.Members
                    .FirstOrDefault(m => m.Email == "admin@pcm.com");

                if (adminMember != null)
                {
                    context.Transactions.AddRange(
                        new Transaction
                        {
                            Date = DateTime.Now.AddDays(-10),
                            Amount = 1000000,
                            Description = "Quỹ tháng 1",
                            CategoryId = 1,
                            CreatedById = adminMember.Id
                        },
                        new Transaction
                        {
                            Date = DateTime.Now.AddDays(-5),
                            Amount = -200000,
                            Description = "Mua nước",
                            CategoryId = 3,
                            CreatedById = adminMember.Id
                        }
                    );

                    await context.SaveChangesAsync();
                }
            }

            // ================= CHALLENGE =================

            if (!context.Challenges.Any())
            {
                var adminMember = context.Members
                    .FirstOrDefault(m => m.Email == "admin@pcm.com");

                if (adminMember != null)
                {
                    var challenge = new Challenge
                    {
                        Title = "Giải Mini Tháng 1",
                        Type = ChallengeType.MiniGame,
                        GameMode = GameMode.TeamBattle,
                        Status = ChallengeStatus.Ongoing,
                        Config_TargetWins = 5,
                        EntryFee = 50000,
                        PrizePool = 500000,
                        CreatedById = adminMember.Id,
                        StartDate = DateTime.Now.AddDays(-7),
                        CreatedDate = DateTime.Now
                    };

                    context.Challenges.Add(challenge);
                    await context.SaveChangesAsync();

                    var participants = context.Members.Take(6).ToList();

                    foreach (var member in participants)
                    {
                        context.Participants.Add(new Participant
                        {
                            ChallengeId = challenge.Id,
                            MemberId = member.Id,
                            Team = participants.IndexOf(member) % 2 == 0
                                ? TeamSide.TeamA
                                : TeamSide.TeamB,
                            EntryFeePaid = true,
                            Status = ParticipantStatus.Confirmed,
                            JoinedDate = DateTime.Now.AddDays(-7)
                        });
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
