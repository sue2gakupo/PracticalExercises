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

-- 會員登入資料表
CREATE TABLE Member (
    MemberID nchar(36) NOT NULL,
    Email nvarchar(255) NOT NULL,
    PasswordHash nvarchar(512) NOT NULL,
    GoogleUserID nvarchar(100) NULL,
    [Name] nvarchar(50) NULL,
    IsEmailVerified bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    LastLoginDate datetime NULL DEFAULT GETDATE(),
    [Status] tinyint NOT NULL DEFAULT 1, -- 1:正常 2:停用 3:鎖定
    CONSTRAINT PK_Member PRIMARY KEY (MemberID),
    CONSTRAINT UQ_Member_Email UNIQUE (Email)
);

-- 插入會員資料 (需包含所有必要的會員ID)
INSERT INTO Member (MemberID, Email, PasswordHash, [Name], IsEmailVerified, [Status])
VALUES
    ('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', 'creator_a@example.com', 'hash_for_creator_a', N'王創作者', 1, 1),
    ('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 'supporter_b@example.com', 'hash_for_supporter_b', N'陳支持者', 1, 1),
    ('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', 'creator_c@example.com', 'hash_for_creator_c', N'林創作者', 1, 1),
    ('a1b2c3d4-e5f6-7890-1234-567890abcdef', 'disabled_user@example.com', 'hash_for_disabled_user', N'停用會員', 1, 2);

-- 創作者表
CREATE TABLE Creator (
    CreatorID nchar(8) NOT NULL,  
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
    [Status] tinyint NOT NULL DEFAULT 1,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
    MemberID nchar(36) NOT NULL,
    CONSTRAINT PK_Creator PRIMARY KEY (CreatorID),
    CONSTRAINT FK_Creator_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT UQ_Creator_Member UNIQUE (MemberID) -- 一個會員只能有一個創作者身分
);

INSERT INTO Creator (CreatorID, CreatorName, Biography,  ECPayStatus, ECPayMerchantID, [Status], MemberID)
VALUES
('CR000001', N'創作者A的空間', N'專注於插畫與設計。', NULL, NULL, 1,'50d1b32d-20d0-48e0-a4f6-7b61c944f2e8'),
('CR000002', N'林的作品集', N'分享生活攝影與短片教學。', NULL, NULL, 1,'e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2');

-- 支持者表
CREATE TABLE Supporter (
    SupporterID nchar(8) NOT NULL, 
    SupporterName nvarchar(50) NULL,
    AvatarUrl nvarchar(500) NULL,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
	MemberID nchar(36) NOT NULL,
    CONSTRAINT PK_Supporter PRIMARY KEY (SupporterID),
    CONSTRAINT FK_Supporter_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
	CONSTRAINT UQ_Supporter_Member UNIQUE (MemberID) -- 一個會員只能有一個支持者身分
);

INSERT INTO Supporter (SupporterID, SupporterName, AvatarUrl,MemberID)
VALUES
('SU000001', N'陳大支持', 'https://example.com/images/avatars/supporter_b.jpg','b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d'),
('SU000002', N'王的支持帳號', 'https://example.com/images/avatars/supporter_a.jpg','50d1b32d-20d0-48e0-a4f6-7b61c944f2e8'); -- 創作者也可以是支持者


-- 收件地址表
CREATE TABLE ShippingAddress (
    AddressID nchar(8) NOT NULL,
    AddressType tinyint NOT NULL, -- 1:宅配 2:超商取貨
    RecipientName nvarchar(50) NOT NULL,
    RecipientPhone nvarchar(20) NOT NULL,
    ConvenienceStoreType nvarchar(20) NULL,
    StoreCode nvarchar(20) NULL,
    StoreName nvarchar(50) NULL,
    PostalCode nvarchar(10) NULL,
    City nvarchar(20) NULL,
    District nvarchar(20) NULL,
    [Address] nvarchar(200) NULL,
    IsDefault bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
	SupporterID nchar(8) NOT NULL,
	CONSTRAINT PK_ShippingAddress PRIMARY KEY (AddressID),
    CONSTRAINT FK_ShippingAddress_Supporter FOREIGN KEY (SupporterID) REFERENCES Supporter(SupporterID)
);

INSERT INTO ShippingAddress (AddressID, AddressType, RecipientName, RecipientPhone, City, District, [Address], IsDefault, SupporterID)
VALUES
('AD000001', 1, N'陳支持', '0912345678', N'台北市', N'信義區', N'市府路1號', 1, 'SU000001'),
('AD000002', 2, N'王創作者', '0987654321', NULL, NULL, NULL, 0, 'SU000002'); -- 超商取貨地址

-- 更新超商取貨地址資訊
UPDATE ShippingAddress 
SET ConvenienceStoreType = 'FamilyMart', 
    StoreCode = 'FM001234', 
    StoreName = N'全家台北信義店',
    PostalCode = '110'
WHERE AddressID = 'AD000002';



-- 商品分類表 (全站統一，管理員維護)
CREATE TABLE Category (
    CategoryID nchar(8) NOT NULL,
    CategoryName nvarchar(50) NOT NULL,
    [Description] nvarchar(200) NULL,
    ParentCategoryID nchar(8) NULL,
    IconUrl nvarchar(500) NULL,
    SortOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1,
    CONSTRAINT PK_Category PRIMARY KEY (CategoryID),
    CONSTRAINT FK_Category_Parent FOREIGN KEY (ParentCategoryID) REFERENCES Category(CategoryID)
);

-- 建立根分類
INSERT INTO Category (CategoryID, CategoryName, [Description], ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000001', '藝術作品', '各類藝術創作', NULL, 1, 1),
('CA000002', '插畫設計', '插畫與設計作品', NULL, 2, 1),
('CA000003', '攝影作品', '攝影相關商品', NULL, 3, 1),
('CA000004', '手作商品', '手工製作商品', NULL, 4, 1),
('CA000005', '數位素材', '數位設計素材', NULL, 5, 1);

-- 建立子分類
INSERT INTO Category (CategoryID, CategoryName, [Description], ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000006', '貼紙', '各式貼紙商品', 'CA000002', 1, 1),
('CA000007', '明信片', '明信片商品', 'CA000002', 2, 1),
('CA000008', '海報', '海報商品', 'CA000002', 3, 1);


-- 系列分類表 (創作者自定義)
CREATE TABLE ProductSeries (
    SeriesID nchar(8) NOT NULL,
    SeriesName nvarchar(80) NOT NULL,
    [Description] ntext NULL,
    CoverImageUrl nvarchar(500) NULL,
    Tags nvarchar(200) NULL,
    SortOrder int NOT NULL DEFAULT 0,
    IsPublic bit NOT NULL DEFAULT 1,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL,
	CreatorID nchar(8) NOT NULL, 
    CONSTRAINT PK_ProductSeries PRIMARY KEY (SeriesID),
    CONSTRAINT FK_ProductSeries_Creator FOREIGN KEY (CreatorID) REFERENCES Creator(CreatorID)
);

INSERT INTO ProductSeries (SeriesID, SeriesName, [Description], IsPublic, CreatorID)
VALUES
('SE000001',  N'城市風景系列', N'一系列以城市為主題的插畫作品。', 1,'CR000001'),
('SE000002',  N'動物攝影集', N'來自世界各地野生動物的攝影作品。', 1,'CR000001');

-- 商品主檔表
CREATE TABLE Product (
    ProductID nchar(8) NOT NULL,
    ProductName nvarchar(100) NOT NULL,
    [Description] ntext NULL,
    ProductType tinyint NOT NULL, -- 1:數位商品 2:實體商品
    CategoryID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    Price money NOT NULL,
    [Status] tinyint NOT NULL DEFAULT 1, -- 1:上架中 2:已下架 3:暫停販售
    LaunchDate datetime NULL,
    OfflineDate datetime NULL,
    SortOrder int NOT NULL DEFAULT 0,
    PurchaseCount int NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
	CreatorID nchar(8) NOT NULL, 
    CONSTRAINT PK_Product PRIMARY KEY (ProductID),
    CONSTRAINT FK_Product_Creator FOREIGN KEY (CreatorID) REFERENCES Creator(CreatorID),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
    CONSTRAINT FK_Product_Series FOREIGN KEY (SeriesID) REFERENCES ProductSeries(SeriesID),
    CONSTRAINT CHK_Product_Price CHECK (Price >= 0)
);

INSERT INTO Product (ProductID, ProductName, [Description], ProductType, CategoryID, SeriesID, Price, [Status], CreatorID)
VALUES
('DI000001' , N'城市夜景插畫電子圖檔', N'高解析度數位插畫，可用於印刷或螢幕背景。', 1, 'CA000002', 'SE000001', 50.00, 1,'CR000001'),
('PH000001',  N'動物攝影明信片套組', N'包含10張精選動物攝影的實體明信片。', 2, 'CA000007', 'SE000002', 150.00, 1,'CR000002');

-- 商品款式表
CREATE TABLE ProductVariations (
    VariationID nchar(8) NOT NULL,
    SKU nvarchar(50) NOT NULL,
    VariationName nvarchar(100) NOT NULL,
    Color nvarchar(30) NULL,
    Size nvarchar(20) NULL,
    Material nvarchar(30) NULL,
    Edition nvarchar(20) NULL,
    PriceDifference money NULL,
    Stock int NOT NULL DEFAULT 0,
	SafetyStock int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1,
    SortOrder int NOT NULL DEFAULT 0,
    IsDefault bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
	ProductID nchar(8) NOT NULL,
    CONSTRAINT PK_ProductVariations PRIMARY KEY (VariationID),
    CONSTRAINT FK_ProductVariations_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
	CONSTRAINT UQ_ProductVariations_SKU UNIQUE (SKU)
);

-- 為兩個商品都建立款式
INSERT INTO ProductVariations (VariationID, SKU, VariationName, Stock, IsActive, IsDefault, ProductID)
VALUES
('PV000001', 'PH000001-SET-A', N'標準套組', 100, 1, 1, 'PH000001'),
('PV000002', 'DI000001-STD', N'標準版', 999, 1, 1, 'DI000001');

-- 商品圖片表
CREATE TABLE ProductImages (
    ImageID nchar(8) NOT NULL,
    ImageUrl nvarchar(500) NOT NULL,
    SortOrder int NOT NULL DEFAULT 0,
    ImageCaption nvarchar(100) NULL,
    FileSize bigint NOT NULL,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    UpdateDate datetime NULL DEFAULT GETDATE(),
	VariationID nchar(8) NOT NULL,
    CONSTRAINT PK_ProductImages PRIMARY KEY (ImageID),
    CONSTRAINT FK_ProductImages_Variation FOREIGN KEY (VariationID) REFERENCES ProductVariations(VariationID)
);

INSERT INTO ProductImages (ImageID, ImageUrl, SortOrder, FileSize, VariationID)
VALUES
('10000001', 'https://example.com/images/products/postcard_main.jpg', 1, 512000, 'PV000001'),
('10000002', 'https://example.com/images/products/postcard_detail.jpg', 2, 450000, 'PV000001');

-- 實體商品表
CREATE TABLE PhysicalProduct (
    ProductID nchar(8) NOT NULL,
    [Weight] decimal(8,3) NULL,
    [Length] decimal(8,2) NULL,
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

INSERT INTO PhysicalProduct (ProductID, [Weight], [Length], Width, Height, ShippingFeeType, FixedShippingFee)
VALUES
('PH000001', 0.250, 15.00, 10.00, 0.50, 2, 60.00);

-- 數位商品表
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


-- 商品收藏表
CREATE TABLE Favorite (
    FavoriteID nchar(8) NOT NULL,
    ProductID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    FavoriteType tinyint NOT NULL, -- 1:商品 2:系列
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
	SupporterID nchar(8) NOT NULL,
    CONSTRAINT PK_Favorite PRIMARY KEY (FavoriteID),
    CONSTRAINT FK_Favorite_Supporter FOREIGN KEY (SupporterID) REFERENCES Supporter(SupporterID),
    CONSTRAINT FK_Favorite_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_Favorite_Series FOREIGN KEY (SeriesID) REFERENCES ProductSeries(SeriesID),

);
INSERT INTO Favorite (FavoriteID, ProductID, SeriesID, FavoriteType,SupporterID)
VALUES
('FA000001',  'PH000001', NULL, 1,'SU000001'), -- 收藏商品
('FA000002',  NULL, 'SE000001', 2,'SU000001'); -- 收藏系列


-- 訂單表
CREATE TABLE [Order] (
    OrderID nchar(13) NOT NULL,
    TotalAmount money NOT NULL,
    PaymentStatus tinyint NOT NULL DEFAULT 1,
    OrderStatus tinyint NOT NULL DEFAULT 1,
    ECPayMerchantID nvarchar(20) NULL,
    RecipientName nvarchar(50) NULL,
    RecipientPhone nvarchar(20) NULL,
    ShippingAddress nvarchar(300) NULL,
    CreateDate datetime NULL DEFAULT GETDATE(),
    PaidDate datetime NULL,
    ShippedDate datetime NULL,
    SupporterID nchar(8) NOT NULL,  -- 指向 Supporter 表
    CreatorID nchar(8) NOT NULL,    -- 指向 Creator 表
    AddressID nchar(8) NOT NULL,
    CONSTRAINT PK_Order PRIMARY KEY (OrderID),
    CONSTRAINT FK_Order_Supporter FOREIGN KEY (SupporterID) REFERENCES Supporter(SupporterID),
    CONSTRAINT FK_Order_Creator FOREIGN KEY (CreatorID) REFERENCES Creator(CreatorID),
    CONSTRAINT FK_Order_Address FOREIGN KEY (AddressID) REFERENCES ShippingAddress(AddressID)

);

INSERT INTO [Order] (OrderID, TotalAmount, PaymentStatus, OrderStatus, SupporterID, CreatorID,AddressID)
VALUES
('O202509030001', 210.00, 3, 2,'SU000001','CR000001' , 'AD000001'),
('O202509030002',  50.00, 1, 1,'SU000002','CR000001', 'AD000002');

-- 訂單明細表
CREATE TABLE OrderDetail (
    OrderID nchar(13) NOT NULL,    
    ProductName nvarchar(100) NOT NULL,
	VariationName nvarchar(100) NOT NULL,
    Price money NOT NULL,
    Quantity int NOT NULL,
    SubTotal money NOT NULL,
    ProductType tinyint NOT NULL, -- 1:數位 2:實體
    ShippingFee money NULL,
    PlatformFee money NULL,
    CreatorAmount money NULL,
    PaymentMethod tinyint NOT NULL, -- 1:信用卡 2:LINEPAY 3:超商取貨付款
	ProductID nchar(8) NOT NULL,
    VariationID nchar(8) NOT NULL,
    CONSTRAINT PK_OrderDetail PRIMARY KEY (OrderID, ProductID, VariationID),
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_OrderDetail_Variation FOREIGN KEY (VariationID) REFERENCES ProductVariations(VariationID),  
);

INSERT INTO OrderDetail (OrderID, ProductName, VariationName, Price, Quantity, SubTotal, ProductType, ShippingFee, PaymentMethod, ProductID, VariationID)
VALUES
('O202509030001', N'動物攝影明信片套組', N'標準套組', 150.00, 1, 150.00, 2, 60.00, 1, 'PH000001', 'PV000001'),
('O202509030002', N'城市夜景插畫電子圖檔', N'標準版', 50.00, 1, 50.00, 1, NULL, 1, 'DI000001', 'PV000002');

-- 綠界交易記錄表
CREATE TABLE ECPayTransactions (
    TransactionID nchar(15) NOT NULL,
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
	OrderID nchar(13) NOT NULL,
    AddressID nchar(8) NULL,
    CONSTRAINT PK_ECPayTransactions PRIMARY KEY (TransactionID),
    CONSTRAINT FK_ECPayTransactions_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
);
INSERT INTO ECPayTransactions (TransactionID, ECPayMerchantID, ECPayTradeNo, ECPayPaymentType, TradeAmount, PaymentStatus, OrderID, AddressID)
VALUES
('T20250903000001', 'MID001', 'EC001234567890', 'Credit', 210.00, 3, 'O202509030001', 'AD000001'),
('T20250903000002', 'MID002', 'EC001234567891', 'Credit', 50.00, 1, 'O202509030002', 'AD000002');


-- 數位商品下載紀錄表
CREATE TABLE DigitalDownload (
    DownloadID nchar(8) NOT NULL,
    DownloadCount int NOT NULL DEFAULT 0,
    DownloadLimit int NOT NULL DEFAULT 3,
    ExpiryDate datetime NULL,
    LastDLDate datetime NULL,
    OrderID nchar(13) NOT NULL,
    CONSTRAINT PK_DigitalDownload PRIMARY KEY (DownloadID),
    CONSTRAINT FK_DigitalDownload_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
   
);
INSERT INTO DigitalDownload (DownloadID, DownloadCount, DownloadLimit, ExpiryDate, OrderID)
VALUES
('DD000001', 1, 5, DATEADD(day, 30, GETDATE()), 'O202509030002');


--建立預存程序
alter proc getCreatorWithMember
@memID nchar(36)
as
begin
	select Creator.* ,Creator.MemberID from Member
	inner join Creator on Member.MemberID=Creator.MemberID	
	where Member.MemberID=@memID
end
--測試
exec getCreatorWithMember '50d1b32d-20d0-48e0-a4f6-7b61c944f2e8'