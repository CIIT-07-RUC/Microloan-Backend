var dataService = new DataService.DataService();
var userAccount = dataService.GetUserById(1);

var registerUser = dataService.RegisterUser("emailtest@gmail", 2020202, "dsadsad", "dsadsad");


Console.WriteLine("userAccount, {0}", registerUser);