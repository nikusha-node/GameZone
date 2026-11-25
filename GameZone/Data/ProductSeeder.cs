
using GameZone.Enums;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Data
{
    public static class ProductSeeder
    {
        public static async Task SeedAsync(DataContext context)
        {

            var brandDefinitions = new List<Brand>
            {
                new Brand
                {
                    Name = "NVIDIA",
                    Slug = "nvidia",
                    Description = "Graphics processing technology",
                    LogoUrl = "https://cdn.simpleicons.org/nvidia"
                },
                new Brand
                {
                    Name = "AMD",
                    Slug = "amd",
                    Description = "Processors and graphics technology",
                    LogoUrl = "https://cdn.simpleicons.org/amd"
                },
                new Brand
                {
                    Name = "Intel",
                    Slug = "intel",
                    Description = "Computer processors",
                    LogoUrl = "https://cdn.simpleicons.org/intel"
                },
                new Brand
                {
                    Name = "ASUS",
                    Slug = "asus",
                    Description = "Computer hardware and gaming products",
                    LogoUrl = "https://cdn.simpleicons.org/asus"
                },
                new Brand
                {
                    Name = "Sony",
                    Slug = "sony",
                    Description = "Gaming consoles and electronics",
                    LogoUrl = "https://cdn.simpleicons.org/sony"
                },
                new Brand
                {
                    Name = "Logitech",
                    Slug = "logitech",
                    Description = "Gaming peripherals and accessories",
                    LogoUrl = "https://cdn.simpleicons.org/logitech"
                },
                new Brand
                {
                    Name = "Samsung",
                    Slug = "samsung",
                    Description = "Displays and electronics",
                    LogoUrl = "https://cdn.simpleicons.org/samsung"
                },
                new Brand
                {
                    Name = "Corsair",
                    Slug = "corsair",
                    Description = "Gaming hardware and PC components",
                    LogoUrl = "https://cdn.simpleicons.org/corsair"
                },
                new Brand
                {
                    Name = "Rockstar Games",
                    Slug = "rockstar-games",
                    Description = "Video game publisher",
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmz41qXRbHa97uFVGmA0tLXcgT87EieVQl-GFpx5IarQ&s"
                },
                new Brand
                {
                    Name = "Microsoft",
                    Slug = "microsoft",
                    Description = "Gaming and technology company",
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5CmUEh8QMtWhIPQ1MLZUzoZNNUF-aV-8ZSeSxZ06ypQ&s=10"
                },
                new Brand
                {
                    Name = "Capcom",
                    Slug = "capcom",
                    Description = "Video game developer and publisher",
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSPt-2pVnQf3F3W-14sJz10WO6IrAUHpOVH0RjGkfXNWw&s=10"
                },
                new Brand
                {
                    Name = "Mojang Studios",
                    Slug = "mojang-studios",
                    Description = "Video game developer",
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSuiJIAxDNx4GIAbovyt6kuE-bcbQwyZHe-97_rDf3gYA&s=10"
                },
                new Brand
                {
                    Name = "CD Projekt Red",
                    Slug = "cd-projekt-red",
                    Description = "Video game developer and publisher",
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQSjBEw1CLexpFue5EVjZkdWumONF0f20KsaE93dAOpVw&s=10"
                },
                new Brand
                {
                    Name = "Electronic Arts",
                    Slug = "electronic-arts",
                    Description = "Video game publisher",
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR-SlbzDFn1z61hwPJiCOmwI4U4fUXASp_CP9rAMOyenQ&s=10"
                },
                new Brand
                {
                    Name = "Nintendo",
                    Slug = "nintendo",
                    Description = "Video game developer and publisher",
                    LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTGr7IMhKd6om7PL9gmObYMZEIIx9zeTUT5tU0svGCDhw&s=10"
                }
            };

            foreach (var brand in brandDefinitions)
            {
                var existingBrand = await context.Brands
                    .FirstOrDefaultAsync(b => b.Name == brand.Name);

                if (existingBrand == null)
                {
                    context.Brands.Add(brand);
                }
                else
                {
                    existingBrand.LogoUrl = brand.LogoUrl;
                }
            }

            await context.SaveChangesAsync();

            var platformDefinitions = new List<Platform>
            {
                new Platform { Name = "PlayStation 4", Manufacturer = "Sony" },
                new Platform { Name = "PlayStation 5", Manufacturer = "Sony" },
                new Platform { Name = "Xbox One", Manufacturer = "Microsoft" },
                new Platform { Name = "Xbox Series X/S", Manufacturer = "Microsoft" },
                new Platform { Name = "Nintendo Switch", Manufacturer = "Nintendo" }
            };

            foreach (var platform in platformDefinitions)
            {
                if (!await context.Platforms.AnyAsync(p => p.Name == platform.Name))
                {
                    context.Platforms.Add(platform);
                }
            }

            await context.SaveChangesAsync();

            var pcPlatform = await context.Platforms
                .FirstOrDefaultAsync(p => p.Name == "PC");

            if (pcPlatform != null)
            {
                var gamesWithPc = await context.Games
                    .Include(g => g.Platforms)
                    .Where(g => g.Platforms.Any(p => p.Id == pcPlatform.Id))
                    .ToListAsync();

                foreach (var game in gamesWithPc)
                {
                    game.Platforms.Remove(pcPlatform);
                }

                context.Platforms.Remove(pcPlatform);

                await context.SaveChangesAsync();
            }


            var genreDefinitions = new List<Genre>
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Horror" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Shooter" },
                new Genre { Name = "Sports" },
                new Genre { Name = "Simulation" },
                new Genre { Name = "Strategy" },
                new Genre { Name = "Fighting" },
                new Genre { Name = "Puzzle" },
                new Genre { Name = "Open World" }
            };

            foreach (var genre in genreDefinitions)
            {
                if (!await context.Genres.AnyAsync(g => g.Name == genre.Name))
                {
                    context.Genres.Add(genre);
                }
            }

            await context.SaveChangesAsync();

            var brands = await context.Brands.ToDictionaryAsync(b => b.Name);
            var gamePlatforms = await context.Platforms.ToDictionaryAsync(p => p.Name);
            var gameGenres = await context.Genres.ToDictionaryAsync(g => g.Name);

            var sampleProducts = new (
                string Name,
                string Description,
                decimal Price,
                int Stock,
                string? ImageUrl,
                string Brand,
                Category Category)[]
            {
                (
                    "GeForce RTX 5070 Ti",
                    "High-performance NVIDIA graphics card for modern gaming.",
                    2899.99m,
                    8,
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTgDWOvlW3V0JRPZa5BY1UcL9ytOWMfpELOmxBUYCS9Vw&s=10",
                    "NVIDIA",
                    Category.GPU
                ),
                (
                    "GeForce RTX 4060",
                    "Efficient graphics card for 1080p and 1440p gaming.",
                    1099.99m,
                    12,
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTfKpzqJ7GGSSDNbggpWzzmh0OoWSHtD26DAst3K9DiZw&s=10",
                    "NVIDIA",
                    Category.GPU
                ),
                (
                    "AMD Ryzen 7 7800X3D",
                    "High-performance gaming processor with 3D V-Cache.",
                    1199.99m,
                    6,
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ143PsnnoTOnu2MBBPcm8iMpFTERrhoS3A6QWpc99oJg&s",
                    "AMD",
                    Category.CPU
                ),
                (
                    "Intel Core i7-14700K",
                    "Powerful desktop processor for gaming and productivity.",
                    999.99m,
                    7,
                    "https://gamezone.ge/images/thumbnails/225/225/detailed/27/%E1%83%AA26_a9xi-tf.jpg.png",
                    "Intel",
                    Category.CPU
                ),
                (
                    "ASUS ROG STRIX B650E-F",
                    "AM5 gaming motherboard with advanced connectivity.",
                    899.99m,
                    5,
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5Sgt_mJeE-4nmsHhIQ5Y2BOK4XdLJowQkmkacOGeUHg&s=10",
                    "ASUS",
                    Category.Motherboard
                ),
                (
                    "Corsair Vengeance 32GB DDR5",
                    "32GB DDR5 memory kit for modern gaming PCs.",
                    399.99m,
                    15,
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTLlUAqAadjiO_1qD-ShkkgHlQS4tvnQq0_1v96vXg3CQ&s=10",
                    "Corsair",
                    Category.RAM
                ),
                (
                    "PlayStation 5 Slim",
                    "Sony PlayStation 5 Slim gaming console.",
                    1399.99m,
                    4,
                    "https://gamezone.ge/images/thumbnails/570/570/detailed/20/1_ethl-2d.jpg.png",
                    "Sony",
                    Category.Consoles
                ),
                (
                    "Samsung Odyssey G5 27",
                    "27-inch gaming monitor with high refresh rate.",
                    749.99m,
                    9,
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRFS72c-Wmc8b3wHA3-yfZxSZa38t4xQ4RQvgEBvYFe2w&s=10",
                    "Samsung",
                    Category.Monitors
                ),
                (
                    "Logitech G Pro X Superlight",
                    "Lightweight wireless gaming mouse.",
                    349.99m,
                    11,
                    "https://gamezone.ge/images/thumbnails/570/570/detailed/31/69a68403-2b5c-4158-a4cc-2d31858b5ee2_Thumb.webp.png",
                    "Logitech",
                    Category.Peripherals
                ),
                (
                    "Corsair 4000D Airflow",
                    "Mid-tower PC case designed for excellent airflow.",
                    299.99m,
                    6,
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShePGzP2osT3msjqGTq-0N0n5-hxZihmJRiQudFtz8Tw&s=10",
                    "Corsair",
                    Category.Case
                )
            };

            foreach (var item in sampleProducts)
            {
                var existingProduct = await context.Products
                    .FirstOrDefaultAsync(p => p.Name == item.Name);

                if (existingProduct != null)
                {
                    existingProduct.ImageUrl = item.ImageUrl;
                    continue;
                }

                var brand = brands[item.Brand];

                context.Products.Add(new Product
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    StockQuantity = item.Stock,
                    BrandId = brand.Id,
                    Brand = brand,
                    Category = item.Category,
                    ImageUrl = item.ImageUrl
                });
            }

            await context.SaveChangesAsync();

            (string Name, string Description, string ImageUrl,
            decimal Price, decimal? OriginalPrice, string Developer,
            string Publisher, DateTime ReleaseDate, AgeRating AgeRating,
            string[] Platforms, string[] Genres)[] gameDefinitions =
            {
                (
                    Name: "Grand Theft Auto V",
                    Description: "Explore Los Santos in this open-world action adventure.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/14/71rmY66nqoL._SL1361_.jpg.png",
                    Price: 89.99m,
                    OriginalPrice: (decimal?)129.99m,
                    Developer: "Rockstar North",
                    Publisher: "Rockstar Games",
                    ReleaseDate: new DateTime(2013, 9, 17),
                    AgeRating: AgeRating.PEGI18,
                    Platforms: new[] { "PlayStation 4", "PlayStation 5", "Xbox One", "Xbox Series X/S" },
                    Genres: new[] { "Action", "Open World" }
                ),
                (
                    Name: "Forza Horizon 5",
                    Description: "Experience an open-world racing adventure in Mexico.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/10/71LSwnEXpXL._SL1500_.jpg.png",
                    Price: 119.99m,
                    OriginalPrice: (decimal?)null,
                    Developer: "Playground Games",
                    Publisher: "Microsoft",
                    ReleaseDate: new DateTime(2021, 11, 9),
                    AgeRating: AgeRating.PEGI3,
                    Platforms: new[] { "Xbox One", "Xbox Series X/S" },
                    Genres: new[] { "Racing", "Simulation" }
                ),
                (
                    Name: "Resident Evil 4",
                    Description: "Survive a terrifying horror adventure.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/15/resident-evil-4-PS5_q4qn-fv.jpg.png",
                    Price: 149.99m,
                    OriginalPrice: (decimal?)179.99m,
                    Developer: "Capcom",
                    Publisher: "Capcom",
                    ReleaseDate: new DateTime(2023, 3, 24),
                    AgeRating: AgeRating.PEGI18,
                    Platforms: new[] { "PlayStation 4", "PlayStation 5", "Xbox Series X/S" },
                    Genres: new[] { "Horror", "Action" }
                ),
                (
                    Name: "Minecraft",
                    Description: "Build, explore and survive in an endless block world.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/26/PS5_Minecraft.jpg.png",
                    Price: 69.99m,
                    OriginalPrice: (decimal?)null,
                    Developer: "Mojang Studios",
                    Publisher: "Mojang Studios",
                    ReleaseDate: new DateTime(2011, 11, 18),
                    AgeRating: AgeRating.PEGI7,
                    Platforms: new[] { "PlayStation 4", "PlayStation 5", "Xbox One", "Xbox Series X/S", "Nintendo Switch" },
                    Genres: new[] { "Adventure", "Simulation" }
                ),
                (
                    Name: "Cyberpunk 2077",
                    Description: "Explore Night City in a futuristic open-world RPG.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/20/Untitled-1_npka-gh.jpg.png",
                    Price: 129.99m,
                    OriginalPrice: (decimal?)null,
                    Developer: "CD Projekt Red",
                    Publisher: "CD Projekt Red",
                    ReleaseDate: new DateTime(2020, 12, 10),
                    AgeRating: AgeRating.PEGI18,
                    Platforms: new[] { "PlayStation 5", "Xbox Series X/S" },
                    Genres: new[] { "RPG", "Action", "Open World" }
                ),
                (
                    Name: "EA Sports FC 26",
                    Description: "Experience modern football with your favourite teams.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/26/fc-26-tbilisi-georgia.jpg.png",
                    Price: 169.99m,
                    OriginalPrice: (decimal?)null,
                    Developer: "EA Sports",
                    Publisher: "Electronic Arts",
                    ReleaseDate: new DateTime(2025, 9, 26),
                    AgeRating: AgeRating.PEGI3,
                    Platforms: new[] { "PlayStation 5", "Xbox Series X/S" },
                    Genres: new[] { "Sports" }
                ),
                (
                    Name: "Red Dead Redemption 2",
                    Description: "Experience an epic western open-world adventure.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/19/PS4-RED-DEAD-REDEMPTION-2.jpg.png",
                    Price: 99.99m,
                    OriginalPrice: (decimal?)149.99m,
                    Developer: "Rockstar Studios",
                    Publisher: "Rockstar Games",
                    ReleaseDate: new DateTime(2018, 10, 26),
                    AgeRating: AgeRating.PEGI18,
                    Platforms: new[] { "PlayStation 4", "Xbox One" },
                    Genres: new[] { "Action", "Adventure", "Open World" }
                ),
                (
                    Name: "The Witcher 3: Wild Hunt",
                    Description: "Become a monster hunter in a fantasy open world.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/14/45gg.jpg.png",
                    Price: 79.99m,
                    OriginalPrice: (decimal?)null,
                    Developer: "CD Projekt Red",
                    Publisher: "CD Projekt Red",
                    ReleaseDate: new DateTime(2015, 5, 19),
                    AgeRating: AgeRating.PEGI18,
                    Platforms: new[] { "PlayStation 4", "PlayStation 5", "Xbox One", "Xbox Series X/S", "Nintendo Switch" },
                    Genres: new[] { "RPG", "Adventure", "Open World" }
                ),
                (
                    Name: "God of War Ragnarök",
                    Description: "Join Kratos and Atreus on an epic Norse adventure.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/14/817y77i7EFL._SL1500__uz7j-gh.jpg.png",
                    Price: 159.99m,
                    OriginalPrice: (decimal?)null,
                    Developer: "Santa Monica Studio",
                    Publisher: "Sony",
                    ReleaseDate: new DateTime(2022, 11, 9),
                    AgeRating: AgeRating.PEGI18,
                    Platforms: new[] { "PlayStation 4", "PlayStation 5" },
                    Genres: new[] { "Action", "Adventure" }
                ),
                (
                    Name: "Mario Kart 8 Deluxe",
                    Description: "Race against friends and characters from the Mario universe.",
                    ImageUrl: "https://gamezone.ge/images/thumbnails/570/570/detailed/21/6950.jpg.png",
                    Price: 139.99m,
                    OriginalPrice: (decimal?)null,
                    Developer: "Nintendo",
                    Publisher: "Nintendo",
                    ReleaseDate: new DateTime(2017, 4, 28),
                    AgeRating: AgeRating.PEGI3,
                    Platforms: new[] { "Nintendo Switch" },
                    Genres: new[] { "Racing", "Sports" }
                )
            };


            foreach (var item in gameDefinitions)
            {
                var brand = brands[item.Publisher];

                var product = await context.Products
                    .FirstOrDefaultAsync(p => p.Name == item.Name);

                if (product == null)
                {
                    product = new Product
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        OriginalPrice = item.OriginalPrice,
                        IsOnSale = item.OriginalPrice.HasValue,
                        StockQuantity = 10,
                        ImageUrl = item.ImageUrl,
                        BrandId = brand.Id,
                        Brand = brand,
                        Category = Category.Games
                    };

                    context.Products.Add(product);
                    await context.SaveChangesAsync();
                }
                else
                {
                    product.Description = item.Description;
                    product.Price = item.Price;
                    product.OriginalPrice = item.OriginalPrice;
                    product.IsOnSale = item.OriginalPrice.HasValue;
                    product.ImageUrl = item.ImageUrl;
                    product.BrandId = brand.Id;
                    product.Brand = brand;
                    product.Category = Category.Games;
                }

                var platformsForGame = item.Platforms
                    .Select(p => gamePlatforms[p])
                    .ToList();

                var genresForGame = item.Genres
                    .Select(g => gameGenres[g])
                    .ToList();

                var game = await context.Games
                    .Include(g => g.Platforms)
                    .Include(g => g.Genres)
                    .FirstOrDefaultAsync(g => g.ProductId == product.Id);

                if (game == null)
                {
                    game = new Game
                    {
                        Name = item.Name,
                        Description = item.Description,
                        ReleaseDate = item.ReleaseDate,
                        Developer = item.Developer,
                        Publisher = item.Publisher,
                        ProductId = product.Id,
                        Product = product,
                        AgeRating = item.AgeRating,
                        Platforms = platformsForGame,
                        Genres = genresForGame
                    };

                    context.Games.Add(game);
                }
                else
                {
                    game.Name = item.Name;
                    game.Description = item.Description;
                    game.ReleaseDate = item.ReleaseDate;
                    game.Developer = item.Developer;
                    game.Publisher = item.Publisher;
                    game.AgeRating = item.AgeRating;
                    game.Platforms = platformsForGame;
                    game.Genres = genresForGame;
                }
            }

            await context.SaveChangesAsync();
        }
    }
}


