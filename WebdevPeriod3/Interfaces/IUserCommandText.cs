namespace WebdevPeriod3.Interfaces
{
    public interface IUserCommandText
    {
        string GetAllUsers { get; }
        string GetUserById { get; }
        string AddUser { get; }
        string UpdateUser { get; }
        string RemoveUser { get; }
    }

    public class UserCommandText : IUserCommandText
    {
        public string GetAllUsers => "Select * From Users";
        public string GetUserById => "Select * From Users Where Id = @Id";
        public string AddUser => "Insert Into Users (Username, Password) Values (@Username, @Password)";
        public string UpdateUser => "Update Users set Username = @Username, Password = @Password Where Id = @Id";
        public string RemoveUser => "Delete from Users Where Id = @Id";
    }
}
