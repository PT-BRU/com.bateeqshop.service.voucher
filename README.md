# com.bateeqshop.service.voucher# com.bateeqshop.service.voucher

Installation :
1. Create file appsettings.json in 'com.bateeqshop.service.voucher.api' Project with the following content :
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb);Initial Catalog=bateeqshop-voucher-dev;Persist Security Info=False;User ID=(localuser);Password=(localpassword);Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
2. Update (localdb), (localuser), (localpassword) based on Sql Server Database installed in your development machine or Cloud
3. Delete existing database or uncomment 'context.Database.EnsureDeleted()' in Program.cs to ensure latest schema installation (Backup your data if necessary & undo Program.cs changes if you previously uncomment 'context.Database.EnsureDeleted()' line)

Important Notes before Production :
1. Remove 'context.Database.EnsureCreated' line in Program.cs
2. Use proper Migration method & put 'Scaled Out Database' in consideration when migrating
