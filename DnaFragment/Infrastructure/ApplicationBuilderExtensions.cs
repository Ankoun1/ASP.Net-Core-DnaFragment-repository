namespace DnaFragment.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using static WebConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedAdministrator(services);
            //SeedCategories(services);
            //SeedLrProducts(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<DnaFragmentDbContext>();

            data.Database.Migrate();
        }

        /* private static void SeedCategories(IServiceProvider services)
         {
           var data = services.GetRequiredService<DnaFragmentContext>();

             if (data.Categories.Any())
             {
                 return;
             }


             data.Categories.AddRange(new[]
             {
                 new Category { Name = "ПАРФЮМИ ЗА ЖЕНИ LR", PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/guido-maria-kretscher-lr-woman.jpg" },
                 new Category { Name = "ТЕРАПИЯ ЗА ТЯЛО LR" ,PictureUrl = "https://primelr.ru/wp-content/uploads/2017/08/thumb_body-3.jpg"},
                 new Category { Name = "ХРАНИТЕЛНИ ДОБАВКИ LR",PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/super_omega_timelr_lr.jpg" },
                 new Category { Name = "КРЕМОВЕ ЗА ЛИЦЕ LR" ,PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/thumb_eye.jpg"},
                 new Category { Name = "АЛОЕ ВЕРА ГЕЛ ЗА ПИЕНЕ",PictureUrl = "https://primelr.ru/wp-content/uploads/2020/06/thumb_gel_aloe_vera_maglr-lr-immun_plus.jpg" },
                 new Category { Name = "КОЗМЕТИКА LR" ,PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/lr-colours-lipstick-care-balm-maglr.jpg"},
                 new Category { Name = "ПАРФЮМИ ЗА МЪЖЕ LR", PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/full_ocean_parfum.jpg" }
             });


             data.SaveChanges();
         }*/

        /*private static void SeedLrProducts(IServiceProvider services)
        {
          var data = services.GetRequiredService<DnaFragmentContext>();

            if (data.LrProducts.Any())
            {
                //var products = data.LrProducts.ToList();
                //data.LrProducts.RemoveRange(products);
                //data.SaveChanges();
                return;
            }


           data.LrProducts.AddRange(new[]
           {
           new LrProduct
           {
               Name = "Мъжки парфюм LR Bruce Willis",
               PackagingVolume = "50",
               Year = 1999,
               Price = 80,
               Description = "Първият парфюм на Брус Уилис. Целеустремен, мъжествен, нетрадиционен. Силно ухание " +
               "на сандалово дърво и пикантен пипер срещат земен ветивер и витализиращ грейпфрут.",
               ChemicalIngredients = "Дървесен Ароматен",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/lr_brus_willis_parfum_primelr.jpg",
               PlateNumber = "3505",
               CategoryId = 7
           },
           new LrProduct
           {
               Name = "Мъжки парфюм Guido Maria Kretschmer LR",
               PackagingVolume = "50",
               Year = 1999,
               Price = 70,
               Description = "Като международен моден дизайнер Гуидо Мария Кречмер от години създава мода за мъже и жени.Част от перфектната визия е и аромат, който подчертава личността." +
               "И какво би било по-естествено от създаването на собствена ароматна колекция за мъже и жени? В двата парфюма е вложено много от Гуидо: стилна елегантност, любов към детайла и нотка интернационално излъчване." +
               "Докато дамският парфюм омагьосва с ароматен букет от флорални, елегантни и нежни есенции, мъжкият – завладява със своята топла, пикантна и чаровна ароматна композиция.",
               ChemicalIngredients = "бергамот, мандарина, мек шинус, здравец, дърво от кашмир,ванилия",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/guido-maria-kretscher-lr-man.jpg",
               PlateNumber = "30200",
               CategoryId = 7
           },
           new LrProduct
           {
               Name = "Парфюм LR Rockin' Romance",
               PackagingVolume = "50",
               Year = 1999,
               Price = 52,
               Description = "Вдъхновяващо свеж. Уникалната композиция е заредена с енергията на витализиращи цитрусови плодове и чувствеността на романтична роза, ценен иланг-иланг и топло кедрово дърво.",
               ChemicalIngredients = "портокал, лимон, масло от канела,гардения, роза, жасмин, иланг-иланг,лек амбър, скъпоценни видове мускус, кедрово дърво",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/full_rockin_romance_parfum.jpg",
               PlateNumber = "3250",
               CategoryId = 1
           },
           new LrProduct
           {
               Name = "Дамски парфюм LR Lovingly by Bruce WillIis",
               PackagingVolume = "50",
               Year = 1999,
               Price = 80,
               Description = "Ексклузивно: Първият дамски аромат от Брус Уилис.Много лична история вдъхнови световната звезда да подари парфюм на своята съпруга Ема Хеминг-Уилис. Lovingly е създаден като символ на неговата любов." +
               "Ароматът е толкова страстен, колкото и това уникално любовно обяснение. С букет от бели цветя и нотка на свежи цитрусови плодове този парфюм ви обгръща в чувствена аура, изпълнена с очарование и жизнерадост." +
               "Сандалово дърво и мускус придават на аромата топлина и романтика. Това е истинска любов!",
               ChemicalIngredients = "цитруси, круша, жасмин, лилия, божур, тубероза, бял кедър, сандалово дърво, мускус.",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/lr-lovengly-parfum-maglr.jpg",
               PlateNumber = "3630",
               CategoryId = 1
           },
           new LrProduct
           {
               Name = "Дамски парфюм Guido Maria Kretschmer LR",
               PackagingVolume = "50",
               Year = 1999,
               Price = 80,
               Description = "Като международен моден дизайнер Гуидо Мария Кречмер от години създава мода за мъже и жени.Част от перфектната визия е и аромат, който подчертава личността." +
               "И какво би било по-естествено от създаването на собствена ароматна колекция за мъже и жени? В двата парфюма е вложено много от Гуидо: стилна елегантност, любов към детайла и нотка интернационално излъчване." +
               "Докато дамският парфюм омагьосва с ароматен букет от флорални, елегантни и нежни есенции, мъжкият – завладява със своята топла, пикантна и чаровна ароматна композиция.",
               ChemicalIngredients = "бергамот, мандарина, слива, круша,роза, жасмин, портокалов цвят, лотос,кехлибар, сандалово дърво, ванилия, мускус.",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/guido-maria-kretscher-lr-woman.jpg",
               PlateNumber = "30220",
               CategoryId = 1
           },
           new LrProduct
           {
               Name = "Парфюмна вода 'Стокхолм'",
               PackagingVolume = "50",
               Year = 1999,
               Price = 28.70m,
               Description = "Стокхолм е град на модата, дизайна и специалния стил, въплътен в парфюма със същото име. Стилен, но традиционен аромат, изпълнен с кедрови и бергамотови интонации, хармонично балансиран от благородни кехлибарени нотки.",
               ChemicalIngredients = "бергамот, лимон, анасон,кедър,кехлибар, мускус.",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/full_lr_classics_stokgolme-1.jpg",
               PlateNumber = "32956",
               CategoryId = 7
           },
           new LrProduct
           {
               Name = "Алое Вера Контурен гел за тяло",
               PackagingVolume = "200",
               Year = 1999,
               Price = 49.40m,
               Description = "Попива бързо,овлажнява и освежава,възстановява естествения воден баланс.Кожата става,мека и добре поддържана.Доставя усещане за свежест и елиминира ефекта на портокалова кора." +
               "При ежедневна употреба стяга коремната област,придава еластичност на кожата и прави тъканите по - еластични",
               ChemicalIngredients = "Алое Вера гел 30%,eкстракт от зелен чай.Патентован състав на съставките InterSlim.",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/08/thumb_body-3.jpg",
               PlateNumber = "27536",
               CategoryId = 2
           },
           new LrProduct
           {
               Name = "Алое Вера коригиращ крем за тяло",
               PackagingVolume = "200",
               Year = 1999,
               Price = 49.40m,
               Description = " Попива бързо,овлажнява и освежава,кожата става стегната и еластична,доставя усещане за свежест." +
               "Елиминира ефекта на портокалова кора и освежава кожата като прави повърхността й равномерна и гладка.Намалява площта на подкожно отлагане на мазнини,Осигурява анти - ейдж ефект",
               ChemicalIngredients = "Алое Вера гел 30%,eкстракт от зелен чай.Патентовани съставки Legance.",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/08/thumb_body-2.jpg",
               PlateNumber = "27535",
               CategoryId = 2
           },
           new LrProduct
           {
               Name = "Алое Вера рол-он дезодорант",
               PackagingVolume = "50",
               Year = 1999,
               Price = 11.36m,
               Description = "Модерен дълготраен продукт за лична хигиена.Предотвратява прекомерното изпотяване и образуването на миризма,може да се използва след бръснене и епилация,съдържа екстракт от памук." +
               "Ефективен през целия ден,лесно се разнася по кожата,не оставя усещане за лепкавост и следи по дрехите.Не съдържа алкохол,съчетава се с всеки от любимите ви аромати",
               ChemicalIngredients = "Алое Вера гел 15%,Екстракт от памучно семе (BIO).",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/thumb_deo.jpg",
               PlateNumber = "20643",
               CategoryId = 2
           },
           new LrProduct
           {
               Name = "Алое Вера Хидратиращ лосион за тяло",
               PackagingVolume = "200",
               Year = 1999,
               Price = 29.39m,
               Description = "Нежно се грижи за кожата, насищайки я с влага.Прави кожата мека и гладка.Поддържа естествения водно - липиден баланс на кожата,подобрява тонуса,без парабени и вазелин." +
               "Има приятна копринена текстура и бързо помага за възстановяване на кожата след душ(вана,плажа),предотвратява сухота",
               ChemicalIngredients = "Алое Вера гел 69%,екстракт от магнолия,витамин Е,масло от шеа.",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/thumb_body.jpg",
               PlateNumber = "20639",
               CategoryId = 2
           },
           new LrProduct
           {
               Name = "Алое Вера Паста за зъби-гел",
               PackagingVolume = "100",
               Year = 1999,
               Price = 10.69m,
               Description = "Има превантивен ефект срещу кариес и заболявания на венците,Премахва старателно плаката и се бори срещу основната причина за нейното образуване - бактериите." +
               "Има подчертан положителен ефект върху проблемни венци благодарение на комбинацията от гел от алое вера,екстракт от ехинацея и прополис." +
               "Не съдържа флуорид,затова се препоръчва особено за региони с високо съдържание на флуорид в питейната вода или с флуороза,има подчертан и дълготраен освежаващ ефект",
               ChemicalIngredients = "Алое Вера гел 69%,екстракт от ехинацея,прополис.",
               PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/thumb_tooth.jpg",
               PlateNumber = "20690",
               CategoryId = 2
           }});


            data.SaveChanges();
            string userId = data.LrUsers.Select(x => x.Id).FirstOrDefault();
            foreach (var productId in data.LrProducts.Select(x => x.Id).ToList())
            {
                var userProduct = new UserProduct { LrUserId = userId, LrProductId = productId };
                data.UserProducts.Add(userProduct);
                data.SaveChanges();
            }
        }*/

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string admin1Email = "ankonikolchevpl@gmail.com";
                    const string admin1Password = "admin123";

                    var user1 = new User
                    {
                        Email = admin1Email,
                        UserName = admin1Email,
                        FullName = "Admin1"
                    }; 
                    const string admin2Email = "niki@gmail.com";
                    const string admin2Password = "admin1234";

                    var user2 = new User
                    {
                        Email = admin2Email,
                        UserName = admin2Email,
                        FullName = "Admin2"
                    };

                    await userManager.CreateAsync(user1, admin1Password);
                    await userManager.AddToRoleAsync(user1, role.Name);

                    await userManager.CreateAsync(user2, admin2Password);
                    await userManager.AddToRoleAsync(user2, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}


