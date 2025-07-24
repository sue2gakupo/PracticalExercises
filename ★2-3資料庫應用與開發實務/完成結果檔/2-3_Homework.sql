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

--任務二
--請試寫一合併查詢，查詢出訂購日期落在1996年7月並指定送貨公司為「United Package」的有訂單之訂貨明細資料，
--並列出訂單號碼、產品類別名稱、產品名稱、產品訂購單價、產品訂購數量、產品價錢小計、客戶編號、客戶名稱、收貨人名字、訂購日期、處理訂單員工的姓名、貨運公司、供應商名稱等資料項目，
--其中產品價錢小計請以四捨五入計算至整數位。


select od.OrderID,c.CategoryName,p.ProductName,od.UnitPrice as 單價,
od.Quantity as 數量, od.
p.單價 as 訂價,p.單價-od.單價 as 價差,od.數量 as 售出數量,(p.單價-od.單價)*od.數量 as 總折扣金額,s.供應商,s.連絡人,s.連絡人職稱
,o.收貨人,o.訂單日期,cu.公司名稱,e.姓名,t.貨運公司名稱

from OrderDetails as od 
inner join Products as p on p.ProductID=od.ProductID
inner join Categories as c on c.CategoryID=p.CategoryID
inner join Suppliers as s on s.SupplierID=p.SupplierID
inner join OrderDetails as o on o.OrderID=od.OrderID
inner join Customers as cu on cu.CustomerID=o.CustomerID
inner join Employees as e on e.EmployeeID=o.EmployeeID
inner join Shippers as t on t.ShipperID=o.ShipVia
where o.OrderDate between '1996-7-1' and '1996-7-31'

select * 
from  Categories as c, Customers as cu,  Employees as e, Orders as o, 
OrderDetails as od, Products as p, Shippers as s,Suppliers as su
where od.OrderID=o.OrderID
and p.ProductID=od.ProductID
and cu.CustomerID=o.CustomerID
and su.SupplierID=p.SupplierID
and e.EmployeeID=o.EmployeeID
and c.CategoryID=p.CategoryID





