using Microsoft.EntityFrameworkCore;

namespace GuestBooks.Models
{
    public class SeedData
    {
        //1.3.3 撰寫SeedData類別的內容
        //      (1)撰寫靜態方法 Initialize(IServiceProvider serviceProvider)
        //      (2)撰寫Book及ReBook資料表內的初始資料程式
        //      (3)撰寫上傳圖片的程式
        //      (4)加上 using() 及 判斷資料庫是否有資料的程式

        public static void Initailize(IServiceProvider serviceProvider)
        {

            using (GuestBookContext context = new GuestBookContext(serviceProvider.GetRequiredService<DbContextOptions<GuestBookContext>>()))
            {
                {

                    if (!context.Book.Any())
                    {
                        string[] guid = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

                        context.Book.AddRange(
                                new Book
                                {
                                    BookID = guid[0],
                                    Title = "挑戰手搖飲金萱茶之巔",
                                    Description = "金茶伍的日光金萱是我喝過最好喝的純茶類",
                                    Author = "泉hooshi",
                                    Photo = guid[0] + ".jpg",
                                    CreateDate = DateTime.Now
                                },
                                new Book
                                {
                                    BookID = guid[1],
                                    Title = "很像某品牌...",
                                    Description = "所以有人知道三分春色跟麻古有關係嗎",
                                    Author = "glassbox666",
                                    Photo = guid[1] + ".jpg",
                                    CreateDate = DateTime.Now
                                },
                                new Book
                                {
                                    BookID = guid[2],
                                    Title = "總之提神一下",
                                    Description = "我有金萱癮，一天一罐很基本吧",
                                    Author = "迪肉燦",
                                    Photo = guid[2] + ".jpg",
                                    CreateDate = DateTime.Now
                                },
                                new Book
                                {
                                    BookID = guid[3],
                                    Title = "咀嚼控到處喝飲料的日記",
                                    Description = "好喝~推薦三分春色的金萱雙Q",
                                    Author = "民大土土",
                                    Photo = guid[3] + ".jpg",
                                    CreateDate = DateTime.Now
                                },
                                new Book
                                {
                                    BookID = guid[4],
                                    Title = "永遠支持山楂烏龍統治地球",
                                    Description = "茶摩什麼時候才要全通路販售這款山楂烏龍",
                                    Author = "DrinKing",
                                    Photo = guid[4] + "jpg",
                                    CreateDate = DateTime.Now
                                }

                            );
                        context.ReBook.AddRange(
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "沒喝過，應該不是連鎖品牌ㄅ",
                                Author = "橘子島",
                                CreateDate = DateTime.Now,
                                BookID = guid[0]
                            },
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "之前出差喝過，有股很重的奶香味",
                                Author = "秀秀枝枝",
                                CreateDate = DateTime.Now,
                                BookID = guid[0]
                            },
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "認同",
                                Author = "烏吉zi",
                                CreateDate = DateTime.Now,
                                BookID = guid[0]
                            },
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "應該只是logo跟門店設計很像而已?",
                                Author = "勝勝",
                                CreateDate = DateTime.Now,
                                BookID = guid[1]
                            },
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "只有我覺得金萱喝起來有股怪味嗎",
                                Author = "88888",
                                CreateDate = DateTime.Now,
                                BookID = guid[2]
                            },
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "以前可以換寒天，比珍珠好吃",
                                Author = "率星人bonon",
                                CreateDate = DateTime.Now,
                                BookID = guid[3]
                            },
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "50嵐1號可以吃很飽",
                                Author = "眼鏡盒",
                                CreateDate = DateTime.Now,
                                BookID = guid[3]
                            },
                            new ReBook
                            {
                                ReBookID = Guid.NewGuid().ToString(),
                                Description = "my love山楂烏龍^3^",
                                Author = "橘子島",
                                CreateDate = DateTime.Now,
                                BookID = guid[4]
                            }
                        );


                        context.SaveChanges();

                        //(3)撰寫上傳圖片的程式 //會把 SeedPhoto 資料夾裡的圖片複製到 wwwroot/SeedPhoto，並重新命名為 guid 對應的檔名。
                        string SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedPhoto");
                        string BookPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SeedPhoto");

                        string[] files = Directory.GetFiles(SeedPhotoPath);

                        for (int i = 0; i < files.Length; i++)
                        {
                            string destFile = Path.Combine(BookPhotoPath, guid[i] + ".jpg");

                            File.Copy(files[i], destFile);
                        }
                    }
                }

            }


        }
    }
}
