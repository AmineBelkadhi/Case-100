using MongoDB.Bson;
using System;

public class Account 
{
    public ObjectId id;

    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }

    public int connectionId;
    public string token;
    public int status;
    public DateTime lastlogin { get; set; }
}
