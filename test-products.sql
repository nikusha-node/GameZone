IF NOT EXISTS (SELECT 1 FROM Categories WHERE Slug = 'graphics-cards')
    INSERT INTO Categories (Name, Slug, Description) VALUES (N'Graphics Cards', N'graphics-cards', N'GPUs');
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Slug = 'processors')
    INSERT INTO Categories (Name, Slug, Description) VALUES (N'Processors', N'processors', N'CPUs');

IF NOT EXISTS (SELECT 1 FROM Brands WHERE Slug = 'nvidia') INSERT INTO Brands (Name, Slug) VALUES (N'NVIDIA', N'nvidia');
IF NOT EXISTS (SELECT 1 FROM Brands WHERE Slug = 'amd')    INSERT INTO Brands (Name, Slug) VALUES (N'AMD', N'amd');
IF NOT EXISTS (SELECT 1 FROM Brands WHERE Slug = 'intel')  INSERT INTO Brands (Name, Slug) VALUES (N'Intel', N'intel');

INSERT INTO Products (Name, Slug, Sku, Description, Price, DiscountPrice, StockQuantity, IsActive, CreatedAtUtc, CategoryId, BrandId)
SELECT N'GeForce RTX 4070', N'geforce-rtx-4070', N'GPU-4070', N'12 GB graphics card for 1440p gaming', 599.99, 549.99, 10, 1, SYSUTCDATETIME(),
       (SELECT Id FROM Categories WHERE Slug = 'graphics-cards'), (SELECT Id FROM Brands WHERE Slug = 'nvidia')
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Slug = 'geforce-rtx-4070');

INSERT INTO Products (Name, Slug, Sku, Description, Price, DiscountPrice, StockQuantity, IsActive, CreatedAtUtc, CategoryId, BrandId)
SELECT N'Radeon RX 7800 XT', N'radeon-rx-7800-xt', N'GPU-7800XT', N'16 GB graphics card', 499.99, NULL, 5, 1, SYSUTCDATETIME(),
       (SELECT Id FROM Categories WHERE Slug = 'graphics-cards'), (SELECT Id FROM Brands WHERE Slug = 'amd')
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Slug = 'radeon-rx-7800-xt');

INSERT INTO Products (Name, Slug, Sku, Description, Price, DiscountPrice, StockQuantity, IsActive, CreatedAtUtc, CategoryId, BrandId)
SELECT N'GeForce RTX 4090', N'geforce-rtx-4090', N'GPU-4090', N'Top-end 24 GB graphics card', 1799.99, NULL, 0, 1, SYSUTCDATETIME(),
       (SELECT Id FROM Categories WHERE Slug = 'graphics-cards'), (SELECT Id FROM Brands WHERE Slug = 'nvidia')
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Slug = 'geforce-rtx-4090');

INSERT INTO Products (Name, Slug, Sku, Description, Price, DiscountPrice, StockQuantity, IsActive, CreatedAtUtc, CategoryId, BrandId)
SELECT N'Ryzen 7 7800X3D', N'ryzen-7-7800x3d', N'CPU-7800X3D', N'8-core gaming processor', 399.99, NULL, 20, 1, SYSUTCDATETIME(),
       (SELECT Id FROM Categories WHERE Slug = 'processors'), (SELECT Id FROM Brands WHERE Slug = 'amd')
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Slug = 'ryzen-7-7800x3d');

INSERT INTO Products (Name, Slug, Sku, Description, Price, DiscountPrice, StockQuantity, IsActive, CreatedAtUtc, CategoryId, BrandId)
SELECT N'Core i7-14700K', N'core-i7-14700k', N'CPU-14700K', N'20-core processor', 409.99, 379.99, 12, 1, SYSUTCDATETIME(),
       (SELECT Id FROM Categories WHERE Slug = 'processors'), (SELECT Id FROM Brands WHERE Slug = 'intel')
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Slug = 'core-i7-14700k');

INSERT INTO ProductImages (Url, AltText, IsPrimary, SortOrder, ProductId)
SELECT N'https://example.com/rtx4070.jpg', N'RTX 4070', 1, 0, Id FROM Products
WHERE Slug = 'geforce-rtx-4070' AND NOT EXISTS (SELECT 1 FROM ProductImages WHERE ProductId = Products.Id);

INSERT INTO ProductSpecifications (Name, Value, SortOrder, ProductId)
SELECT N'VRAM', N'12 GB', 0, Id FROM Products
WHERE Slug = 'geforce-rtx-4070' AND NOT EXISTS (SELECT 1 FROM ProductSpecifications WHERE ProductId = Products.Id);

INSERT INTO ProductSpecifications (Name, Value, SortOrder, ProductId)
SELECT N'Power', N'200 W', 1, Id FROM Products
WHERE Slug = 'geforce-rtx-4070' AND (SELECT COUNT(*) FROM ProductSpecifications WHERE ProductId = Products.Id) < 2;