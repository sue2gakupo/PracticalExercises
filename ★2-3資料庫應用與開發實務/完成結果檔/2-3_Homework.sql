--在【員工】資料表中找出所有在1993年(含)以後到職的資料。
select * from Employees 
where HireDate >= '1993-01-01'

--在【訂單】資料表找出送貨郵遞區號為44087與05022及82520的資料。
select * from Orders
where ShipPostalCode in ('44087', '05022', '82520')