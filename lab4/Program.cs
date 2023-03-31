using System.Globalization;

namespace Main
{
    class Lab04
    {
        public static void Main(string[] args)
        {
            var EmployeeReader = new CSVReader<Employee>();
            var employees = EmployeeReader.ReadList("C:/Users/Piotr/pz/lab4/csv/employees.csv", employee => new Employee(){
                Id = employee[0],
                LastName = employee[1],
                FirstName = employee[2],
                Title = employee[3],
                TitleOfCourtesy = employee[4],
                BirthDate = employee[5],
                HireDate = employee[6],
                Address = employee[7],
                City = employee[8],
                Region = employee[9],
                PostalCode = employee[10],
                Country = employee[11],
                HomePhone = employee[12],
                Extension = employee[13],
                Photo = employee[14],
                Notes = employee[15],
                ReportsTo = employee[16],
                PhotoPath = employee[17]
            });

            employees.Take(5).ToList().ForEach(employee => Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}"));

            var TerritoryReader = new CSVReader<Territory>();
            var territories = TerritoryReader.ReadList("C:/Users/Piotr/pz/lab4/csv/territories.csv", territory => new Territory(){
                Id = territory[0],
                TerritoryDescription = territory[1],
                RegionId = territory[2] 
            });

            territories.Take(5).ToList().ForEach(territory => Console.WriteLine($"Territory: {territory.Id}, {territory.TerritoryDescription}, {territory.RegionId}"));

            var RegionReader = new CSVReader<Region>();
            var regions = RegionReader.ReadList("C:/Users/Piotr/pz/lab4/csv/regions.csv", region => new Region(){
                Id = region[0],
                RegionDescription = region[1]
            });

            regions.Take(5).ToList().ForEach(region => Console.WriteLine($"Region: {region.Id}, {region.RegionDescription}"));

            var EmployeeTerritoryReader = new CSVReader<EmployeeTerritory>();
            var employeeTerritories = EmployeeTerritoryReader.ReadList("C:/Users/Piotr/pz/lab4/csv/employee_territories.csv", employeeTeritorry =>
                new EmployeeTerritory(){
                    EmployeeId = employeeTeritorry[0],
                    TerritoryId = employeeTeritorry[1]
                });

            employeeTerritories.Take(5).ToList().ForEach(employeeTerritory => Console.WriteLine($"EmployeeTerritory: {employeeTerritory.EmployeeId}, {employeeTerritory.TerritoryId}"));
            
            var OrderReader = new CSVReader<Order>();
            var orders = OrderReader.ReadList("C:/Users/Piotr/pz/lab4/csv/orders.csv", order => new Order(){
                Id = order[0],
                CustomerId = order[1],
                EmployeeId = order[2],
                OrderDate = order[3],
                RequiredDate = order[4],
                ShippedDate = order[5],
                ShipVia = order[6],
                Freight = order[7],
                ShipName = order[8],
                ShipAddress = order[9],
                ShipCity = order[10],
                ShipRegion = order[11],
                ShipPostalCode = order[12],
                ShipCountry = order[13]
            });

            orders.Take(5).ToList().ForEach(order => Console.WriteLine($"Order: {order.Id}, {order.CustomerId}, {order.EmployeeId}, {order.OrderDate}, {order.RequiredDate}, {order.ShippedDate}, {order.ShipVia}, {order.Freight}, {order.ShipName}, {order.ShipAddress}, {order.ShipCity}, {order.ShipRegion}, {order.ShipPostalCode}, {order.ShipCountry}"));
            
            var OrderDetailReader = new CSVReader<OrderDetail>();
            var orderDetails = OrderDetailReader.ReadList("C:/Users/Piotr/pz/lab4/csv/orders_details.csv", orderDetail => new OrderDetail(){
                OrderId = orderDetail[0],
                ProductId = orderDetail[1],
                UnitPrice = orderDetail[2],
                Quantity = orderDetail[3],
                Discount = orderDetail[4]
            });

            orderDetails.Take(5).ToList().ForEach(orderDetail => Console.WriteLine($"OrderDetail: {orderDetail.OrderId}, {orderDetail.ProductId}, {orderDetail.UnitPrice}, {orderDetail.Quantity}, {orderDetail.Discount}"));

            // Zad 1
            Console.WriteLine("\n\nZad 1: \n");
            var employeesLastNames = getAllLastNames(employees);
            employeesLastNames.ForEach(employee => Console.WriteLine($"Employee: {employee}"));

            // Zad 2
            Console.WriteLine("\n\nZad 2: \n");
            var query = from r in regions
                join t in territories on r.Id equals t.RegionId
                join et in employeeTerritories on t.Id equals et.TerritoryId
                join e in employees on et.EmployeeId equals e.Id
                select new
                {
                    LastName = e.LastName,
                    Region = r.RegionDescription,
                    Territory = t.TerritoryDescription
                };

            query.ToList().ForEach(q => Console.WriteLine($"Employee: {q.LastName}, region: {q.Region}, territory: {q.Territory}"));

                // Zad 3
            Console.WriteLine("\n\nZad 3: \n");
            var query2 = from r in regions
                join t in territories on r.Id equals t.RegionId
                join et in employeeTerritories on t.Id equals et.TerritoryId
                join e in employees on et.EmployeeId equals e.Id
                group e by r.RegionDescription into g
                select new
                {
                    Region = g.Key,
                    Employees = g.Select(e => e.LastName).ToList()
                };

            query2.ToList().ForEach(q => Console.WriteLine($"Region: {q.Region}, employees: {String.Join(", ", q.Employees.Distinct())}"));

                // Zad 4
            Console.WriteLine("\n\nZad 4: \n");
            var query3 = from r in regions
                join t in territories on r.Id equals t.RegionId
                join et in employeeTerritories on t.Id equals et.TerritoryId
                join e in employees on et.EmployeeId equals e.Id
                group e by r.RegionDescription into g
                select new
                {
                    Region = g.Key,
                    Employees = g.Distinct().Count()
                };

            query3.ToList().ForEach(q => Console.WriteLine($"Region: {q.Region}, employees: {q.Employees}"));
            
            // Zad 5
            Console.WriteLine("\n\nZad 5: \n");
            var ordersByEmployee = orders.Join(orderDetails, o => o.Id, od => od.OrderId, (o, od) => new
                {
                    EmployeeID = o.EmployeeId, 
                    OrderTotal = float.Parse(od.UnitPrice, new CultureInfo("en-US")) * int.Parse(od.Quantity)
                })
                .GroupBy(o => o.EmployeeID)
                .ToDictionary(g => g.Key, g => new
                {
                    OrderCount = g.Count(), 
                    OrderAvg = g.Average(o => o.OrderTotal), 
                    OrderMax = g.Max(o => o.OrderTotal)
                });

            foreach (var employee in ordersByEmployee)
            {
                Console.WriteLine(@$"Pracownik {employee.Key}:
                Liczba zamówień: {employee.Value.OrderCount}
                Średnia wartość zamówienia: {employee.Value.OrderAvg:C}
                Największa wartość zamówienia: {employee.Value.OrderMax:C}
                ");
            }
        }

        public static List<string> getAllLastNames(List<Employee> employees)
        {
            return employees.Select(e => e.LastName).ToList();
        }
    }
};