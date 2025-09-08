-- Bondle�ӫ~�޲z�P�c�⥭�x��Ʈw�إ߻y�k
-- SQL Server Database Schema

-- �إ߸�Ʈw
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'BondleDB')
begin
	use master
	DROP DATABASE BondleDB
end

CREATE DATABASE BondleDB;
GO

USE BondleDB;
GO

-- �|���n�J��ƪ�
CREATE TABLE Member (
    MemberID nchar(36) NOT NULL,
    Email nvarchar(255) NOT NULL,
    PasswordHash nvarchar(512) NOT NULL,
    GoogleUserID nvarchar(100) NULL,
    [Name] nvarchar(50) NULL,
    IsEmailVerified bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    LastLoginDate datetime NULL DEFAULT GETDATE(),
    [Status] tinyint NOT NULL DEFAULT 1, -- 1:���` 2:���� 3:��w
    CONSTRAINT PK_Member PRIMARY KEY (MemberID),
    CONSTRAINT UQ_Member_Email UNIQUE (Email)
);

-- ���J�|����� (�ݥ]�t�Ҧ����n���|��ID)
INSERT INTO Member (MemberID, Email, PasswordHash, [Name], IsEmailVerified, [Status])
VALUES
    ('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', 'creator_a@example.com', 'hash_for_creator_a', N'���Ч@��', 1, 1),
    ('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 'supporter_b@example.com', 'hash_for_supporter_b', N'�������', 1, 1),
    ('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', 'creator_c@example.com', 'hash_for_creator_c', N'�L�Ч@��', 1, 1),
    ('a1b2c3d4-e5f6-7890-1234-567890abcdef', 'disabled_user@example.com', 'hash_for_disabled_user', N'���η|��', 1, 2);

-- �Ч@�̪�
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
    CONSTRAINT UQ_Creator_Member UNIQUE (MemberID) -- �@�ӷ|���u�঳�@�ӳЧ@�̨���
);

INSERT INTO Creator (CreatorID, CreatorName, Biography,  ECPayStatus, ECPayMerchantID, [Status], MemberID)
VALUES
('CR000001', N'�Ч@��A���Ŷ�', N'�M�`�󴡵e�P�]�p�C', NULL, NULL, 1,'50d1b32d-20d0-48e0-a4f6-7b61c944f2e8'),
('CR000002', N'�L���@�~��', N'���ɥͬ���v�P�u���оǡC', NULL, NULL, 1,'e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2');

-- ����̪�
CREATE TABLE Supporter (
    SupporterID nchar(8) NOT NULL, 
    SupporterName nvarchar(50) NULL,
    AvatarUrl nvarchar(500) NULL,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
	MemberID nchar(36) NOT NULL,
    CONSTRAINT PK_Supporter PRIMARY KEY (SupporterID),
    CONSTRAINT FK_Supporter_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
	CONSTRAINT UQ_Supporter_Member UNIQUE (MemberID) -- �@�ӷ|���u�঳�@�Ӥ���̨���
);

INSERT INTO Supporter (SupporterID, SupporterName, AvatarUrl,MemberID)
VALUES
('SU000001', N'���j���', 'https://example.com/images/avatars/supporter_b.jpg','b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d'),
('SU000002', N'��������b��', 'https://example.com/images/avatars/supporter_a.jpg','50d1b32d-20d0-48e0-a4f6-7b61c944f2e8'); -- �Ч@�̤]�i�H�O�����


-- ����a�}��
CREATE TABLE ShippingAddress (
    AddressID nchar(8) NOT NULL,
    AddressType tinyint NOT NULL, -- 1:�v�t 2:�W�Ө��f
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
('AD000001', 1, N'�����', '0912345678', N'�x�_��', N'�H�q��', N'������1��', 1, 'SU000001'),
('AD000002', 2, N'���Ч@��', '0987654321', NULL, NULL, NULL, 0, 'SU000002'); -- �W�Ө��f�a�}

-- ��s�W�Ө��f�a�}��T
UPDATE ShippingAddress 
SET ConvenienceStoreType = 'FamilyMart', 
    StoreCode = 'FM001234', 
    StoreName = N'���a�x�_�H�q��',
    PostalCode = '110'
WHERE AddressID = 'AD000002';



-- �ӫ~������ (�����Τ@�A�޲z�����@)
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

-- �إ߮ڤ���
INSERT INTO Category (CategoryID, CategoryName, [Description], ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000001', '���N�@�~', '�U�����N�Ч@', NULL, 1, 1),
('CA000002', '���e�]�p', '���e�P�]�p�@�~', NULL, 2, 1),
('CA000003', '��v�@�~', '��v�����ӫ~', NULL, 3, 1),
('CA000004', '��@�ӫ~', '��u�s�@�ӫ~', NULL, 4, 1),
('CA000005', '�Ʀ����', '�Ʀ�]�p����', NULL, 5, 1);

-- �إߤl����
INSERT INTO Category (CategoryID, CategoryName, [Description], ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000006', '�K��', '�U���K�Ȱӫ~', 'CA000002', 1, 1),
('CA000007', '���H��', '���H���ӫ~', 'CA000002', 2, 1),
('CA000008', '����', '�����ӫ~', 'CA000002', 3, 1);


-- �t�C������ (�Ч@�̦۩w�q)
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
('SE000001',  N'���������t�C', N'�@�t�C�H�������D�D�����e�@�~�C', 1,'CR000001'),
('SE000002',  N'�ʪ���v��', N'�Ӧۥ@�ɦU�a���Ͱʪ�����v�@�~�C', 1,'CR000001');

-- �ӫ~�D�ɪ�
CREATE TABLE Product (
    ProductID nchar(8) NOT NULL,
    ProductName nvarchar(100) NOT NULL,
    [Description] ntext NULL,
    ProductType tinyint NOT NULL, -- 1:�Ʀ�ӫ~ 2:����ӫ~
    CategoryID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    Price money NOT NULL,
    [Status] tinyint NOT NULL DEFAULT 1, -- 1:�W�[�� 2:�w�U�[ 3:�Ȱ��c��
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
('DI000001' , N'�����]�����e�q�l����', N'���ѪR�׼Ʀ촡�e�A�i�Ω�L��οù��I���C', 1, 'CA000002', 'SE000001', 50.00, 1,'CR000001'),
('PH000001',  N'�ʪ���v���H���M��', N'�]�t10�i���ʪ���v��������H���C', 2, 'CA000007', 'SE000002', 150.00, 1,'CR000002');

-- �ӫ~�ڦ���
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

-- ����Ӱӫ~���إߴڦ�
INSERT INTO ProductVariations (VariationID, SKU, VariationName, Stock, IsActive, IsDefault, ProductID)
VALUES
('PV000001', 'PH000001-SET-A', N'�зǮM��', 100, 1, 1, 'PH000001'),
('PV000002', 'DI000001-STD', N'�зǪ�', 999, 1, 1, 'DI000001');

-- �ӫ~�Ϥ���
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

-- ����ӫ~��
CREATE TABLE PhysicalProduct (
    ProductID nchar(8) NOT NULL,
    [Weight] decimal(8,3) NULL,
    [Length] decimal(8,2) NULL,
    Width decimal(8,2) NULL,
    Height decimal(8,2) NULL,
    DeliveryDays int NOT NULL DEFAULT 3,
    ShippingFeeType tinyint NOT NULL DEFAULT 1, -- 1:�K�B�O 2:�T�w�B�O 3:�̭��q 4:����n
    FixedShippingFee money NULL,
    IsFragile bit NOT NULL DEFAULT 0,
    PackagingNote nvarchar(200) NULL,
    CONSTRAINT PK_PhysicalProduct PRIMARY KEY (ProductID),
    CONSTRAINT FK_PhysicalProduct_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

INSERT INTO PhysicalProduct (ProductID, [Weight], [Length], Width, Height, ShippingFeeType, FixedShippingFee)
VALUES
('PH000001', 0.250, 15.00, 10.00, 0.50, 2, 60.00);

-- �Ʀ�ӫ~��
CREATE TABLE DigitalProduct (
    ProductID nchar(8) NOT NULL,
    FileFormat nvarchar(50) NOT NULL,
    FileSize bigint NULL,
    FilePath nvarchar(500) NOT NULL,
    PreviewImagePath nvarchar(500) NULL,
    DownloadLimit int NOT NULL DEFAULT 3,
    ValidityDays int NULL,
    LicenseType tinyint NULL, -- 1:�ӤH�ϥ� 2:�ӷ~�ϥ� 3:�Ш|�ϥ�
    LicenseDescription ntext NULL,
    CONSTRAINT PK_DigitalProduct PRIMARY KEY (ProductID),
    CONSTRAINT FK_DigitalProduct_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

INSERT INTO DigitalProduct (ProductID, FileFormat, FileSize, FilePath, DownloadLimit)
VALUES
('DI000001', 'JPG', 10240000, 'https://example.com/files/digital/city_night.jpg', 5);


-- �ӫ~���ê�
CREATE TABLE Favorite (
    FavoriteID nchar(8) NOT NULL,
    ProductID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    FavoriteType tinyint NOT NULL, -- 1:�ӫ~ 2:�t�C
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
	SupporterID nchar(8) NOT NULL,
    CONSTRAINT PK_Favorite PRIMARY KEY (FavoriteID),
    CONSTRAINT FK_Favorite_Supporter FOREIGN KEY (SupporterID) REFERENCES Supporter(SupporterID),
    CONSTRAINT FK_Favorite_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_Favorite_Series FOREIGN KEY (SeriesID) REFERENCES ProductSeries(SeriesID),

);
INSERT INTO Favorite (FavoriteID, ProductID, SeriesID, FavoriteType,SupporterID)
VALUES
('FA000001',  'PH000001', NULL, 1,'SU000001'), -- ���ðӫ~
('FA000002',  NULL, 'SE000001', 2,'SU000001'); -- ���èt�C


-- �q���
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
    SupporterID nchar(8) NOT NULL,  -- ���V Supporter ��
    CreatorID nchar(8) NOT NULL,    -- ���V Creator ��
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

-- �q����Ӫ�
CREATE TABLE OrderDetail (
    OrderID nchar(13) NOT NULL,    
    ProductName nvarchar(100) NOT NULL,
	VariationName nvarchar(100) NOT NULL,
    Price money NOT NULL,
    Quantity int NOT NULL,
    SubTotal money NOT NULL,
    ProductType tinyint NOT NULL, -- 1:�Ʀ� 2:����
    ShippingFee money NULL,
    PlatformFee money NULL,
    CreatorAmount money NULL,
    PaymentMethod tinyint NOT NULL, -- 1:�H�Υd 2:LINEPAY 3:�W�Ө��f�I��
	ProductID nchar(8) NOT NULL,
    VariationID nchar(8) NOT NULL,
    CONSTRAINT PK_OrderDetail PRIMARY KEY (OrderID, ProductID, VariationID),
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_OrderDetail_Variation FOREIGN KEY (VariationID) REFERENCES ProductVariations(VariationID),  
);

INSERT INTO OrderDetail (OrderID, ProductName, VariationName, Price, Quantity, SubTotal, ProductType, ShippingFee, PaymentMethod, ProductID, VariationID)
VALUES
('O202509030001', N'�ʪ���v���H���M��', N'�зǮM��', 150.00, 1, 150.00, 2, 60.00, 1, 'PH000001', 'PV000001'),
('O202509030002', N'�����]�����e�q�l����', N'�зǪ�', 50.00, 1, 50.00, 1, NULL, 1, 'DI000001', 'PV000002');

-- ��ɥ���O����
CREATE TABLE ECPayTransactions (
    TransactionID nchar(15) NOT NULL,
    ECPayMerchantID nvarchar(20) NULL,
    ECPayTradeNo nvarchar(20) NULL,
    ECPayPaymentType nvarchar(20) NULL,
    TradeAmount money NOT NULL,
    PaymentStatus tinyint NOT NULL DEFAULT 1, -- 1:�إ� 2:�I�ڤ� 3:�I�ڧ��� 4:����
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


-- �Ʀ�ӫ~�U��������
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


--�إ߹w�s�{��
alter proc getCreatorWithMember
@memID nchar(36)
as
begin
	select Creator.* ,Creator.MemberID from Member
	inner join Creator on Member.MemberID=Creator.MemberID	
	where Member.MemberID=@memID
end
--����
exec getCreatorWithMember '50d1b32d-20d0-48e0-a4f6-7b61c944f2e8'