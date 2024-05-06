var dataService = new DataService.DataService();
var userAccount = dataService.GetUserById(1);
Console.WriteLine("userAccount, {0}", userAccount.PhoneNumber);