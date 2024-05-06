namespace Dataverse.Http.Connector.Core.Test
{
    [TestClass]
    public class Employee
    {
        private Guid _id = Guid.Empty;

        private IServiceCollection _services;

        private IServiceProvider _provider;

        private IDataverseContext _dataverse;

        [TestInitialize]
        public void Initialize()
        {
            _services = new ServiceCollection();
            _services.AddDataverseContext<DataverseContext>(builder =>
            {
                builder.SetDefaultConnection(Conn.Dataverse.Connection);
                builder.SetThrowExceptions(true);
                builder.AddEntitiesFromAssembly(typeof(Employees).Assembly);
            });
            _provider = _services.BuildServiceProvider();
            _dataverse = _provider.GetService<IDataverseContext>()!;
        }

        [TestMethod]
        public async Task Should_Get_Employees_List()
        {
            try
            {
                var employees = await _dataverse.Set<Employees>()
                    .FilterAnd(conditions =>
                    {
                        conditions.NotEqual(c => c.Id, Guid.NewGuid());
                        conditions.Equal(c => c.StatusCode, 1);
                        conditions.Equal(c => c.StateCode, 0);
                    })
                    .Top(10)
                    .ToListAsync();

                employees.Count.Should().BeGreaterThan(0);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error ocurred during the query to the dataverse's entity with name '{nameof(Employees)}'.", ex);
            }
        }

        [TestMethod]
        public async Task Should_Get_Employees_Paged_List()
        {
            try
            {
                var pagedResponse = await _dataverse.Set<Employees>()
                    .FilterAnd(conditions =>
                    {
                        conditions.Equal(x => x.IsDeleted, false);
                        conditions.ThisYear(x => x.CreatedOn);
                    })
                    .ToPagedListAsync(1, 3);

                pagedResponse.Should().NotBeNull();
                pagedResponse.Results.Count.Should().BeGreaterThan(0);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error ocurred during the query to the dataverse's entity with name '{nameof(Employees)}'.", ex);
            }
        }

        [TestMethod]
        public async Task Should_Get_Employees_Paged_List_Filtered_By_Name_Coincidence()
        {
            try
            {
                var pagedResponse = await _dataverse.Set<Employees>()
                    .FilterAnd(conditions =>
                    {
                        conditions.Like(c => c.Name, "%E%");
                    })
                    .ToPagedListAsync(1);

                pagedResponse.Should().NotBeNull();
                pagedResponse.Results.Count.Should().BeGreaterThan(0);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error ocurred during the query to the dataverse's entity with name '{nameof(Employees)}'.", ex);
            }
        }

        [TestMethod]
        public async Task Should_Get_First_Employee_Record()
        {
            try
            {
                var employee = await _dataverse.Set<Employees>().FirstAsync();
                employee.Should().NotBeNull();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error ocurred during the query to the dataverse's entity with name '{nameof(Employees)}'.", ex);
            }
        }

        [TestMethod]
        public async Task Should_Create_New_Employee_Record()
        {
            try
            {
                Employees employee = new()
                {
                    Name = "Barry Allen",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    StatusCode = 1,
                    StateCode = 0,
                    IsDeleted = false,
                    OwnerId = new("ef4269c5-a4f2-ec11-bb3d-00224820d6d5")
                };

                var entity = await _dataverse.Set<Employees>().AddAsync(employee);

                entity.Should().NotBe(null);
                entity?.Id.Should().NotBe(Guid.Empty);

                _id = entity!.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error has ocurred during the creation of {nameof(Employees)} record", ex);
            }
        }

        [TestMethod]
        public async Task Should_Delete_Employee_Record()
        {
            try
            {
                if (_id == Guid.Empty)
                    await Should_Create_New_Employee_Record();

                var employee = await _dataverse.Set<Employees>()
                    .FilterAnd(conditions => conditions.Equal(x => x.Id, _id))
                    .FirstOrDefaultAsync();

                await _dataverse.Set<Employees>().DeleteAsync(employee!);

                employee = await _dataverse.Set<Employees>()
                    .FilterAnd(conditions => conditions.Equal(x => x.Id, _id))
                    .FirstOrDefaultAsync();

                employee.Should().BeNull();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error has ocurred during the creation of {nameof(Employees)} record", ex);
            }
        }

        [TestMethod]
        public async Task Should_Update_Employee_Record()
        {
            try
            {
                if(_id == Guid.Empty)
                    await Should_Create_New_Employee_Record();

                var employee = await _dataverse.Set<Employees>()
                    .FilterAnd(conditions => conditions.Equal(x => x.Id, _id))
                    .FirstOrDefaultAsync();

                employee!.Name = "Michelle Jones";
                employee!.IsDeleted = true;

                var entity = await _dataverse.Set<Employees>().UpdateAsync(employee!);

                entity.Should().NotBeNull();
                entity!.Name.Should().Be(employee.Name);
                entity!.IsDeleted.Should().Be(employee.IsDeleted);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error has ocurred during the creation of {nameof(Employees)} record", ex);
            }
        }
    }
}
