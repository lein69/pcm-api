using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Enums;
using PCM.Api.Models;

namespace PCM.Api.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("Treasurer"))
                await roleManager.CreateAsync(new IdentityRole("Treasurer"));
            if (!await roleManager.RoleExistsAsync("Referee"))
                await roleManager.CreateAsync(new IdentityRole("Referee"));
            if (!await roleManager.RoleExistsAsync("Member"))
                await roleManager.CreateAsync(new IdentityRole("Member"));

            // Create admin user
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@pcm.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                // Update password if user exists but password is wrong
                var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, passwordResetToken, "Admin123!");
            }

            // Create members
            var membersData = new[]
            {
                new { UserName = "member1", Email = "member1@pcm.com", FullName = "Nguyen Van A", Phone = "0123456789" },
                new { UserName = "member2", Email = "member2@pcm.com", FullName = "Tran Thi B", Phone = "0987654321" },
                new { UserName = "member3", Email = "member3@pcm.com", FullName = "Le Van C", Phone = "0111111111" },
                new { UserName = "member4", Email = "member4@pcm.com", FullName = "Pham Thi D", Phone = "0222222222" },
                new { UserName = "member5", Email = "member5@pcm.com", FullName = "Hoang Van E", Phone = "0333333333" },
                new { UserName = "member6", Email = "member6@pcm.com", FullName = "Vo Thi F", Phone = "0444444444" },
                new { UserName = "member7", Email = "member7@pcm.com", FullName = "Dinh Van G", Phone = "0555555555" },
                new { UserName = "member8", Email = "member8@pcm.com", FullName = "Bui Thi H", Phone = "0666666666" }
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
                    await userManager.CreateAsync(user, "Member123!");
                    await userManager.AddToRoleAsync(user, "Member");

                    var member = new Member
                    {
                        FullName = data.FullName,
                        Email = data.Email,
                        PhoneNumber = data.Phone,
                        UserId = user.Id,
                        JoinDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 365)),
                        RankLevel = 3.0 + Random.Shared.NextDouble() * 2,
                        IsActive = true,
                        TotalMatches = Random.Shared.Next(0, 50),
                        WinMatches = Random.Shared.Next(0, 25),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    context.Members.Add(member);
                }
                else
                {
                    // Update password if user exists but password is wrong
                    var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                    await userManager.ResetPasswordAsync(user, passwordResetToken, "Member123!");
                }
            }

            await context.SaveChangesAsync();

            // Courts
            if (!context.Courts.Any())
            {
                context.Courts.AddRange(
                    new Court { Name = "Sân 1", IsActive = true, Description = "Sân chính", CreatedDate = DateTime.Now },
                    new Court { Name = "Sân 2", IsActive = true, Description = "Sân phụ", CreatedDate = DateTime.Now }
                );
                await context.SaveChangesAsync();
            }

            // Transaction Categories
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

            // Transactions
            if (!context.Transactions.Any())
            {
                var adminMember = context.Members.FirstOrDefault(m => m.Email == "admin@pcm.com");
                if (adminMember != null)
                {
                    context.Transactions.AddRange(
                        new Transaction { Date = DateTime.Now.AddDays(-10), Amount = 1000000, Description = "Quỹ tháng 1", CategoryId = 2, CreatedById = adminMember.Id },
                        new Transaction { Date = DateTime.Now.AddDays(-5), Amount = -200000, Description = "Mua nước", CategoryId = 3, CreatedById = adminMember.Id }
                    );
                    await context.SaveChangesAsync();
                }
            }

            // Challenge
            if (!context.Challenges.Any())
            {
                var adminMember = context.Members.FirstOrDefault(m => m.Email == "admin@pcm.com");
                if (adminMember != null)
                {
                    var challenge = new Challenge
                    {
                        Title = "Giải Mini-game Tháng 1",
                        Type = ChallengeType.MiniGame,
                        GameMode = GameMode.TeamBattle,
                        Status = ChallengeStatus.Ongoing,
                        Config_TargetWins = 5,
                        CurrentScore_TeamA = 2,
                        CurrentScore_TeamB = 1,
                        EntryFee = 50000,
                        PrizePool = 500000,
                        CreatedById = adminMember.Id,
                        StartDate = DateTime.Now.AddDays(-7),
                        CreatedDate = DateTime.Now
                    };
                    context.Challenges.Add(challenge);
                    await context.SaveChangesAsync();

                    // Participants
                    var participants = context.Members.Take(10).ToList();
                    foreach (var member in participants)
                    {
                        context.Participants.Add(new Participant
                        {
                            ChallengeId = challenge.Id,
                            MemberId = member.Id,
                            Team = participants.IndexOf(member) % 2 == 0 ? TeamSide.TeamA : TeamSide.TeamB,
                            EntryFeePaid = true,
                            EntryFeeAmount = 50000,
                            JoinedDate = DateTime.Now.AddDays(-7),
                            Status = ParticipantStatus.Confirmed
                        });
                    }
                    await context.SaveChangesAsync();

                    // Matches
                    var teamA = participants.Where(p => participants.IndexOf(p) % 2 == 0).ToList();
                    var teamB = participants.Where(p => participants.IndexOf(p) % 2 != 0).ToList();

                    // Match 1: Singles
                    context.Matches.Add(new Match
                    {
                        Date = DateTime.Now.AddDays(-5),
                        IsRanked = true,
                        ChallengeId = challenge.Id,
                        MatchFormat = MatchFormat.Singles,
                        Team1_Player1Id = teamA[0].Id,
                        Team2_Player1Id = teamB[0].Id,
                        WinningSide = WinningSide.Team1
                    });

                    // Match 2: Doubles
                    context.Matches.Add(new Match
                    {
                        Date = DateTime.Now.AddDays(-3),
                        IsRanked = true,
                        ChallengeId = challenge.Id,
                        MatchFormat = MatchFormat.Doubles,
                        Team1_Player1Id = teamA[0].Id,
                        Team1_Player2Id = teamA[1].Id,
                        Team2_Player1Id = teamB[0].Id,
                        Team2_Player2Id = teamB[1].Id,
                        WinningSide = WinningSide.Team2
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}