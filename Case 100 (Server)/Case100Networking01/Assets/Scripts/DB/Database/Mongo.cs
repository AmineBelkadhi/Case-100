using MongoDB.Driver;
using MongoDB.Driver.Builders;
using UnityEngine;

public class Mongo 
{
    private const string mongoURI = "mongodb://127.0.0.1:27017/?gssapiServiceName=mongodb";
    private const string DATA_BASE_NAME = "accounts";

    private MongoClient client;
    private MongoServer server;
    private MongoDatabase db;

    private MongoCollection<Account> accounts;

    public void Init()
    {
        client = new MongoClient(mongoURI);
        server = client.GetServer();
        db = server.GetDatabase(DATA_BASE_NAME);

        Debug.Log("Database setup don!");
        //accounts= db.GetCollection<>
        accounts = db.GetCollection<Account>("info");
    }
    
    public void ShutDown()
    {
        client = null;
        server.Shutdown();
        db = null;
    }

    public bool InsertAccount(string username, string password, string email)
    {

        Account acc= new Account();

        if(LookForAccountEmail(email) != null)
        {
            Debug.Log("Email Already Exists");
            return false;
        }
               

        acc.username = username;
        acc.password = password;
        acc.email = email;

        accounts.Insert(acc);

        return true;
    }

    public Account LookForAccountEmail(string email)
    {
        var query = Query<Account>.EQ(acc => acc.email, email);
        return accounts.FindOne(query);
    }

    public Account LookForAccount(string email, string password)
    {
        var query = Query.And(Query<Account>.EQ(acc => acc.email, email), Query<Account>.EQ(acc => acc.password, password));
        return accounts.FindOne(query);
    }

    public Account LoginAccount(string usernameOrEmail, string password, int connectionId, string token)
    {
        Account myAccount = null;
        IMongoQuery query = null;

        if (Utility.isEmail(usernameOrEmail))
        {
            query = Query.And(Query<Account>.EQ(acc => acc.email, usernameOrEmail),
                Query<Account>.EQ(acc => acc.password, password));

            myAccount = accounts.FindOne(query);
        }
        else
        {
            query = Query.And(Query<Account>.EQ(acc => acc.email, usernameOrEmail),
                Query<Account>.EQ(acc => acc.password, password));

            myAccount = accounts.FindOne(query);
        }

        if (myAccount != null)
        {
            myAccount.connectionId = connectionId;
            myAccount.status = 1;
            myAccount.token = token;
            myAccount.lastlogin = System.DateTime.Now;

            accounts.Update(query, Update<Account>.Replace(myAccount));
        }

        return myAccount;
    }
}
