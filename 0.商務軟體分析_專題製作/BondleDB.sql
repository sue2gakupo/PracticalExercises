-- Bondle商品管理與販售平台資料庫建立語法
-- SQL Server Database Schema

-- 建立資料庫
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'BondleDB')
begin
	use master
	DROP DATABASE BondleDB
end

CREATE DATABASE BondleDB;
GO

USE BondleDB;
GO

-- 1. 會員登入資料表
CREATE TABLE Member (
    MemberID nchar(36) NOT NULL,
    Email nvarchar(255) NOT NULL,
    PasswordHash nvarchar(512) NOT NULL,
    GoogleUserID nvarchar(100) NULL,
    Name nvarchar(50) NULL,
    IsEmailVerified bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    LastLoginDate datetime NULL DEFAULT GETDATE(),
    Status tinyint NOT NULL DEFAULT 1, -- 1:正常 2:停用 3:鎖定
    CONSTRAINT PK_Member PRIMARY KEY (MemberID),
    CONSTRAINT UQ_Member_Email UNIQUE (Email)
);

-- 插入會員資料 (需包含所有必要的會員ID)
INSERT INTO Member (MemberID, Email, PasswordHash, Name, IsEmailVerified, Status)
VALUES
    ('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', 'creator_a@example.com', 'hash_for_creator_a', N'王創作者', 1, 1),
    ('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 'supporter_b@example.com', 'hash_for_supporter_b', N'陳支持者', 1, 1),
    ('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', 'creator_c@example.com', 'hash_for_creator_c', N'林創作者', 1, 1),
    ('a1b2c3d4-e5f6-7890-1234-567890abcdef', 'disabled_user@example.com', 'hash_for_disabled_user', N'停用會員', 1, 2);

-- 2. 會員角色表
CREATE TABLE MemberRoles (
    MemberID nchar(36) NOT NULL,
    IsCreator bit NOT NULL DEFAULT 0,
    IsSupporter bit NOT NULL DEFAULT 0,
    CONSTRAINT PK_MemberRoles PRIMARY KEY (MemberID),
    CONSTRAINT FK_MemberRoles_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);

INSERT INTO MemberRoles (MemberID, IsCreator, IsSupporter)
VALUES
    ('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', 1, 1), -- 創作者也是支持者
    ('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 0, 1), -- 僅為支持者
    ('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', 1, 0), -- 僅為創作者
    ('a1b2c3d4-e5f6-7890-1234-567890abcdef', 0, 0); -- 沒有特殊身分

-- 3. 商品分類表 (全站統一，管理員維護)
CREATE TABLE Categories (
    CategoryID nchar(8) NOT NULL,
    CategoryName nvarchar(50) NOT NULL,
    Description nvarchar(200) NULL,
    ParentCategoryID nchar(8) NULL,
    IconUrl nvarchar(500) NULL,
    SortOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1,
    CONSTRAINT PK_Categories PRIMARY KEY (CategoryID),
    CONSTRAINT FK_Categories_Parent FOREIGN KEY (ParentCategoryID) REFERENCES Categories(CategoryID)
);

-- 建立根分類
INSERT INTO Categories (CategoryID, CategoryName, Description, ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000001', '藝術作品', '各類藝術創作', NULL, 1, 1),
('CA000002', '插畫設計', '插畫與設計作品', NULL, 2, 1),
('CA000003', '攝影作品', '攝影相關商品', NULL, 3, 1),
('CA000004', '手作商品', '手工製作商品', NULL, 4, 1),
('CA000005', '數位素材', '數位設計素材', NULL, 5, 1);

-- 建立子分類
INSERT INTO Categories (CategoryID, CategoryName, Description, ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000006', '貼紙', '各式貼紙商品', 'CA000002', 1, 1),
('CA000007', '明信片', '明信片商品', 'CA000002', 2, 1),
('CA000008', '海報', '海報商品', 'CA000002', 3, 1);

-- 4. 創作者表
CREATE TABLE Creator (
    MemberID nchar(36) NOT NULL,
    CreatorName nvarchar(50) NOT NULL,
    Biography ntext NULL,
    AvatarUrl nvarchar(500) NULL,
    CoverUrl nvarchar(500) NULL,
    VerificationStatus tinyint NULL,
    BankAccount nvarchar(50) NULL,
    BankCode nvarchar(10) NULL,
    AccountHolderName nvarchar(50) NULL,
    ECPayMerchantID nvarchar(20) NULL,
    ECPayStatus tinyint NULL,
    PlatformFeeRate decimal(5,3) NOT NULL DEFAULT 5.000,
    ApplyDate datetime NOT NULL DEFAULT GETDATE(),
    VerificationDate datetime NULL,
    Status tinyint NOT NULL DEFAULT 1,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Creator PRIMARY KEY (MemberID),
    CONSTRAINT FK_Creator_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);

INSERT INTO Creator (MemberID, CreatorName, Biography, VerificationStatus, ECPayStatus, ECPayMerchantID, Status)
VALUES
('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', N'創作者A的空間', N'這是一位充滿熱情的創作者，專注於插畫與設計。', NULL, NULL, NULL, 1),
('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', N'林的作品集', N'主要分享生活攝影與短片教學。', NULL, NULL, NULL, 1);

-- 5. 支持者表
CREATE TABLE Supporter (
    MemberID nchar(36) NOT NULL,
    SupporterName nvarchar(50) NULL,
    AvatarUrl nvarchar(500) NULL,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Supporter PRIMARY KEY (MemberID),
    CONSTRAINT FK_Supporter_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);

INSERT INTO Supporter (MemberID, SupporterName, AvatarUrl)
VALUES
('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', N'陳大支持', 'https://example.com/images/avatars/supporter_b.jpg');

-- 6. 系列分類表 (創作者自定義)
CREATE TABLE ProductSeries (
    SeriesID nchar(8) NOT NULL,
    MemberID nchar(36) NOT NULL,
    SeriesName nvarchar(80) NOT NULL,
    Description ntext NULL,
    CoverImageUrl nvarchar(500) NULL,
    Tags nvarchar(200) NULL,
    SortOrder int NOT NULL DEFAULT 0,
    IsPublic bit NOT NULL DEFAULT 1,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL,
    CONSTRAINT PK_ProductSeries PRIMARY KEY (SeriesID),
    CONSTRAINT FK_ProductSeries_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);

INSERT INTO ProductSeries (SeriesID, MemberID, SeriesName, Description, IsPublic)
VALUES
('SE000001', '50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', N'城市風景系列', N'一系列以城市為主題的插畫作品。', 1),
('SE000002', 'e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', N'動物攝影集', N'來自世界各地野生動物的攝影作品。', 1);

-- 7. 商品主檔表
CREATE TABLE Product (
    ProductID nchar(8) NOT NULL,
    MemberID nchar(36) NOT NULL,
    ProductName nvarchar(40) NOT NULL,
    Description ntext NULL,
    ProductType tinyint NOT NULL, -- 1:數位商品 2:實體商品
    CategoryID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    Price money NOT NULL,
    Status tinyint NOT NULL DEFAULT 1, -- 1:上架中 2:已下架 3:暫停販售
    LaunchDate datetime NULL,
    OfflineDate datetime NULL,
    SortOrder int NOT NULL DEFAULT 0,
    PurchaseCount int NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Product PRIMARY KEY (ProductID),
    CONSTRAINT FK_Product_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    CONSTRAINT FK_Product_Series FOREIGN KEY (SeriesID) REFERENCES ProductSeries(SeriesID),
    CONSTRAINT CHK_Product_Price CHECK (Price >= 0)
);

INSERT INTO Product (ProductID, MemberID, ProductName, Description, ProductType, CategoryID, SeriesID, Price, Status)
VALUES
('DI000001', '50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', N'城市夜景插畫電子圖檔', N'高解析度數位插畫，可用於印刷或螢幕背景。', 1, 'CA000002', 'SE000001', 50.00, 1),
('PH000001', 'e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', N'動物攝影明信片套組', N'包含10張精選動物攝影的實體明信片。', 2, 'CA000007', 'SE000002', 150.00, 1);

-- 8. 商品款式表
CREATE TABLE ProductVariations (
    VariationID nchar(8) NOT NULL,
    ProductID nchar(8) NOT NULL,
    SKU nvarchar(50) NOT NULL,
    VariationName nvarchar(50) NOT NULL,
    Color nvarchar(30) NULL,
    Size nvarchar(20) NULL,
    Material nvarchar(30) NULL,
    Edition nvarchar(20) NULL,
    PriceDifference money NULL,
    Stock int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1,
    SortOrder int NOT NULL DEFAULT 0,
    IsDefault bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ProductVariations PRIMARY KEY (VariationID),
    CONSTRAINT FK_ProductVariations_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

-- 為兩個商品都建立款式
INSERT INTO ProductVariations (VariationID, ProductID, SKU, VariationName, Stock, IsActive, IsDefault)
VALUES
('PV000001', 'PH000001', 'PH000001-SET-A', N'標準套組', 100, 1, 1),
('PV000002', 'DI000001', 'DI000001-STD', N'標準版', 999, 1, 1);

-- 9. 商品圖片表
CREATE TABLE ProductImages (
    ImageID nchar(8) NOT NULL,
    VariationID nchar(8) NOT NULL,
    ImageUrl nvarchar(500) NOT NULL,
    SortOrder int NOT NULL DEFAULT 0,
    ImageCaption nvarchar(100) NULL,
    FileSize bigint NOT NULL,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ProductImages PRIMARY KEY (ImageID),
    CONSTRAINT FK_ProductImages_Variation FOREIGN KEY (VariationID) REFERENCES ProductVariations(VariationID)
);

INSERT INTO ProductImages (ImageID, VariationID, ImageUrl, SortOrder, FileSize)
VALUES
('10000001', 'PV000001', 'https://example.com/images/products/postcard_main.jpg', 1, 512000),
('10000002', 'PV000001', 'https://example.com/images/products/postcard_detail.jpg', 2, 450000);

-- 10. 實體商品表
CREATE TABLE PhysicalProduct (
    ProductID nchar(8) NOT NULL,
    Weight decimal(8,3) NULL,
    Length decimal(8,2) NULL,
    Width decimal(8,2) NULL,
    Height decimal(8,2) NULL,
    DeliveryDays int NOT NULL DEFAULT 3,
    ShippingFeeType tinyint NOT NULL DEFAULT 1, -- 1:免運費 2:固定運費 3:依重量 4:依體積
    FixedShippingFee money NULL,
    IsFragile bit NOT NULL DEFAULT 0,
    PackagingNote nvarchar(200) NULL,
    CONSTRAINT PK_PhysicalProduct PRIMARY KEY (ProductID),
    CONSTRAINT FK_PhysicalProduct_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

INSERT INTO PhysicalProduct (ProductID, Weight, Length, Width, Height, ShippingFeeType, FixedShippingFee)
VALUES
('PH000001', 0.250, 15.00, 10.00, 0.50, 2, 60.00);

-- 11. 數位商品表
CREATE TABLE DigitalProduct (
    ProductID nchar(8) NOT NULL,
    FileFormat nvarchar(50) NOT NULL,
    FileSize bigint NULL,
    FilePath nvarchar(500) NOT NULL,
    PreviewImagePath nvarchar(500) NULL,
    DownloadLimit int NOT NULL DEFAULT 3,
    ValidityDays int NULL,
    LicenseType tinyint NULL, -- 1:個人使用 2:商業使用 3:教育使用
    LicenseDescription ntext NULL,
    CONSTRAINT PK_DigitalProduct PRIMARY KEY (ProductID),
    CONSTRAINT FK_DigitalProduct_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

INSERT INTO DigitalProduct (ProductID, FileFormat, FileSize, FilePath, DownloadLimit)
VALUES
('DI000001', 'JPG', 10240000, 'https://example.com/files/digital/city_night.jpg', 5);

-- 12. 會員收件地址表
CREATE TABLE ShippingAddresses (
    AddressID nchar(8) NOT NULL,
    MemberID nchar(36) NOT NULL,
    AddressType tinyint NOT NULL, -- 1:宅配 2:超商取貨
    RecipientName nvarchar(50) NOT NULL,
    RecipientPhone nvarchar(20) NOT NULL,
    ConvenienceStoreType nvarchar(20) NULL,
    StoreCode nvarchar(20) NULL,
    StoreName nvarchar(50) NULL,
    PostalCode nvarchar(10) NULL,
    City nvarchar(20) NULL,
    District nvarchar(20) NULL,
    Address nvarchar(200) NULL,
    IsDefault bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ShippingAddresses PRIMARY KEY (AddressID),
    CONSTRAINT FK_ShippingAddresses_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);

INSERT INTO ShippingAddresses (AddressID, MemberID, AddressType, RecipientName, RecipientPhone, City, District, Address, IsDefault)
VALUES
('AD000001', 'b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 1, N'陳支持', '0912345678', N'台北市', N'信義區', N'市府路1號', 1);

-- 13. 商品收藏表
CREATE TABLE Favorite (
    FavoriteID nchar(8) NOT NULL,
    MemberID nchar(36) NOT NULL,
    ProductID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    FavoriteType tinyint NOT NULL, -- 1:商品 2:系列
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Favorite PRIMARY KEY (FavoriteID),
    CONSTRAINT FK_Favorite_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT FK_Favorite_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_Favorite_Series FOREIGN KEY (SeriesID) REFERENCES ProductSeries(SeriesID),
    CONSTRAINT CHK_Favorite_Type CHECK (
        (FavoriteType = 1 AND ProductID IS NOT NULL AND SeriesID IS NULL) OR
        (FavoriteType = 2 AND ProductID IS NULL AND SeriesID IS NOT NULL)
    )
);

INSERT INTO Favorite (FavoriteID, MemberID, ProductID, SeriesID, FavoriteType)
VALUES
('FA000001', 'b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 'PH000001', NULL, 1),
('FA000002', 'b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', NULL, 'SE000001', 2);

-- 14. 訂單主檔表
CREATE TABLE [Order] (
    OrderID nchar(13) NOT NULL,
    MemberID nchar(36) NOT NULL,
    AddressID nchar(8) NOT NULL,
    OrderNumber nchar(12) NOT NULL,
    TotalAmount money NOT NULL,
    PaymentStatus tinyint NOT NULL DEFAULT 1, -- 1:待付款 2:付款中 3:已付款 4:付款失敗
    OrderStatus tinyint NOT NULL DEFAULT 1, -- 1:處理中 2:已出貨 3:已完成 4:已取消
    ECPayMerchantID nvarchar(20) NULL,
    RecipientName nvarchar(50) NULL,
    RecipientPhone nvarchar(20) NULL,
    ShippingAddress nvarchar(300) NULL,
    CreateDate datetime NULL DEFAULT GETDATE(),
    PaidDate datetime NULL,
    ShippedDate datetime NULL,
    CONSTRAINT PK_Order PRIMARY KEY (OrderID),
    CONSTRAINT FK_Order_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT FK_Order_Address FOREIGN KEY (AddressID) REFERENCES ShippingAddresses(AddressID),
    CONSTRAINT UQ_Order_OrderNumber UNIQUE (OrderNumber)
);

INSERT INTO [Order] (OrderID, MemberID, AddressID, OrderNumber, TotalAmount, PaymentStatus, OrderStatus)
VALUES
('2025090300001', 'b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 'AD000001', 'B20250901001', 210.00, 3, 2);

-- 15. 訂單明細表
CREATE TABLE OrderDetail (
    OrderID nchar(13) NOT NULL,
    ProductID nchar(8) NOT NULL,
    VariationID nchar(8) NOT NULL,
    ProductName nvarchar(100) NOT NULL,
    Price money NOT NULL,
    Quantity int NOT NULL,
    SubTotal money NOT NULL,
    ProductType tinyint NOT NULL, -- 1:數位 2:實體
    ShippingFee money NULL,
    PlatformFee money NULL,
    CreatorAmount money NULL,
    PaymentMethod tinyint NOT NULL, -- 1:信用卡 2:LINEPAY 3:超商取貨付款
    CONSTRAINT PK_OrderDetail PRIMARY KEY (OrderID, ProductID, VariationID),
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_OrderDetail_Variation FOREIGN KEY (VariationID) REFERENCES ProductVariations(VariationID),
    CONSTRAINT CHK_OrderDetail_Price CHECK (Price >= 0),
    CONSTRAINT CHK_OrderDetail_Quantity CHECK (Quantity >= 1)
);

INSERT INTO OrderDetail (OrderID, ProductID, VariationID, ProductName, Price, Quantity, SubTotal, ProductType, ShippingFee, PaymentMethod)
VALUES
('2025090300001', 'PH000001', 'PV000001', N'動物攝影明信片套組', 150.00, 1, 150.00, 2, 60.00, 1);

-- 16. 綠界交易記錄表
CREATE TABLE ECPayTransactions (
    TransactionID nchar(15) NOT NULL,
    OrderID nchar(13) NOT NULL,
    AddressID nchar(8) NOT NULL,
    ECPayMerchantID nvarchar(20) NULL,
    ECPayTradeNo nvarchar(20) NULL,
    ECPayPaymentType nvarchar(20) NULL,
    TradeAmount money NOT NULL,
    PaymentStatus tinyint NOT NULL DEFAULT 1, -- 1:建立 2:付款中 3:付款完成 4:失敗
    TradeDate datetime NOT NULL DEFAULT GETDATE(),
    PaymentDate datetime NULL,
    CVSPaymentNo nvarchar(20) NULL,
    CVSValidationNo nvarchar(10) NULL,
    ExpireDate datetime NULL,
    ECPayReturnData ntext NULL,
    CheckMacValue nvarchar(100) NULL,
    CONSTRAINT PK_ECPayTransactions PRIMARY KEY (TransactionID),
    CONSTRAINT FK_ECPayTransactions_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_ECPayTransactions_Address FOREIGN KEY (AddressID) REFERENCES ShippingAddresses(AddressID)
);

-- 17. 數位商品下載紀錄表
CREATE TABLE DigitalDownload (
    DownloadID nchar(8) NOT NULL,
    OrderID nchar(13) NOT NULL,
    MemberID nchar(36) NOT NULL,
    DownloadCount int NOT NULL DEFAULT 0,
    DownloadLimit int NOT NULL DEFAULT 3,
    ExpiryDate datetime NULL,
    LastDLDate datetime NULL,
    CONSTRAINT PK_DigitalDownload PRIMARY KEY (DownloadID),
    CONSTRAINT FK_DigitalDownload_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_DigitalDownload_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);

-- 注意：由於訂單是實體商品，不插入數位下載記錄
-- 如果需要數位下載記錄的範例，需要先有數位商品的訂單

-- 建立索引以提升查詢效能
-- 會員表索引
CREATE INDEX IX_Member_Email ON Member(Email);
CREATE INDEX IX_Member_Status ON Member(Status);
CREATE INDEX IX_Member_CreateDate ON Member(CreateDate);

-- 商品表索引
CREATE INDEX IX_Product_MemberID ON Product(MemberID);
CREATE INDEX IX_Product_CategoryID ON Product(CategoryID);
CREATE INDEX IX_Product_SeriesID ON Product(SeriesID);
CREATE INDEX IX_Product_Status ON Product(Status);
CREATE INDEX IX_Product_ProductType ON Product(ProductType);
CREATE INDEX IX_Product_CreateDate ON Product(CreateDate);

-- 訂單表索引
CREATE INDEX IX_Order_MemberID ON [Order](MemberID);
CREATE INDEX IX_Order_PaymentStatus ON [Order](PaymentStatus);
CREATE INDEX IX_Order_OrderStatus ON [Order](OrderStatus);
CREATE INDEX IX_Order_CreateDate ON [Order](CreateDate);
CREATE INDEX IX_Order_OrderNumber ON [Order](OrderNumber);

-- 收藏表索引
CREATE INDEX IX_Favorite_MemberID ON Favorite(MemberID);
CREATE INDEX IX_Favorite_ProductID ON Favorite(ProductID);
CREATE INDEX IX_Favorite_SeriesID ON Favorite(SeriesID);
CREATE INDEX IX_Favorite_Type ON Favorite(FavoriteType);

-- 綠界交易記錄索引
CREATE INDEX IX_ECPayTransactions_OrderID ON ECPayTransactions(OrderID);
CREATE INDEX IX_ECPayTransactions_ECPayTradeNo ON ECPayTransactions(ECPayTradeNo);
CREATE INDEX IX_ECPayTransactions_PaymentStatus ON ECPayTransactions(PaymentStatus);

-- 數位下載記錄索引
CREATE INDEX IX_DigitalDownload_MemberID ON DigitalDownload(MemberID);
CREATE INDEX IX_DigitalDownload_OrderID ON DigitalDownload(OrderID);

-- 商品款式索引
CREATE INDEX IX_ProductVariations_ProductID ON ProductVariations(ProductID);
CREATE INDEX IX_ProductVariations_SKU ON ProductVariations(SKU);

-- 商品圖片索引
CREATE INDEX IX_ProductImages_VariationID ON ProductImages(VariationID);
CREATE INDEX IX_ProductImages_SortOrder ON ProductImages(SortOrder);

GO