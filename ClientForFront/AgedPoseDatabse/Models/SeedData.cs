using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgedPoseDatabse.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AiForAgedDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AiForAgedDbContext>>()))
            {
                if (context.RoomInfos.Any())
                {
                    return;//DB has been seeded
                }

                RoomInfo room1 = new RoomInfo()
                {
                    Name = "暂未分配",
                    RoomSize = 25
                };
                RoomInfo room2 = new RoomInfo()
                {
                    Name = "101",
                    RoomSize = 30
                };

                AgesInfo aged1 = new AgesInfo()
                {
                    Name = "张三",
                };
                AgesInfo aged2 = new AgesInfo()
                {
                    Name = "李四",
                };

                Random random = new Random();
                aged1.PoseInfoes = new List<PoseInfo>();
                aged2.PoseInfoes = new List<PoseInfo>();
                for (int i = 0; i < 40; i++)
                {
                    PoseInfo poseInfo1 = new PoseInfo()
                    {
                        Date = DateTime.Now.AddDays(-i),
                        TimeIn = DateTime.Now.ToShortTimeString(),
                        TimeLie = random.Next(1800, 7200),//0.5h-2h
                        TimeSit = random.Next(1800, 7200),
                        TimeStand = random.Next(1800, 7200),
                        TimeDown = random.Next(1800, 7200),
                        TimeOther = random.Next(1800, 7200),
                        IsAlarm = false
                    };
                    aged1.PoseInfoes.Add(poseInfo1);

                    PoseInfo poseInfo2 = new PoseInfo()
                    {
                        Date = DateTime.Now.AddDays(-i),
                        TimeIn = DateTime.Now.ToShortTimeString(),
                        TimeLie = random.Next(1800, 7200),//0.5h-2h
                        TimeSit = random.Next(1800, 7200),
                        TimeStand = random.Next(1800, 7200),
                        TimeDown = random.Next(1800, 7200),
                        TimeOther = random.Next(1800, 7200),
                        IsAlarm = false
                    };
                    aged2.PoseInfoes.Add(poseInfo2);

                }

                room1.AgesInfos = new List<AgesInfo>();
                room1.AgesInfos.Add(aged1);

                room2.AgesInfos = new List<AgesInfo>();
                room2.AgesInfos.Add(aged2);

                context.RoomInfos.AddRange(room1, room2);
                context.SaveChanges();

                //PoseInfo poseInfo1 = new PoseInfo()
                //{
                //    
                //};




            }
        }
    }
}
