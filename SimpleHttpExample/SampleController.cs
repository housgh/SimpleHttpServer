using System.Net;
using SimpleHttpExample.Server.Attributes;
using SimpleHttpExample.Server.Models;

namespace SimpleHttpExample.Client;


[Controller]
public class SampleController : BaseController
{
    private static int _currentId = 4;
    private static readonly List<User> Db = new()
    {
        new User(1, "Houssam"),
        new User(2, "Tom"),
        new User(3, "Sasha")
    };

    [GetRequest("{id}")]
    public HttpResult GetAsync(int id)
    {
        var user = Db.Find(u => u.Id == id);
        return user is null ? 
            new HttpResult(HttpStatusCode.NotFound) : 
            new HttpResult(HttpStatusCode.OK, user);
    }

    [PostRequest]
    public HttpResult AddAsync(UserDto userDto)
    {
        var user = new User(_currentId++, userDto.Name);
        Db.Add(user);
        return new HttpResult(HttpStatusCode.Created, user);
    }

    [PutRequest("{id}")]
    public HttpResult UpdateUser(int id, UserDto userDto)
    {
        var user = new User(id, userDto.Name);
        var userToUpdate = Db.Find(u => u.Id == id);
        if (userToUpdate is null) return new HttpResult(HttpStatusCode.NotFound);
        var index = Db.IndexOf(userToUpdate);
        Db[index] = user;
        return new HttpResult(HttpStatusCode.OK, user);
    }

    [DeleteRequest("{id}")]
    public HttpResult DeleteAsync(int id)
    {
        var userToDelete = Db.Find(u => u.Id == id);
        if(userToDelete is null) return new HttpResult(HttpStatusCode.NotFound);
        Db.Remove(userToDelete);
        return new HttpResult(HttpStatusCode.OK);
    }
}

public class User
{
    public User(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public int Id { get; set; }
    public string Name { get; set; }
}

public class UserDto
{
    public string Name { get; set; }
}