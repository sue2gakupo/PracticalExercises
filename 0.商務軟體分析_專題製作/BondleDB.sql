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

-- 1. �|���n�J��ƪ�
CREATE TABLE Member (
    MemberID nchar(36) NOT NULL,
    Email nvarchar(255) NOT NULL,
    PasswordHash nvarchar(512) NOT NULL,
    GoogleUserID nvarchar(100) NULL,
    Name nvarchar(50) NULL,
    IsEmailVerified bit NOT NULL DEFAULT 0,
    CreateDate datetime NOT NULL DEFAULT GETDATE(),
    LastLoginDate datetime NULL DEFAULT GETDATE(),
    Status tinyint NOT NULL DEFAULT 1, -- 1:���` 2:���� 3:��w
    CONSTRAINT PK_Member PRIMARY KEY (MemberID),
    CONSTRAINT UQ_Member_Email UNIQUE (Email)
);

-- ���J�|����� (�ݥ]�t�Ҧ����n���|��ID)
INSERT INTO Member (MemberID, Email, PasswordHash, Name, IsEmailVerified, Status)
VALUES
    ('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', 'creator_a@example.com', 'hash_for_creator_a', N'���Ч@��', 1, 1),
    ('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 'supporter_b@example.com', 'hash_for_supporter_b', N'�������', 1, 1),
    ('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', 'creator_c@example.com', 'hash_for_creator_c', N'�L�Ч@��', 1, 1),
    ('a1b2c3d4-e5f6-7890-1234-567890abcdef', 'disabled_user@example.com', 'hash_for_disabled_user', N'���η|��', 1, 2);

-- 2. �|�������
CREATE TABLE MemberRoles (
    MemberID nchar(36) NOT NULL,
    IsCreator bit NOT NULL DEFAULT 0,
    IsSupporter bit NOT NULL DEFAULT 0,
    CONSTRAINT PK_MemberRoles PRIMARY KEY (MemberID),
    CONSTRAINT FK_MemberRoles_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);

INSERT INTO MemberRoles (MemberID, IsCreator, IsSupporter)
VALUES
    ('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', 1, 1), -- �Ч@�̤]�O�����
    ('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 0, 1), -- �Ȭ������
    ('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', 1, 0), -- �Ȭ��Ч@��
    ('a1b2c3d4-e5f6-7890-1234-567890abcdef', 0, 0); -- �S���S����

-- 3. �ӫ~������ (�����Τ@�A�޲z�����@)
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

-- �إ߮ڤ���
INSERT INTO Categories (CategoryID, CategoryName, Description, ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000001', '���N�@�~', '�U�����N�Ч@', NULL, 1, 1),
('CA000002', '���e�]�p', '���e�P�]�p�@�~', NULL, 2, 1),
('CA000003', '��v�@�~', '��v�����ӫ~', NULL, 3, 1),
('CA000004', '��@�ӫ~', '��u�s�@�ӫ~', NULL, 4, 1),
('CA000005', '�Ʀ����', '�Ʀ�]�p����', NULL, 5, 1);

-- �إߤl����
INSERT INTO Categories (CategoryID, CategoryName, Description, ParentCategoryID, SortOrder, IsActive) 
VALUES 
('CA000006', '�K��', '�U���K�Ȱӫ~', 'CA000002', 1, 1),
('CA000007', '���H��', '���H���ӫ~', 'CA000002', 2, 1),
('CA000008', '����', '�����ӫ~', 'CA000002', 3, 1);

-- 4. �Ч@�̪�
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
('50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', N'�Ч@��A���Ŷ�', N'�o�O�@��R���������Ч@�̡A�M�`�󴡵e�P�]�p�C', NULL, NULL, NULL, 1),
('e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', N'�L���@�~��', N'�D�n���ɥͬ���v�P�u���оǡC', NULL, NULL, NULL, 1);

-- 5. ����̪�
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
('b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', N'���j���', 'https://example.com/images/avatars/supporter_b.jpg');

-- 6. �t�C������ (�Ч@�̦۩w�q)
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
('SE000001', '50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', N'���������t�C', N'�@�t�C�H�������D�D�����e�@�~�C', 1),
('SE000002', 'e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', N'�ʪ���v��', N'�Ӧۥ@�ɦU�a���Ͱʪ�����v�@�~�C', 1);

-- 7. �ӫ~�D�ɪ�
CREATE TABLE Product (
    ProductID nchar(8) NOT NULL,
    MemberID nchar(36) NOT NULL,
    ProductName nvarchar(40) NOT NULL,
    Description ntext NULL,
    ProductType tinyint NOT NULL, -- 1:�Ʀ�ӫ~ 2:����ӫ~
    CategoryID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    Price money NOT NULL,
    Status tinyint NOT NULL DEFAULT 1, -- 1:�W�[�� 2:�w�U�[ 3:�Ȱ��c��
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
('DI000001', '50d1b32d-20d0-48e0-a4f6-7b61c944f2e8', N'�����]�����e�q�l����', N'���ѪR�׼Ʀ촡�e�A�i�Ω�L��οù��I���C', 1, 'CA000002', 'SE000001', 50.00, 1),
('PH000001', 'e92c23f1-f0e2-4d2d-9e6b-a2c9b4d1c1a2', N'�ʪ���v���H���M��', N'�]�t10�i���ʪ���v��������H���C', 2, 'CA000007', 'SE000002', 150.00, 1);

-- 8. �ӫ~�ڦ���
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

-- ����Ӱӫ~���إߴڦ�
INSERT INTO ProductVariations (VariationID, ProductID, SKU, VariationName, Stock, IsActive, IsDefault)
VALUES
('PV000001', 'PH000001', 'PH000001-SET-A', N'�зǮM��', 100, 1, 1),
('PV000002', 'DI000001', 'DI000001-STD', N'�зǪ�', 999, 1, 1);

-- 9. �ӫ~�Ϥ���
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

-- 10. ����ӫ~��
CREATE TABLE PhysicalProduct (
    ProductID nchar(8) NOT NULL,
    Weight decimal(8,3) NULL,
    Length decimal(8,2) NULL,
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

INSERT INTO PhysicalProduct (ProductID, Weight, Length, Width, Height, ShippingFeeType, FixedShippingFee)
VALUES
('PH000001', 0.250, 15.00, 10.00, 0.50, 2, 60.00);

-- 11. �Ʀ�ӫ~��
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

-- 12. �|������a�}��
CREATE TABLE ShippingAddresses (
    AddressID nchar(8) NOT NULL,
    MemberID nchar(36) NOT NULL,
    AddressType tinyint NOT NULL, -- 1:�v�t 2:�W�Ө��f
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
('AD000001', 'b4c9e8a7-a2f2-4c28-9d29-612b7a9f8f2d', 1, N'�����', '0912345678', N'�x�_��', N'�H�q��', N'������1��', 1);

-- 13. �ӫ~���ê�
CREATE TABLE Favorite (
    FavoriteID nchar(8) NOT NULL,
    MemberID nchar(36) NOT NULL,
    ProductID nchar(8) NULL,
    SeriesID nchar(8) NULL,
    FavoriteType tinyint NOT NULL, -- 1:�ӫ~ 2:�t�C
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

-- 14. �q��D�ɪ�
CREATE TABLE [Order] (
    OrderID nchar(13) NOT NULL,
    MemberID nchar(36) NOT NULL,
    AddressID nchar(8) NOT NULL,
    OrderNumber nchar(12) NOT NULL,
    TotalAmount money NOT NULL,
    PaymentStatus tinyint NOT NULL DEFAULT 1, -- 1:�ݥI�� 2:�I�ڤ� 3:�w�I�� 4:�I�ڥ���
    OrderStatus tinyint NOT NULL DEFAULT 1, -- 1:�B�z�� 2:�w�X�f 3:�w���� 4:�w����
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

-- 15. �q����Ӫ�
CREATE TABLE OrderDetail (
    OrderID nchar(13) NOT NULL,
    ProductID nchar(8) NOT NULL,
    VariationID nchar(8) NOT NULL,
    ProductName nvarchar(100) NOT NULL,
    Price money NOT NULL,
    Quantity int NOT NULL,
    SubTotal money NOT NULL,
    ProductType tinyint NOT NULL, -- 1:�Ʀ� 2:����
    ShippingFee money NULL,
    PlatformFee money NULL,
    CreatorAmount money NULL,
    PaymentMethod tinyint NOT NULL, -- 1:�H�Υd 2:LINEPAY 3:�W�Ө��f�I��
    CONSTRAINT PK_OrderDetail PRIMARY KEY (OrderID, ProductID, VariationID),
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_OrderDetail_Variation FOREIGN KEY (VariationID) REFERENCES ProductVariations(VariationID),
    CONSTRAINT CHK_OrderDetail_Price CHECK (Price >= 0),
    CONSTRAINT CHK_OrderDetail_Quantity CHECK (Quantity >= 1)
);

INSERT INTO OrderDetail (OrderID, ProductID, VariationID, ProductName, Price, Quantity, SubTotal, ProductType, ShippingFee, PaymentMethod)
VALUES
('2025090300001', 'PH000001', 'PV000001', N'�ʪ���v���H���M��', 150.00, 1, 150.00, 2, 60.00, 1);

-- 16. ��ɥ���O����
CREATE TABLE ECPayTransactions (
    TransactionID nchar(15) NOT NULL,
    OrderID nchar(13) NOT NULL,
    AddressID nchar(8) NOT NULL,
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
    CONSTRAINT PK_ECPayTransactions PRIMARY KEY (TransactionID),
    CONSTRAINT FK_ECPayTransactions_Order FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    CONSTRAINT FK_ECPayTransactions_Address FOREIGN KEY (AddressID) REFERENCES ShippingAddresses(AddressID)
);

-- 17. �Ʀ�ӫ~�U��������
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

-- �`�N�G�ѩ�q��O����ӫ~�A�����J�Ʀ�U���O��
-- �p�G�ݭn�Ʀ�U���O�����d�ҡA�ݭn�����Ʀ�ӫ~���q��

-- �إ߯��ޥH���ɬd�߮į�
-- �|�������
CREATE INDEX IX_Member_Email ON Member(Email);
CREATE INDEX IX_Member_Status ON Member(Status);
CREATE INDEX IX_Member_CreateDate ON Member(CreateDate);

-- �ӫ~�����
CREATE INDEX IX_Product_MemberID ON Product(MemberID);
CREATE INDEX IX_Product_CategoryID ON Product(CategoryID);
CREATE INDEX IX_Product_SeriesID ON Product(SeriesID);
CREATE INDEX IX_Product_Status ON Product(Status);
CREATE INDEX IX_Product_ProductType ON Product(ProductType);
CREATE INDEX IX_Product_CreateDate ON Product(CreateDate);

-- �q������
CREATE INDEX IX_Order_MemberID ON [Order](MemberID);
CREATE INDEX IX_Order_PaymentStatus ON [Order](PaymentStatus);
CREATE INDEX IX_Order_OrderStatus ON [Order](OrderStatus);
CREATE INDEX IX_Order_CreateDate ON [Order](CreateDate);
CREATE INDEX IX_Order_OrderNumber ON [Order](OrderNumber);

-- ���ê����
CREATE INDEX IX_Favorite_MemberID ON Favorite(MemberID);
CREATE INDEX IX_Favorite_ProductID ON Favorite(ProductID);
CREATE INDEX IX_Favorite_SeriesID ON Favorite(SeriesID);
CREATE INDEX IX_Favorite_Type ON Favorite(FavoriteType);

-- ��ɥ���O������
CREATE INDEX IX_ECPayTransactions_OrderID ON ECPayTransactions(OrderID);
CREATE INDEX IX_ECPayTransactions_ECPayTradeNo ON ECPayTransactions(ECPayTradeNo);
CREATE INDEX IX_ECPayTransactions_PaymentStatus ON ECPayTransactions(PaymentStatus);

-- �Ʀ�U���O������
CREATE INDEX IX_DigitalDownload_MemberID ON DigitalDownload(MemberID);
CREATE INDEX IX_DigitalDownload_OrderID ON DigitalDownload(OrderID);

-- �ӫ~�ڦ�����
CREATE INDEX IX_ProductVariations_ProductID ON ProductVariations(ProductID);
CREATE INDEX IX_ProductVariations_SKU ON ProductVariations(SKU);

-- �ӫ~�Ϥ�����
CREATE INDEX IX_ProductImages_VariationID ON ProductImages(VariationID);
CREATE INDEX IX_ProductImages_SortOrder ON ProductImages(SortOrder);

GO