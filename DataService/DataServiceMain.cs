﻿using System;
using DataService.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using DataService;
using Humanizer;
using static Humanizer.On;

namespace DataService
{
	public class DataServiceMain : IDataService
	{

        public DataServiceMain()
		{
        }
        Random rand = new Random();

        /* Reusable functions */

        public UserAccount GetUserById(int id)
        {
            microloan_dbContext db = new();
            var user = db.UserAccounts
            .Where(x => x.Id == id)
            .FirstOrDefault();
            return user;
        }

        public UserAccount GetUserByMail(string email)
        {
            microloan_dbContext db = new();
            var user = db.UserAccounts
            .Where(x => x.EmailAdress == email)
            .FirstOrDefault();
            return user;
        }

        public bool CreateInvestor(string email)
        {
            var user = GetUserByMail(email);

            microloan_dbContext db = new();

            var investor = new Investor
            {
                Id = rand.Next(1, 900000000 + 1),
                UserAccountId = user.Id
            };

            // Add the user to the database
            db.Investors.Add(investor);
            db.SaveChanges();
            return true;
        }

        public bool CreateBorrower(string email)
        {
            var user = GetUserByMail(email);

            microloan_dbContext db = new();

            var borrower = new Borrower
            {
                Id = rand.Next(1, 900000000 + 1),
                UserAccountId = user.Id
            };

            // Add the user to the database
            db.Borrowers.Add(borrower);
            db.SaveChanges();
            return true;
        }

        public Tuple<bool, string> RegisterUser(string email, decimal phone, string password, string confirmPassword, bool isInvestor )
        {
            var findUser = GetUserByMail(email);
            if (findUser != null)
            {
                return Tuple.Create(false, "User already exists");
            }
            if (password != confirmPassword)
            {
                return Tuple.Create(false, "Passwords do not match");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            

            var user = new UserAccount
            {
                Id = rand.Next(1, 900000000 + 1),
                EmailAdress = email,
                PhoneNumber = phone,
                Password = hashedPassword,
                IsInvestor = isInvestor
            };


            microloan_dbContext db = new();

            // Add the user to the database
            db.UserAccounts.Add(user);
            db.SaveChanges();

            if (isInvestor)
            {
                CreateInvestor(email);
            }
            else
            {
                CreateBorrower(email);
            }

            return Tuple.Create(true, "It was succesful");
        }

        public Tuple<bool, string> LoginUser(string email, string password)
        {
            var user = GetUserByMail(email);

            if (user == null)
            {
                return Tuple.Create(false, "User does not exist");
            }

            // Compare the provided password with the hashed password stored in the database
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!passwordMatch)
            {
                return Tuple.Create(false, "Incorrect password");
            }

            return Tuple.Create(true, "Login successful");
        }



    }
}
