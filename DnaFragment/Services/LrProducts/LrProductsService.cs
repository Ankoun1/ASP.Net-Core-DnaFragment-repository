

namespace DnaFragment.Services.LrProducts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models;
    using DnaFragment.Services.LrProducts.Models;
    using Microsoft.EntityFrameworkCore;

    public class LrProductsService : ILrProductsService
    {
        private readonly DnaFragmentDbContext data;
        private readonly IConfigurationProvider mapper;

        public LrProductsService(DnaFragmentDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void StartLrProduktDb()
        {
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
                    Model = "Мъжки парфюм LR Bruce Willis",
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
                    Model = "Мъжки парфюм Guido Maria Kretschmer LR",
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
                    Model = "Парфюм LR Rockin' Romance",
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
                    Model = "Дамски парфюм LR Lovingly by Bruce WillIis",
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
                    Model = "Дамски парфюм Guido Maria Kretschmer LR",
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
                    Model = "Парфюмна вода 'Стокхолм'",
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
                    Model = "Алое Вера Контурен гел за тяло",
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
                    Model = "Алое Вера коригиращ крем за тяло",
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
                    Model = "Алое Вера рол-он дезодорант",
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
                    Model = "Алое Вера Хидратиращ лосион за тяло",
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
                    Model = "Алое Вера Паста за зъби-гел",
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
                } });

            data.SaveChanges();

            string userId = data.Users.Select(x => x.Id).FirstOrDefault();
            foreach (var productId in data.LrProducts.Select(x => x.Id).ToList())
            {
                var userProduct = new UserProduct { UserId = userId, LrProductId = productId };
                data.UserProducts.Add(userProduct);
                data.SaveChanges();
            }

            StartStatisticsProduct();
        }

        private void StartStatisticsProduct()
        {
            var statisticsProducts = new List<StatisticsProduct>();
            for (int i = 1; i <= 7; i++)
            {
                var category = data.Categories.Where(x => x.Id == i).FirstOrDefault();

                for (int j = 0; j < category.LrProducts.Count(); j++)
                {
                    var product = data.LrProducts.Select(x => new { Id = x.Id, PlateNumber = x.PlateNumber }).Skip(j).FirstOrDefault();

                    statisticsProducts.Add(new StatisticsProduct { StatisticsCategoryId = i, PlateNumber = product.PlateNumber });
                }
            }
            data.StatisticsProducts.AddRange(statisticsProducts);
            data.SaveChanges();
        }

        public int Create(
                string model,
                string packagingVolume,
                string chemicalIngredients,
                string description,
                decimal price,
                int year,
                string image,
                string plateNumber,
                int categoryId)
        {
            var product = new LrProduct
            {
                Model = model,
                PackagingVolume = packagingVolume,
                ChemicalIngredients = chemicalIngredients,
                Description = description,
                Price = price,
                Year = year,
                PictureUrl = image,
                PlateNumber = plateNumber,
                CategoryId = categoryId
            };

            this.data.LrProducts.Add(product);
            UpdateLrUserStatisticsProducts(plateNumber, categoryId);

            //CreateUserProduct(product.Id, userId);           

            return product.Id;
        }

        private void UpdateLrUserStatisticsProducts(string plateNumber, int categoryId)
        {
            var lrProduct = new StatisticsProduct { StatisticsCategoryId = categoryId, PlateNumber = plateNumber };
            data.StatisticsProducts.Add(new StatisticsProduct { StatisticsCategoryId = categoryId, PlateNumber = plateNumber });
            data.SaveChanges();

            var lrUserStatisticsProducts = new List<LrUserStatisticsProduct>();
            foreach (var user in data.LrUsers.ToList())
            {
                lrUserStatisticsProducts.Add(new LrUserStatisticsProduct { LrUserId = user.Id, StatisticsProductId = data.LrProducts.Where(x => x.PlateNumber == plateNumber).Select(x => x.Id).FirstOrDefault() });
            }
            data.LrUserStatisticsProducts.AddRange(lrUserStatisticsProducts);
            data.SaveChanges();
        }

        public bool Update(string description, decimal price, string image, string plateNumber, int productId, int categoryId)
        {
            var productData = this.data.LrProducts.Find(productId);

            if (productData == null)
            {
                return false;
            }           
                    
            productData.Description = description;         
            productData.Price = price;
            productData.PictureUrl = image;
            productData.PlateNumber = plateNumber;
            productData.CategoryId = categoryId;

            this.data.SaveChanges();

            return true;
        }


        public async Task<LrProductQueryServiceModel> All(string brand,string searchTerm, LrProductSorting sorting,int currentPage,int productsPerPage)
        {
            var productsQuery =  this.data.LrProducts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                productsQuery =   productsQuery.Where(c => c.Category.Name == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(c =>
                    (c.Model).ToLower().Contains(searchTerm.ToLower()) || c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            productsQuery = sorting switch
            {
                LrProductSorting.Volume =>   productsQuery.OrderByDescending(c => c.PackagingVolume),
                LrProductSorting.BrandAndPrice or _ => productsQuery.OrderBy(c => c.Price).ThenBy(c => c.Model)
                //LrProductSorting.DateCreated or _ => productsQuery.OrderByDescending(c => c.Id)
            };

            var totalLrProducts = await productsQuery.CountAsync();

            var products = await productsQuery
                .Skip((currentPage - 1) * productsPerPage)
                .Take(productsPerPage).Select(c => new LrProductServiceModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    PackagingVolume = c.PackagingVolume,
                    Price = c.Price,
                    PictureUrl = c.PictureUrl,
                    //Category = c.Category.Name
                })
                .ToListAsync();
            var lrProductQueryServiceModel =   new LrProductQueryServiceModel
            {
                TotalProducts =  totalLrProducts,
                CurrentPage = currentPage,
                ProductsPerPage = productsPerPage,
                LrProducts = products
            };
            if (await data.Categories.AnyAsync())
            {
                lrProductQueryServiceModel.CategoryAny = true;
            }
             return   lrProductQueryServiceModel;
        }

        public List<LrProductServiceModel> AllProductsByCategory(int categoryId)
        {
            var products = new List<LrProductServiceModel>();
            if (categoryId != 0)
            {
                products = data.LrProducts.Where(x => x.CategoryId == categoryId).OrderBy(x => x.Price).ProjectTo<LrProductServiceModel>(mapper).ToList();
                products.Insert(0, AddImageModel());
            }
            else
            {
                products.Add(AddImageModel());
            }           

            return products;
        }

        private static LrProductServiceModel AddImageModel()
        {
            return new LrProductServiceModel
            {
                Photos = new List<string> { "https://c4.wallpaperflare.com/wallpaper/74/530/410/sweet-girl-pic-3840x2160-wallpaper-preview.jpg",
                "https://media.gettyimages.com/photos/bikini-woman-napping-in-a-hammock-at-the-caribbean-beach-picture-id125145258?k=6&m=125145258&s=612x612&w=0&h=kyver0PULttVYsRqiFCCwQ66DySg3SJj7bodsSMG83A=",
                "https://c4.wallpaperflare.com/wallpaper/577/412/12/hot-girl-pic-1920x1200-wallpaper-preview.jpg",
                "https://c4.wallpaperflare.com/wallpaper/869/515/658/pic-girl-2560x1600-wallpaper-preview.jpg",
                "https://wallup.net/wp-content/uploads/2019/09/473320-landscape-view-height-city-dal-beauty-wind-girl-sunset.jpg",
                "https://c4.wallpaperflare.com/wallpaper/898/902/381/pic-girl-2560x1600-wallpaper-thumb.jpg",
                "https://c4.wallpaperflare.com/wallpaper/898/902/381/pic-girl-2560x1600-wallpaper-thumb.jpg" }
            };
        }

        public IEnumerable<string> AllLrBrands()
        {
            return data
                .LrProducts
                .Select(c => c.Category.Name)
                .Distinct()
                .OrderBy(br => br)
                .ToList();
        }

        public LrProductDetailsServiceModel Details(int Id)
        => data.LrProducts.Where(x => x.Id == Id).Select(x => new LrProductDetailsServiceModel
        {
            Id = x.Id,
            Model = x.Model,
            PackagingVolume = x.PackagingVolume,
            Description = x.Description,
            ChemicalIngredients = x.ChemicalIngredients,
            Price = x.Price,
            PictureUrl = x.PictureUrl,
            PlateNumber = x.PlateNumber,
            CategoryId = x.CategoryId,
            //LrUserId = x.Id
        }).FirstOrDefault();

        public List<LrProductDetailsServiceModel> Favorits(string id)
        =>data.LrProducts.Where(x => x.UserProducts.Where(y => y.UserId == id).Select(y => y.UserId)
            .FirstOrDefault() == id)
            .OrderBy(x => x.CategoryId)
            .ThenBy(x => x.Price)
            .ProjectTo<LrProductDetailsServiceModel>(mapper)
            .ToList();

        public IEnumerable<LrCategoryServiceModel> AllCategories()        
           => this.data
               .Categories
               .Select(c => new LrCategoryServiceModel
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .ToList();

        public bool CategoryExsists(int categoryId)        
        =>  this.data.Categories.Any(c => c.Id == categoryId);

        public bool ExistUserProduct(int productId, string userId)
        => data.UserProducts.Any(x => x.LrProductId == productId && x.UserId == userId);

        public void CreateUserProduct(int productId, string userId)
        {
            var userProduct = new UserProduct
            {
                UserId = userId,                
                LrProductId = productId
            };

            data.UserProducts.Add(userProduct);
            data.SaveChanges();
        }

        public void UpdateCountVisitsCategory(string userName,int categoryId)
        {
            var userId = data.LrUsers.Where(x => x.Email == userName).Select(x => x.Id).FirstOrDefault();
            var statisticsCategory = data.StatisticsCategories.Where(x => x.Id == categoryId).FirstOrDefault();
            var statisticsProducts = data.LrUserStatisticsProducts.Where(x => x.StatisticsProduct.StatisticsCategoryId == statisticsCategory.Id).ToList();

            foreach (var statisticsProduct in statisticsProducts)
            {
                statisticsProduct.CategoryVisitsCount++;
                data.SaveChanges();
            }                     
        }

        public void UpdateCountVisitsProduct(string userName,int id)
        {
            var userId = data.LrUsers.Where(x => x.Email == userName).Select(x => x.Id).FirstOrDefault();
            var statisticsProduct = data.LrUserStatisticsProducts.Where(x => x.LrUserId == userId && x.StatisticsProductId == id).FirstOrDefault();
            
            statisticsProduct.ProductVisitsCount++;
            data.SaveChanges();
        }        
    }
}
