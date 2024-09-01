using KH.Helper.Contracts.Persistence;
using KH.Helper.Responses;
public class UserService
{
  private readonly IUnitOfWork _unitOfWork;

  public UserService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  // Add a new user
  public async Task AddUserAsync(User user)
  {
    var repository = _unitOfWork.Repository<User>();
    await repository.AddAsync(user);
    await _unitOfWork.CommitAsync();
  }

  // Add multiple users
  public async Task AddUsersAsync(IEnumerable<User> users)
  {
    var repository = _unitOfWork.Repository<User>();
    await repository.AddRangeAsync(users.ToList());
    await _unitOfWork.CommitAsync();
  }

  public async Task AddUsersWithTransactionAsync(IEnumerable<User> users)
  {
    await _unitOfWork.BeginTransactionAsync();
    try
    {
      var repository = _unitOfWork.Repository<User>();
      await repository.AddRangeAsync(users.ToList());
      await _unitOfWork.CommitAsync();
      await _unitOfWork.CommitTransactionAsync();
    }
    catch
    {
      await _unitOfWork.RollBackTransactionAsync();
      throw;
    }
  }

  // Get paginated users with optional filters and includes
  public async Task<PagedList<User>> GetPagedUsersWithIncludesAsync(
      int pageNumber,
      int pageSize,
      string firstName = null,
      string lastName = null,
      string email = null,
      string mobileNumber = null)
  {
    var repository = _unitOfWork.Repository<User>();
    IQueryable<User> query = repository.GetQueryable();

    if (!string.IsNullOrEmpty(firstName))
    {
      query = query.Where(u => u.FirstName.Contains(firstName));
    }

    if (!string.IsNullOrEmpty(lastName))
    {
      query = query.Where(u => u.LastName.Contains(lastName));
    }

    if (!string.IsNullOrEmpty(email))
    {
      query = query.Where(u => u.Email.Contains(email));
    }

    if (!string.IsNullOrEmpty(mobileNumber))
    {
      query = query.Where(u => u.MobileNumber.Contains(mobileNumber));
    }

    query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                 .Include(u => u.UserGroups)
                 .Include(u => u.UserDepartments);

    return await repository.GetPagedUsingQueryAsync(pageNumber, pageSize, query);
  }

  public async Task<PagedList<User>> GetPagedUsersWithIncludesAsyncVersion2(
     int pageNumber,
     int pageSize,
     string firstName = null,
     string lastName = null,
     string email = null,
     string mobileNumber = null)
  {
    var repository = _unitOfWork.Repository<User>();
    IQueryable<User> query = repository.GetQueryable();


    //example with internal predicate
    var result =  await repository.GetPagedAsync(pageNumber, pageSize, u =>
    u.FirstName.Contains("search")
    || u.LastName.Contains("search2"),
    q => q.Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
    .Include(u => u.UserDepartments));

    return result;
  }


  // Get all users with optional filters and includes
  public async Task<IEnumerable<User>> GetAllUsersWithIncludesAsync(
      string firstName = null,
      string lastName = null,
      string email = null,
      string mobileNumber = null)
  {
    var repository = _unitOfWork.Repository<User>();
    IQueryable<User> query = repository.GetQueryable();

    if (!string.IsNullOrEmpty(firstName))
    {
      query = query.Where(u => u.FirstName.Contains(firstName));
    }

    if (!string.IsNullOrEmpty(lastName))
    {
      query = query.Where(u => u.LastName.Contains(lastName));
    }

    if (!string.IsNullOrEmpty(email))
    {
      query = query.Where(u => u.Email.Contains(email));
    }

    if (!string.IsNullOrEmpty(mobileNumber))
    {
      query = query.Where(u => u.MobileNumber.Contains(mobileNumber));
    }

    query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                 .Include(u => u.UserGroups)
                 .Include(u => u.UserDepartments);

    return await query.ToListAsync();
  }

  // Get a user by ID
  public async Task<User> GetUserByIdAsync(long id)
  {
    var repository = _unitOfWork.Repository<User>();
    return await repository.GetAsync(id);
  }

  // Get a user by ID with includes
  public async Task<User> GetUserByIdWithIncludesAsync(long id)
  {
    var repository = _unitOfWork.Repository<User>();
    return await repository.GetAsync(id, q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                              .Include(u => u.UserGroups)
                                              .Include(u => u.UserDepartments));
  }

  // Get a user by ID with tracking
  public async Task<User> GetUserByIdWithTrackingAsync(long id)
  {
    var repository = _unitOfWork.Repository<User>();
    return await repository.GetAsyncTracking(id, q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                                      .Include(u => u.UserGroups)
                                                      .Include(u => u.UserDepartments));
  }

  // Find users by expression
  public IEnumerable<User> FindUsers(Expression<Func<User, bool>> predicate)
  {
    var repository = _unitOfWork.Repository<User>();
    return repository.FindBy(predicate);
  }

  // Find users by expression with includes
  public async Task<IEnumerable<User>> FindUsersWithIncludesAsync(Expression<Func<User, bool>> predicate)
  {
    var repository = _unitOfWork.Repository<User>();

    //example with internal predicate
    var result = await repository.FindByAsync(u =>
    u.FirstName.Contains("search")
    || u.LastName.Contains("search2"),

    q => q.Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
    .Include(u => u.UserDepartments));

    return await repository.FindByAsync(predicate, q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                                        .Include(u => u.UserGroups)
                                                        .Include(u => u.UserDepartments));
  }

  // Count users
  public int CountUsers()
  {
    var repository = _unitOfWork.Repository<User>();
    return repository.Count();
  }

  // Count users asynchronously
  public async Task<int> CountUsersAsync()
  {
    var repository = _unitOfWork.Repository<User>();
    return await repository.CountAsync();
  }

  // Count users by expression
  public async Task<int> CountUsersByAsync(Expression<Func<User, bool>> predicate)
  {
    var repository = _unitOfWork.Repository<User>();
    return await repository.CountByAsync(predicate);
  }

  // Update a user
  public async Task UpdateUserAsync(User user)
  {
    var repository = _unitOfWork.Repository<User>();
    repository.Update(user);
    await _unitOfWork.CommitAsync();
  }

  // Update multiple users
  public async Task UpdateUsersAsync(IEnumerable<User> users)
  {
    var repository = _unitOfWork.Repository<User>();
    repository.UpdateRange(users.ToList());
    await _unitOfWork.CommitAsync();
  }

  // Delete a user
  public async Task DeleteUserAsync(long id)
  {
    var repository = _unitOfWork.Repository<User>();
    var user = await repository.GetAsync(id);
    if (user != null)
    {
      repository.Delete(user);
      await _unitOfWork.CommitAsync();
    }
  }

  // Get users as queryable (for custom querying)
  public IQueryable<User> GetUsersQueryable()
  {
    var repository = _unitOfWork.Repository<User>();
    return repository.GetQueryable();
  }
}

