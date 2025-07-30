--任務一
--基本查詢
--在【員工】資料表中找出所有在1993年(含)以後到職的資料。
select * from Employees 
where HireDate >= '1993-1-1'

--在【訂單】資料表找出送貨郵遞區號為44087與05022及82520的資料。
select * from Orders
where ShipPostalCode in ('44087', '05022', '82520')

--在【產品】資料表中找出庫存量最多的前6名資料記錄。
select top 6 with ties *
from Products
order by  UnitsInStock desc

--在【訂單】資料表中找出尚未有送貨日期的記錄資料。
select * from Orders
where ShippedDate is null

--在【訂單明細】資料表中找出訂購的產品數量介於20~40件的資料記錄。
select * from OrderDetails
where Quantity between 20 and 40

--統計查詢
--計算【產品】資料表中類別號為2的產品資料平均單價。
select avg (UnitPrice) as 平均單價
from Products
where CategoryID=2

--在【產品】資料表中找出庫存量小於安全存量，且尚未進行採購的產品資料記錄。
select * from Products
where UnitsInStock < ReorderLevel and UnitsOnOrder=0

--在【訂單明細】資料表找出訂單中包含超過5種產品的資料記錄。
select OrderID, count(*) as ProductID 
from OrderDetails
group by OrderID 
having count(*)>5

--在【訂單明細】資料表中顯示出訂單號碼10263所有產品的價格小計。
select * ,UnitPrice*Quantity*(1-Discount) as 小計
from OrderDetails
where OrderID=10263

--利用【產品】資料表資料，統計出每一個供應商各提供了幾樣產品。
select SupplierID, count(*) as 供應的商品
from Products
group by SupplierID

--利用【訂單明細】資料表資料，統計出各項商品的平均單價與平均銷售數量，
--並列出平均銷售數量大於10的資料，且將資料依產品編號由小到大排序。
select ProductID,avg(UnitPrice) as 平均單價,avg(Quantity) as 平均銷售數量
from OrderDetails
group by ProductID
having avg(Quantity) > 10
order by ProductID

-----------------------------------------------------------------------------
--任務二
--請試寫一合併查詢，查詢出訂購日期落在1996年7月並指定送貨公司為「United Package」的有訂單之訂貨明細資料，
--並列出訂單號碼、產品類別名稱、產品名稱、產品訂購單價、產品訂購數量、產品價錢小計、客戶編號、客戶名稱、收貨人名字、訂購日期、處理訂單員工的姓名、貨運公司、供應商名稱等資料項目，
--其中產品價錢小計請以四捨五入計算至整數位。

select o.OrderID 訂單號碼,c.CategoryID 產品類別,p.ProductName 產品,od.UnitPrice 訂購單價 ,od.Quantity 訂購數量 ,round(od.UnitPrice*(1-od.Discount)*od.Quantity,0) 小計,
cu.CustomerID 客戶編號,cu.CompanyName 客戶名稱,o.ShipName 收貨人,o.OrderDate 訂購日期,e.FirstName+' '+e.LastName as 處理訂單員工,s.CompanyName 貨運公司,su.CompanyName 供應商
from Shippers as s 
inner join (Categories as c 
inner join (Employees as e 
inner join(Suppliers as su 
inner join (Customers as cu 
inner join (Products as p 
inner join (OrderDetails as od 
inner join Orders as o 
on od.OrderID=o.OrderID) 
on p.ProductID=od.ProductID) 
on cu.CustomerID=o.CustomerID)
on su.SupplierID=p.SupplierID)
on e.EmployeeID=o.EmployeeID)
on c.CategoryID=p.CategoryID)
on s.ShipperID=o.ShipVia
where o.OrderDate between '1996-7-1' and '1996-7-31' and s.CompanyName='United Package'

--請利用exists運算子配合子查詢式，找出哪些客戶從未下過訂單，並列出客戶的所有欄位。
--(不可用到in運算，亦不可用合併查詢式) 

select * from Customers as cu 
where not exists 
(select * from Orders as o where cu.CustomerID=o.CustomerID)

--請利用in運算子配合子查詢式，查詢哪些員工有處理過訂單，並列出員工的員工編號、姓名、職稱、內部分機號碼、附註欄位。
--(不可用到exists運算，亦不可用合併查詢式) 

select EmployeeID,FirstName+' '+LastName as EmployeeName,Title,Extension,Notes
from Employees
where EmployeeID in(select distinct EmployeeID from OrderDetails)

--請合併查詢與子查詢兩種寫法，列出1998年度所有被訂購過的產品資料，並列出產品的所有欄位，且依產品編號由小到大排序。
--(寫合併查詢時不得用任何子查詢式，寫子查詢時不得用任何合併查詢式)

--合併查詢(inner join)
select distinct p.*
from Products as p 
inner join OrderDetails	as od on p.ProductID=od.ProductID
inner join Orders as o on od.OrderID=o.OrderID
where o.OrderDate between '1998-01-01'and '1998-12-31'
order by p.ProductID

--子查詢(in)
select * from Products	
where ProductID in (select ProductID from OrderDetails
where OrderID in (select OrderID from Orders
where OrderDate between '1998-01-01'and '1998-12-31'))

-----------------------------------------------------------------------------
--任務三
--建立一個名為【MySchool】資料庫的SQL DDL Script
create database MySchool
go

--寫出相對應之SQL DDL Script，使其可於【MySchool】資料庫中建立這些資料表。
create table[Student](
StuID nchar(10) not null primary key,
StuName nvarchar(20) not null,
Tel nvarchar(20) not null ,
[Address] nvarchar(100),
Birthday datetime not null,
DeptID nchar(1) not null, 
foreign key(DeptID) references [Department](DeptID))--2

create table[Course](
CourseID nchar(5) not null primary key,
CourseName nvarchar(30) not null,
Credit int not null default(0),
[Hour] int not null default(2),
DeptID nchar(1) not null, 
foreign key(DeptID) references [Department](DeptID))--3

create table[SelectionDetail](
StuID nchar(10) not null ,
CourseID nchar(5) not null ,
[Year] int not null default (Year(Getdate())),
Term tinyint  not null,
Score int not null default(0),
primary key (StuID,CourseID),
foreign key(StuID) references [Student](StuID),
foreign key(CourseID) references [Course](CourseID))--4

create table [Department](
DeptID nchar(1) not null primary key,
DeptName nvarchar(30) not null unique)--1

-----------------------------------------------------------------------------
--任務四
create proc InsertDeptmentData

	@DeptID nchar(1),@DeptName nvarchar(30)
as
begin

	declare @d nchar(1)
	declare @dn nvarchar(30)

	select @d=DeptID from [Department]  where DeptID=@DeptID
	select @dn=DeptName from [Department]  where DeptName=@DeptName


	if @d is not null
	begin
		print '科系代碼[' + @DeptID + '] 已存在，無法新增！' 
		return
	end

	if @dn is not null
	begin
		print '科系名稱[' + @DeptName + '] 已存在，無法新增！' 
		return
	end

	insert into [Department] (DeptID, DeptName)  
    values  (@DeptID, @DeptName)
    
    print '成功新增科系：代碼=' + @DeptID + ', 名稱=' + @DeptName

end


-----------------------------------------------------------------------------
--任務五

create function getCourseID(@DeptID nchar(1))
	returns nchar(5)
as
begin

	declare @lastID nchar(5), @newID nchar(5)

	select top 1 @lastID=CourseID
		from [Course]
		where DeptID = @DeptID
		order by CourseID desc

	if @lastID is null
		set @newID=N'C'+ @DeptID + N'001'
	else
	begin

		declare @num nchar(3)=right(N'000'+cast( cast( substring(@lastID, 3,3 ) as int )+1 as nvarchar),3)

		set @newID=N'C'+@DeptID+@num
	end
	return @newID 
end

--測試
--insert into [Course] values(dbo.getCourseID('A'),'test9647',2,3,'A')

--後續自行補充實務上最常用的做法 --max排序
create function getCourseID(@DeptID nchar(1))
returns nchar(5)
as
begin
declare @maxNum int, @newID nchar(5) 
select @maxNum  = max(cast(substring(CourseID,3,3)as int))
from [Course]
where DeptID=@DeptID
	and CourseID like N'C'+@DeptID+N'%'  ---- 如果 @DeptID = 'A'，會比對：CA001, CA002, CABC, CA999999 等所有以 'CA' 開頭的字串(模糊查詢)
	and len(CourseID) =5 --len計算字串長度（不包含尾端空格），確保 CourseID 正好是5個字元
	and isnumeric(substring(CourseID,3,3))=1 --isnumeric()檢查字串是否為有效數字，

	if @maxNum is null 
	set @newID=N'C'+@DeptID+N'001'
	else
	set @newID=N'C'+@DeptID+format(@maxNum+1,'D3')

	return @newID
end














